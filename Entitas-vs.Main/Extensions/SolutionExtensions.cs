using System.Collections.Generic;
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
    }
}
