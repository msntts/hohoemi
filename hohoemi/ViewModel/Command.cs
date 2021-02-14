using System;
using System.Windows.Input;

namespace hohoemi.ViewModel
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Func<object, bool> canExecuteDelegator;

        private readonly Action<object> executeDelegator;

        private readonly Action<Exception> exceptionHandleDelegator;

        public Command(Func<object, bool> onCanExecuteCallback,
                        Action<object> onExecuteCallback,
                        Action<Exception> onExceptionCallback)
        {
            canExecuteDelegator = onCanExecuteCallback;
            executeDelegator = onExecuteCallback;
            exceptionHandleDelegator = onExceptionCallback;
        }

        public bool CanExecute(object parameter) => canExecuteDelegator?.Invoke(parameter) ?? true;
        public void Execute(object parameter)
        {
            try
            {
                executeDelegator?.Invoke(parameter);
            }
            catch (Exception e)
            {
                exceptionHandleDelegator?.Invoke(e);
            }
        }
    }
}
