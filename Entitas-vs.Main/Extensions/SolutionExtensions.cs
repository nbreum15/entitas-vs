using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace EntitasVSGenerator.Extensions
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
    }
}
