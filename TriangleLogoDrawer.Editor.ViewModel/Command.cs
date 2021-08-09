using System;
using System.Windows.Input;

namespace TriangleLogoDrawer.Editor.ViewModel
{
    public class Command : ICommand
    {
        private Action action;
        private Func<bool> canExecute;

        internal Command(Action action)
        {
            this.action = action;
        }
        internal Command(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(canExecute == null)
            {
                return true;
            }
            else
            {
                return canExecute();
            }
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
