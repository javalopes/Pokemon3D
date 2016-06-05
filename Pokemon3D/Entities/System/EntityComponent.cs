using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// A component of an <see cref="Entity"/>, responsible for the Entity's functionality.
    /// </summary>
    class EntityComponent
    {
        private bool _isActive;

        /// <summary>
        /// Entity data as dictionary.
        /// </summary>
        private readonly Dictionary<string, string> _data ;

        /// <summary>
        /// The original name of this component.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The owning parent <see cref="Entity"/> of this component.
        /// </summary>
        protected Entity Parent { get; }

        protected EntityComponent(EntityComponentDataCreationStruct parameters)
        {
            Name = parameters.Name;
            _data = parameters.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Parent = parameters.Parent;
            _isActive = true;
        }

        protected EntityComponent(Entity parent)
        {
            Parent = parent;
            _isActive = true;
        }

        /// <summary>
        /// Sets active state of component.
        /// </summary>
        public bool IsActive
        {
            get { return Parent.IsActive && _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        /// <summary>
        /// Updates this property's logic.
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Converts the data string that came with the component data into the desired data type.
        /// </summary>
        public T GetData<T>(string key)
        {
            return TypeConverter.Convert<T>(_data[key]);
        }

        /// <summary>
        /// Gets a list of Data with starting text and indexing by number.
        /// Therefore each DataElement-Key is checked, whether it starts with the <see cref="baseKey"/> name and ends with a number.
        /// If this is the case, all these elements are ordered by the suffix number and return the values of them in the order.
        /// </summary>
        public T[] GetEnumeratedData<T>(string baseKey)
        {
            return _data.Where(d => IsBaseKeyWithNumberSuffix(d.Key, baseKey))
                .Select(d => CreateSuffixOrderable(d.Key, d.Value, baseKey))
                .OrderBy(d => d.Value)
                .Select(d => TypeConverter.Convert<T>(d.Key))
                .ToArray();
        }

        private static bool IsBaseKeyWithNumberSuffix(string fullKeyName, string baseKeyName)
        {
            if (fullKeyName == baseKeyName) return true;
            return fullKeyName.StartsWith(baseKeyName) && HasIntegerSuffix(fullKeyName, baseKeyName);
        }

        private static bool HasIntegerSuffix(string fullKeyName, string baseKeyName)
        {
            int integer;
            return int.TryParse(fullKeyName.Replace(baseKeyName, ""), out integer);
        }

        private static KeyValuePair<string, int> CreateSuffixOrderable(string fullKeyName, string valueOfFullKey, string baseKeyName)
        {
            return new KeyValuePair<string, int>(valueOfFullKey, GetIntegerSuffix(fullKeyName, baseKeyName));
        }

        private static int GetIntegerSuffix(string fullKeyName, string baseKeyName)
        {
            var suffixAsString = fullKeyName.Replace(baseKeyName, "");
            return string.IsNullOrEmpty(suffixAsString) ? 0 : int.Parse(suffixAsString);
        }

        /// <summary>
        /// Converts the data string that came with the component data into the desired data type. Returns default fullKeyName if not present.
        /// </summary>
        public T GetDataOrDefault<T>(string key, T defaultValue = default(T))
        {
            string value;
            if (_data.TryGetValue(key, out value))
            {
                return TypeConverter.Convert<T>(value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Sets a data value for a specific key for this component. If the key does not exist, a new data entry will be added.
        /// </summary>
        public void SetData(string key, string data)
        {
            if (_data.ContainsKey(key))
            {
                var oldData = _data[key];
                _data[key] = data;
                OnDataChanged(key, oldData, data);
            }
            else
            {
                _data.Add(key, data);
                OnDataChanged(key, null, data);
            }
        }

        /// <summary>
        /// Is called when the activation state has changed.
        /// </summary>
        public virtual void OnIsActiveChanged() { }

        /// <summary>
        /// Is called when the component has been added.
        /// IMPORTANT: When Entity is in initializing mode, defer intialization to OnInitialized()
        /// </summary>
        public virtual void OnComponentAdded() { }

        /// <summary>
        /// Called when the component has been removed.
        /// </summary>
        public virtual void OnComponentRemove() { }

        /// <summary>
        /// Is called when the entity has been created in initializing mode and is now read to initialize properly.
        /// </summary>
        public virtual void OnInitialized()
        {
        }

        protected virtual void OnDataChanged(string key, string oldData, string newData) { }
    }
}
