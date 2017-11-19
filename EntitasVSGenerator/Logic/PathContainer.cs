using EntitasVSGenerator.Extensions;
using MoreLinq;
using System.Collections.Generic;

namespace EntitasVSGenerator.Logic
{
    public class PathContainer
    {
        private HashSet<string> _pathTriggers;

        public PathContainer(IEnumerable<string> paths, string projectDirectory)
        {
            if (paths == null)
                _pathTriggers = new HashSet<string>();
            else
                _pathTriggers = paths.ToAbsolutePaths(projectDirectory).ToHashSet();
        }

        public bool Contains(string path)
        {
            return _pathTriggers.ToHashSet().Contains(path);
        }
    }
}
