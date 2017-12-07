using System;
using System.Windows.Input;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    abstract class BaseCommand : ICommand
    {
        protected static EntitasVsPackage Package { get; private set; }

        protected BaseCommand(EntitasVsPackage package)
        {
            Package = package;
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
        public abstract event EventHandler CanExecuteChanged;
    }
}
