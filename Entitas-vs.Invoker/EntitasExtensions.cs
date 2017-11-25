using Entitas.CodeGeneration;
using Entitas.CodeGeneration.CodeGenerator;
using Entitas.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntitasVSGenerator.Extensions
{
    static class EntitasExtensions
    {
        public static CodeGenerator GetCodeGenerator(string projectPath, out string targetDir)
        {
            string propertiesPath = $@"{projectPath}\{Preferences.DEFAULT_PROPERTIES_PATH}";
            string userPropertiesPath = $@"{projectPath}\{Preferences.DEFAULT_USER_PROPERTIES_PATH}";
            if (!File.Exists(propertiesPath) || !File.Exists(userPropertiesPath))
                throw new FileNotFoundException($"\"{Preferences.DEFAULT_PROPERTIES_PATH}\" or \"{Preferences.DEFAULT_USER_PROPERTIES_PATH}\" not found at directory \"{projectPath}\".");
            var preferences = new Preferences(propertiesPath, userPropertiesPath);
            preferences
                .AppendProjectPath("CodeGenerator.SearchPaths", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.ProjectPath", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.Assemblies", projectPath)
                .AppendProjectPath("Entitas.CodeGeneration.Plugins.TargetDirectory", projectPath);
            targetDir = $"{preferences["Entitas.CodeGeneration.Plugins.TargetDirectory"]}/Generated".Replace("/","\\");
            return CodeGeneratorUtil.CodeGeneratorFromPreferences(preferences);
        }

        public static string[] GetFullPaths(this CodeGenFile[] codeGenFiles, string targetDirectory)
        {
            return codeGenFiles.Select(file => file.fileName).ToAbsolutePaths(targetDirectory).ToArray();
        }

        private static Preferences AppendProjectPath(this Preferences preferences, string preferenceKey, string projectPath)
        {
            string[] value = preferences[preferenceKey].ArrayFromCSV();
            for (int index = 0; index < value.Length; index++)
            {
                value[index] = $@"{projectPath}\{value[index]}".Replace("\\", "/");
            }
            preferences[preferenceKey] = value.ToCSV();
            return preferences;
        }

        public static IEnumerable<string> ToAbsolutePaths(this IEnumerable<string> paths, string appendDirectory)
        {
            if (appendDirectory == null)
                throw new ArgumentNullException(nameof(appendDirectory));
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));
            return paths.Select(path => Path.IsPathRooted(path) ? path : $@"{appendDirectory}\{path}");
        }
    }
}
