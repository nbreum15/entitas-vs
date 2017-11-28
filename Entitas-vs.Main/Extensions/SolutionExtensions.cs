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
    }
}
