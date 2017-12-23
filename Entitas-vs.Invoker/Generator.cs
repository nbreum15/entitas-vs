using DesperateDevs.CodeGeneration.CodeGenerator;
using Entitas_vs.Contract;
using Entitas_vs.Main.Extensions;
using System.Linq;
using Entitas_vs.Common;

namespace Entitas_vs.Invoker
{
    class Generator : IGenerator
    {
        private readonly CodeGenerator _codeGenerator;

        public Generator(string projectPath, string propertiesName, string userPropertiesName)
        {
            var preferences = EntitasHelper.GetPreferences(projectPath, propertiesName, userPropertiesName);
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
