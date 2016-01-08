using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pokemon3D.Editor.Core.Framework
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T value, Action<T> onChanged = null, [CallerMemberName]string propertyName = null)
        {
            if (Equals(backingField, value)) return false;

            var oldValue = backingField;
            backingField = value;

            OnPropertyChanged(propertyName);
            onChanged?.Invoke(oldValue);

            return true;
        }
    }
}
