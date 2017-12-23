using System.Linq;
using Entitas_vs.Common;

namespace Entitas_vs.Main.Logic
{
    class RelativeDirectoryChangeListener : DirectoryChangeListener
    {
        private readonly string _appendPath;

        /// <summary>
        /// Creates an instance of a relative directory change listener using relative paths instead of absolute.
        /// </summary>
        /// <param name="appendPath">Path that is appended to relative paths.</param>
        public RelativeDirectoryChangeListener(string appendPath)
        {
            _appendPath = appendPath.ToLower();
        }

        public override void Add(params string[] paths)
        {
            base.Add(paths.ToAbsolutePaths(_appendPath).ToArray());
        }

        public override void Remove(params string[] paths)
        {
            base.Remove(paths.ToAbsolutePaths(_appendPath).ToArray());
        }
    }
}
