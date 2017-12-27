using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace Entitas_vs.View.Commands
{
    class ModifyAtIndexCommand : ICommand
    {
        private readonly IList _collection;
        private readonly Func<Window> _createWindow;

        public ModifyAtIndexCommand(IList collection, Func<Window> createWindow)
        {
            _collection = collection;
            _createWindow = createWindow;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var view = _createWindow();
            var dataContext = _collection[(int) parameter];
            view.DataContext = dataContext;
            view.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}
