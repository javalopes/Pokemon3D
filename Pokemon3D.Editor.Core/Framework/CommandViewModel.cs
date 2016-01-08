using System;
using System.Windows.Input;

namespace Pokemon3D.Editor.Core.Framework
{
    public class CommandViewModel : ViewModel, ICommand
    {
        private Action _action;
        private bool _isEnabled;

        public event EventHandler CanExecuteChanged;

        public CommandViewModel(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _action = action;
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
