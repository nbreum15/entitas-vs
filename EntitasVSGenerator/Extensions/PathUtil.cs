using EnvDTE;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntitasVSGenerator.Extensions
{
    static class PathUtil
    {
        public const string SettingsName = "entitas-vs.cfg";

        public static string GetProjectDirectory(Project project)
        {
#if DEBUG
            return @"C:\Users\nickl\Desktop\entitas-test";
#else
            return Path.GetDirectoryName(project.FullName);
#endif
        }

        public static string GetSettingsPath(string projectDirectory)
        {
            return $@"{projectDirectory}\{SettingsName}";
        }

        public static IEnumerable<string> ToAbsolutePaths(this IEnumerable<string> paths, string appendDirectory)
        {
            return paths.Select(path => Path.IsPathRooted(path) ? path : $@"{appendDirectory}\{path}");
        }
    }
}
