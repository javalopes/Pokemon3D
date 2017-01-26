using System;
using System.Collections.Generic;

namespace Pokemon3D.GameCore
{
    public class GameEvent
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public const string GameQuitToMainMenu = "GAME_QUIT_TO_MAIN_MENU";

        public const string Inventory = "INVENTORY";

        public GameEvent(object sender, string category, TimeSpan delay)
        {
            Sender = sender;
            Category = category;
            Delay = delay;
        }

        public GameEvent WithProperty(string name, object value)
        {
            this[name] = value;
            return this;
        }

        public object this[string propertyName]
        {
            get { return _properties[propertyName]; }
            set { _properties[propertyName] = value; }
        }

        public T GetProperty<T>(string name)
        {
            return (T)_properties[name];
        }

        public string Category { get; private set; }

        public object Sender { get; private set; }

        public TimeSpan Delay { get; set; }
    }
}