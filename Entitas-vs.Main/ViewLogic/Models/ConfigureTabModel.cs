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

        public ProjectViewModel[] ProjectViewModels { get; set; }
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

        public ConfigureTabModel(ProjectViewModel[] projectViewModels, string generatorPath, string solutionDirectory)
        {
            ProjectViewModels = projectViewModels;
            GeneratorPath = generatorPath;
            SolutionDirectory = solutionDirectory;
        }
    }
}