using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entitas_vs.View.ViewModels
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected virtual void SetValue<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            backingField = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
