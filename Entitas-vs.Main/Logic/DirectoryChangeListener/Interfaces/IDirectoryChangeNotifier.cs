namespace EntitasVSGenerator.Logic
{
    interface IDirectoryChangeNotifier
    {
        void Add(IDirectoryChangeListener listener);
        void Remove(IDirectoryChangeListener listener);
        void Clear();
    }
}