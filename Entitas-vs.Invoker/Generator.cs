using Entitas.CodeGeneration.CodeGenerator;
using Entitas_vs.Contract;
using EntitasVSGenerator.Extensions;
using System.Linq;

namespace Entitas_vs.Invoker
{
    class Generator : IGenerator
    {
        private readonly CodeGenerator _codeGenerator;

        public Generator(string projectPath)
        {
            var preferences = EntitasHelper.GetPreferences(projectPath);
            preferences.RemoveCsprojPlugin();
            _codeGenerator = EntitasHelper.GetCodeGenerator(preferences);
            TargetDirectory = preferences.GetTargetDirectory();
        }

        public string TargetDirectory { get; }

        public string[] Generate()
        {
            return _codeGenerator.Generate().GetPaths().ToAbsolutePaths(TargetDirectory).ToArray();
        }
    }
}
