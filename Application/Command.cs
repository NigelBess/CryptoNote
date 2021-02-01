using System;
using System.Windows.Input;

namespace Application
{
    public class Command:ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            canExecute ??= O => true;
            Initialize(execute, canExecute);
        }
        public Command(Action execute, Func< bool> canExecute = null)
        {
            canExecute ??= () => true;

            Initialize(o=>execute(), o=>canExecute());
        }
        private void Initialize(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
