using System;
using System.Collections;
using System.Windows.Input;

namespace Entitas_vs.View.Commands
{
    class RemoveAtIndexCommand : ICommand
    {
        protected IList Collection { get; }

        public RemoveAtIndexCommand(IList collection)
        {
            Collection = collection;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            int index = (int) parameter;
            if (index >= 0 && index < Collection.Count)
            {
                Collection.RemoveAt(index);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
