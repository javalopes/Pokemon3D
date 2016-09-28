using System.Collections.Generic;
using System.Reflection;
using Pokemon3D.Scripting.Types;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// Holds a link back into the scripting engine to set fields of a script object.
    /// </summary>
    public sealed class ScriptObjectLink
    {
        private SObject _objReference;
        private ScriptProcessor _processor;

        internal ScriptObjectLink(ScriptProcessor processor, SObject obj)
        {
            _objReference = obj;
            _processor = processor;
        }

        /// <summary>
        /// Sets a member of the original script object.
        /// </summary>
        public void SetMember(string identifier, object value)
        {
            SObject data = ScriptInAdapter.Translate(_processor, value);

            _objReference.SetMember(_processor, _processor.CreateString(identifier), false, data);
        }

        /// <summary>
        /// Sets a member of the original script object and updates the .net object with the new value as well.
        /// </summary>
        public void SetMember(string identifier, object value, object netObject)
        {
            SetMember(identifier, value);

            var field = netObject.GetType().GetField(identifier, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                field.SetValue(netObject, value);
        }

        public void SetReference(string identifier, object reference)
        {
            if (_objReference is SProtoObject)
            {
                var protoObj = _objReference as SProtoObject;

                if (protoObj.ReferenceContainer == null)
                    protoObj.ReferenceContainer = new Dictionary<string, object>();

                if (protoObj.ReferenceContainer.ContainsKey(identifier))
                    protoObj.ReferenceContainer[identifier] = reference;
                else
                    protoObj.ReferenceContainer.Add(identifier, reference);
            }
        }

        public object GetReference(string identifier)
        {
            object returnValue = null;

            if (_objReference is SProtoObject)
            {
                var protoObj = _objReference as SProtoObject;

                if (protoObj.ReferenceContainer != null)
                    protoObj.ReferenceContainer.TryGetValue(identifier, out returnValue);
            }
            
            return returnValue;
        }
    }
}
