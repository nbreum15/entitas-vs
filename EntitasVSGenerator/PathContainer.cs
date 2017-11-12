using MoreLinq;
using System.Collections.Generic;

namespace EntitasVSGenerator
{
    public class PathContainer
    {
        readonly HashSet<string> _paths = new HashSet<string>();

        private IEnumerable<string> _pathsFromModel;

        public PathContainer(IEnumerable<string> pathsFromModel)
        {
            _pathsFromModel = pathsFromModel;
        }

        public bool Contains(string path)
        {
            return _pathsFromModel.ToHashSet().Contains(path);
        }
    }
}
