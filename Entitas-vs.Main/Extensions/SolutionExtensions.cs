using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;

namespace Entitas_vs.Main.Extensions
{
    static class SolutionExtensions
    {
        public static void RemoveItems(this Solution solution, IEnumerable<string> filesNames)
        {
            foreach (string fileName in filesNames)
            {
                ProjectItem item = solution.FindProjectItem(fileName);
                item.Remove();
            }
        }

        public static IEnumerable<string> UniqueNames(this IEnumerable<Project> projects)
        {
            return projects.Select(project => project.UniqueName);
        }

        public static Project FindProject(this Solution solution, string uniqueName)
        {
            return solution.GetAllProjects().First(project => project.UniqueName == uniqueName);
        }

        public static string GetDirectory(this Project project)
        {
            return Path.GetDirectoryName(project.FullName);
        }

        public static string GetDirectory(this Solution solution)
        {
            return Path.GetDirectoryName(solution.FullName);
        }

        public static string GetFileNameOnly(this Project project)
        {
            return Path.GetFileName(project.FullName);
        }
    }
}
