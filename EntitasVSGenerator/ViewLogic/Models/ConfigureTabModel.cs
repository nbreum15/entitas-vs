using EntitasVSGenerator.Logic;
using System;

namespace EntitasVSGenerator
{
    public class ConfigureTabModel
    {
        public event Action<string> GeneratePathChanged;
        public event Action GeneratorLoadClick;

        public bool IsGeneratorLoaded { get; set; }
        public void LoadGenerator()
        {
            GeneratorLoadClick?.Invoke();
        }

        public ProjectItem[] ProjectItems { get; set; }
        private string _generatorPath;
        public string GeneratorPath
        {
            get => _generatorPath;
            set
            {
                GeneratePathChanged?.Invoke(value);
                _generatorPath = value;
            }
        }

        public string SolutionDirectory { get; }

        public ConfigureTabModel(ProjectItem[] projectItems, string generatorPath, string solutionDirectory)
        {
            ProjectItems = projectItems;
            GeneratorPath = generatorPath;
            SolutionDirectory = solutionDirectory;
        }
    }
}