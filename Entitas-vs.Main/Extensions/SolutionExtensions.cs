using EnvDTE;
using System.Collections.Generic;

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

        /// <summary>
        /// Adds items to a project.
        /// </summary>
        /// <param name="project">The project to add the items to.</param>
        /// <param name="fileNames">Absolute paths to the files.</param>
        public static void AddItems(this Project project, IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (fileName == null)
                    continue;
                project.ProjectItems.AddFromFile(fileName);
            }
        }
    }
}
