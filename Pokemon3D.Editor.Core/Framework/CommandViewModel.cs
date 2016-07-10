using System;
using System.Windows.Input;

namespace Pokemon3D.Editor.Core.Framework
{
    public class CommandViewModel : ViewModel, ICommand
    {
        private readonly Action<object> _action;
        private bool _isEnabled;
        private string _caption;

        public event EventHandler CanExecuteChanged;

        public CommandViewModel(Action<object> action, string caption)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _action = action;
            IsEnabled = true;
            Caption = caption;
        }

        public CommandViewModel(Action action, string caption) : this(o => action(), caption)
        {
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

        public string Caption
        {
            get { return _caption; }
            set { SetProperty(ref _caption, value); }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
