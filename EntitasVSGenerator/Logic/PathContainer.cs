using EntitasVSGenerator.Extensions;
using MoreLinq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntitasVSGenerator.Logic
{
    public class PathContainer
    {
        private HashSet<string> _pathTriggers;
        private readonly string _projectDirectory;
        
        public IEnumerable<string> Paths
        {
            set
            {
                if (value == null)
                    _pathTriggers = new HashSet<string>();
                else
                    _pathTriggers = value
                        .ToAbsolutePaths(_projectDirectory)
                        .Select(path => path.ToLower())
                        .ToHashSet();
            }
        }

        public PathContainer(IEnumerable<string> paths, string projectDirectory)
        {
            _projectDirectory = projectDirectory;
            Paths = paths;
        }

        public bool Contains(string filePath)
        {
            string filePathLower = filePath.ToLower();
            string directory = Path.GetDirectoryName(filePathLower);
            return _pathTriggers.Contains(filePathLower) || _pathTriggers.Contains(directory);
        }
    }
}
