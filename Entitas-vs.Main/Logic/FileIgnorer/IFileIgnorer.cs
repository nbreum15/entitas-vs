namespace EntitasVSGenerator.Logic
{
    interface IFileIgnorer
    {
        void IgnoreFile(string path);
        void UnignoreFile(string path);
    }
}
