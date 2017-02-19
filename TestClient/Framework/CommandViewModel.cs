using System;
using System.Windows.Input;

namespace TestClient.Framework
{
    public class CommandViewModel : ViewModel, ICommand
    {
        private readonly Action _action;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value, o => CanExecuteChanged?.Invoke(this, EventArgs.Empty)); }
        }

        public CommandViewModel(Action action)
        {
            _action = action;
            IsEnabled = true;
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
