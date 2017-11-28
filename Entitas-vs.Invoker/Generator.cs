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
            _codeGenerator = EntitasExtensions.GetCodeGenerator(projectPath, out string _targetPath);
            TargetDirectory = _targetPath;
        }

        public string TargetDirectory { get; }

        public string[] Generate()
        {
            return _codeGenerator.Generate().GetPaths().ToAbsolutePaths(TargetDirectory).ToArray();
        }
    }
}
