namespace EntitasVSGenerator.Logic
{
    interface IDirectoryChangeNotifier
    {
        void AddListener(IDirectoryChangeListener listener);
        void RemoveListener(IDirectoryChangeListener listener);
        void ClearListeners();
    }
}