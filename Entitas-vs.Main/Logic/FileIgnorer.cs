using Microsoft.VisualStudio.Shell.Interop;

namespace EntitasVSGenerator.Logic
{
    class FileIgnorer
    {
        private IVsFileChangeEx _vsFileChangeEx;

        public FileIgnorer(IVsFileChangeEx vsFileChangeEx)
        {
            _vsFileChangeEx = vsFileChangeEx;
        }

        public void IgnoreFile(string path)
        {
            IgnoreFile(path, true);
        }

        public void UnignoreFile(string path)
        {
            IgnoreFile(path, false);
        }

        private void IgnoreFile(string path, bool shouldIgnore)
        {
            _vsFileChangeEx.IgnoreFile(0, path, shouldIgnore ? 1 : 0);
        }
    }
}
