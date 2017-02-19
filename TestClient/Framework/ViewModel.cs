using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestClient.Framework
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T backingField, T value, Action<T> onChanged = null, [CallerMemberName] string propertyName = null)
        {
            if (Equals(backingField, value)) return;
            var oldValue = backingField;
            backingField = value;
            OnPropertyChanged(propertyName);
            onChanged?.Invoke(oldValue);
        }
    }
}
