using DesperateDevs.CodeGeneration;
using DesperateDevs.CodeGeneration.CodeGenerator;
using DesperateDevs.Serialization;
using DesperateDevs.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entitas_vs.Main.Extensions
{
    static class EntitasHelper
    {
        private const string SEARCH_PATHS = "CodeGenerator.SearchPaths";
        private const string PROJECT_PATH = "DesperateDevs.CodeGeneration.Plugins.ProjectPath";
        private const string TARGET_DIRECTORY = "DesperateDevs.CodeGeneration.Plugins.TargetDirectory";
        
        private const string POSTPROCESSORS = "CodeGenerator.PostProcessors";
        private const string CSPROJ_POSTPROCESSOR = "DesperateDevs.CodeGeneration.Plugins.UpdateCSProjPostProcessor";

        public static Preferences GetPreferences(string projectPath, string propertiesName, string userPropertiesName)
        {
            string propertiesPath = $@"{projectPath}\{propertiesName}";
            string userPropertiesPath = $@"{projectPath}\{userPropertiesName}";
            if (!File.Exists(propertiesPath) || !File.Exists(userPropertiesPath))
                throw new FileNotFoundException($"Could not find \"{propertiesPath}\" or \"{userPropertiesPath}\".");
            var preferences = new Preferences(propertiesPath, userPropertiesPath);
            preferences
                .AppendProjectPath(SEARCH_PATHS, projectPath)
                .AppendProjectPath(PROJECT_PATH, projectPath)
                .AppendProjectPath(TARGET_DIRECTORY, projectPath);
            return preferences;
        }

        public static void RemoveCsprojPlugin(this Preferences preferences)
        {
            string[] values = preferences[POSTPROCESSORS].ArrayFromCSV();
            int index = Array.IndexOf(values, CSPROJ_POSTPROCESSOR);
            if(index != -1) // IndexOf returns -1 if it was not found
            {
                values[index] = null;
            }
            preferences[POSTPROCESSORS] = values.ToCSV();
        }

        public static string GetTargetDirectory(this Preferences preferences)
        {
            return $"{preferences[TARGET_DIRECTORY]}/Generated".Replace("/", "\\");
        }

        public static CodeGenerator GetCodeGenerator(Preferences preferences)
        {
            return CodeGeneratorUtil.CodeGeneratorFromPreferences(preferences);
        }

        public static string[] GetPaths(this CodeGenFile[] codeGenFiles)
        {
            return codeGenFiles.Select(file => file.fileName).ToArray();
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
    }
}
