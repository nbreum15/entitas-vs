using EnvDTE;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EntitasVSGenerator
{
    public class PathContainer
    {
        private string SettingsName => "entitas-vs.cfg";
        private string SettingsPath => $@"{SolutionDirectory}\{SettingsName}";

        readonly HashSet<string> _paths = new HashSet<string>();
        public string SolutionDirectory { get; }

        public event Action<string[]> Changed;

        public PathContainer(string solutionFilePath)
        {
            SolutionDirectory = Path.GetDirectoryName(solutionFilePath);
        }

        public string[] AllPaths => _paths.ToArray();

        public void Add(string path)
        {
            _paths.Add(path);
            OnChanged(AllPaths);
        }

        public void Remove(string path)
        {
            if (_paths.Contains(path))
            {
                _paths.Remove(path);
                OnChanged(AllPaths);
            }
        }

        public void LoadPaths()
        {
            if (!File.Exists(SettingsPath))
                return;
            File.ReadAllLines(SettingsPath).ForEach(path => Add(path));
        }

        public void SavePaths(string[] contentPaths)
        {
            File.WriteAllText(SettingsPath, contentPaths.ToDelimitedString("\n"));
        }

        public bool Contains(string path)
        {
            // if path is a file.
            if (_paths.Contains(path))
            {
                return true;
            }
            else // if path is file in a directory.
            {
                if (File.Exists(path))
                {
                    string directory = Path.GetDirectoryName(path);
                    if (_paths.Contains(directory))
                        return true;
                }
            }
            return false;
        }

        protected void OnChanged(string[] paths)
        {
            Changed?.Invoke(paths);
        }
    }
}
