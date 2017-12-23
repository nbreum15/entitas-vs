namespace Entitas_vs.Main.Logic
{
    interface IDirectoryChangeNotifier
    {
        void AddListener(IDirectoryChangeListener listener);
        void RemoveListener(IDirectoryChangeListener listener);
        void ClearListeners();
    }
}