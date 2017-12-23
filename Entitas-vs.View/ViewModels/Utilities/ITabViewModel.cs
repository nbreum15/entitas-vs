using System.Collections.ObjectModel;

namespace Entitas_vs.View.ViewModels
{
    interface ITabViewModel
    {
        string Name { get; }
        bool IsSelected { get; set; }
        ObservableCollection<ITabViewModel> Children { get; }
        void AddChild(ITabViewModel child);
    }
}
