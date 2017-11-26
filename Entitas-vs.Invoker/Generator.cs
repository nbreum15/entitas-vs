using Entitas.CodeGeneration.CodeGenerator;
using Entitas_vs.Contract;
using EntitasVSGenerator.Extensions;
using System.Linq;

namespace Entitas_vs.Invoker
{
    class Generator : IGenerator
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly string _targetPath;

        public Generator(string projectPath)
        {
            _codeGenerator = EntitasExtensions.GetCodeGenerator(projectPath, out _targetPath);
        }

        public string[] Generate()
        {
            return _codeGenerator.Generate().GetPaths().ToAbsolutePaths(_targetPath).ToArray();
        }
    }
}
