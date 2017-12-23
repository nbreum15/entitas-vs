using System.Runtime.Serialization;
using System.Windows.Input;
using Entitas_vs.View.Commands;

namespace Entitas_vs.View.ViewModels
{
    class GeneratorData : BaseViewModel
    {
        private string _name;
        private string _generatorPath;

        public GeneratorData(string solutionDirectory)
        {
            AddFolderPathCommand = new AddFolderPathCommand(path => GeneratorPath = path, solutionDirectory);
        }

        [DataMember] public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        [DataMember] public string GeneratorPath
        {
            get => _generatorPath;
            set => SetValue(ref _generatorPath, value);
        }

        public ICommand AddFolderPathCommand { get; }
    }
}
