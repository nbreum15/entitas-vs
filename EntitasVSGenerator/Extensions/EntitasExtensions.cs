using Entitas.CodeGeneration;
using Entitas.CodeGeneration.CodeGenerator;
using Entitas.Utils;
using System.Linq;

namespace EntitasVSGenerator.Extensions
{
    static class EntitasExtensions
    {
        public static CodeGenerator GetCodeGenerator(string projectPath, out string targetDir)
        {
            string propertiesPath = $@"{projectPath}\{Preferences.DEFAULT_PROPERTIES_PATH}";
            string userPropertiesPath = $@"{projectPath}\{Preferences.DEFAULT_USER_PROPERTIES_PATH}";
            var preferences = new Preferences(propertiesPath, userPropertiesPath);
            preferences
                .AppendProjectPath("CodeGenerator.SearchPaths", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.ProjectPath", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.Assemblies", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.TargetDirectory", projectPath);
            targetDir = $"{preferences["Entitas.CodeGeneration.Plugins.TargetDirectory"]}/Generated";
            return CodeGeneratorUtil.CodeGeneratorFromPreferences(preferences);
        }

        public static string[] GetFullPaths(this CodeGenFile[] codeGenFiles, string targetDirectory)
        {
            return codeGenFiles.Select(file => $"{targetDirectory}/{file.fileName}".Replace("\\", "/")).ToArray();
        }

        private static Preferences AppendProjectPath(this Preferences preferences, string preferenceKey, string projectPath)
        {
            string[] value = preferences[preferenceKey].ArrayFromCSV();
            for (int index = 0; index < value.Length; index++)
            {
                value[index] = $"{projectPath}{value[index]}".Replace("\\", "/");
            }
            preferences[preferenceKey] = value.ToCSV();
            return preferences;
        }
    }
}
