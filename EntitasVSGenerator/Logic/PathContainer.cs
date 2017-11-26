using EntitasVSGenerator.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntitasVSGenerator.Logic
{
    public class PathContainer
    {
        private IEnumerable<string> _pathTriggers;
        private readonly string _projectDirectory;

        public IEnumerable<string> Triggers
        {
            get => _pathTriggers;
            set
            {
                _pathTriggers = value
                    .ToAbsolutePaths(_projectDirectory)
                    .Select(path => path.ToLower());
            }
        }

        public PathContainer(IEnumerable<string> paths, string projectDirectory)
        {
            _projectDirectory = projectDirectory;
            Triggers = paths;
        }

        public bool Contains(string filePath)
        {
            string filePathLower = filePath.ToLower();
            string directory = Path.GetDirectoryName(filePathLower);
            return _pathTriggers.Contains(filePathLower) || _pathTriggers.Contains(directory);
        }
    }
}
