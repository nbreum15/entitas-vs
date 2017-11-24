using Entitas.CodeGeneration.CodeGenerator;
using EntitasVSGenerator.Extensions;

namespace EntitasVSGenerator.Logic
{
    class CodeGeneratorInvoker
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly string _targetDirectory;

        public CodeGeneratorInvoker(string projectDirectory)
        {
            _codeGenerator = EntitasExtensions.GetCodeGenerator($@"{projectDirectory}\", out _targetDirectory);
        }

        public string[] Generate()
        {
            return _codeGenerator.Generate().GetFullPaths(_targetDirectory);
        }
    }
}
