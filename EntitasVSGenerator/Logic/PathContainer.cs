using EntitasVSGenerator.Extensions;
using MoreLinq;
using System.Collections.Generic;
using System.IO;

namespace EntitasVSGenerator.Logic
{
    public class PathContainer
    {
        private HashSet<string> _pathTriggers;
        private readonly string _projectDirectory;

        private IEnumerable<string> _paths;
        public IEnumerable<string> Paths
        {
            get => _paths;
            set
            {
                if (value == null)
                    _pathTriggers = new HashSet<string>();
                else
                    _pathTriggers = value.ToAbsolutePaths(_projectDirectory).ToHashSet();
            }
        }

        public PathContainer(IEnumerable<string> paths, string projectDirectory)
        {
            _projectDirectory = projectDirectory;
            Paths = paths;
        }

        public bool Contains(string filePath)
        {
            // check if file or folder which file rests in is in triggers
            string directory = Path.GetDirectoryName(filePath);
            return _pathTriggers.Contains(filePath) || _pathTriggers.Contains(directory);
        }
    }
}
