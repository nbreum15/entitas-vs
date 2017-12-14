/*
==== START OF LICENSE ====

Copyright 2016 Mads Kristensen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

==== END OF LICENSE ====

==== START OF ADDITIONAL INFORMATION ====
Code from: https://github.com/madskristensen/Packman/blob/master/
src/PackmanVsix/Helpers/ProjectHelpers.cs
Modifications made to the code: 
- Replaced namespace with my own
- deleted methods: GetActiveProject, GetConfigFile, CheckFileOutOfSourceControl, 
GetFullPath, GetSelectedItems, GetRootFolder, SetItemType, IsConfigFile
- Added vsProjectKindSolutionFolder field instead of using ProjectKinds
.vsProjectKindSolutionFolder
- GetAllProjects takes (this Solution solution) as parameter instead of using 
_dte.Solution
- Removed _dte field
- Added dte parameter to AddFilesToProject

==== END OF ADDITIONAL INFORMATION ====
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EntitasVSGenerator.Extensions
{
    public static class ProjectHelpers
    {
        public const string vsProjectKindSolutionFolder = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        public static void AddFileToProject(this Project project, string file)
        {
            if (project.IsKind(ProjectTypes.ASPNET_5, ProjectTypes.DOTNET_Core))
                return;

            //if (_dte.Solution.FindProjectItem(file) == null)
            //{
            ProjectItem item = project.ProjectItems.AddFromFile(file);
            //}
        }

        public static void AddFilesToProject(this Project project, DTE2 dte, IEnumerable<string> files)
        {
            if (project == null || project.IsKind(ProjectTypes.ASPNET_5, ProjectTypes.DOTNET_Core))
                return;

            if (project.IsKind(ProjectTypes.WEBSITE_PROJECT))
            {
                var command = dte.Commands.Item("SolutionExplorer.Refresh");

                if (command.IsAvailable)
                    dte.ExecuteCommand(command.Name);

                return;
            }

            var solutionService = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;

            IVsHierarchy hierarchy = null;
            solutionService?.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

            if (hierarchy == null)
                return;

            var ip = (IVsProject)hierarchy;
            var result = new VSADDRESULT[files.Count()];

            ip.AddItem(VSConstants.VSITEMID_ROOT,
                       VSADDITEMOPERATION.VSADDITEMOP_LINKTOFILE,
                       string.Empty,
                       (uint)files.Count(),
                       files.ToArray(),
                       IntPtr.Zero,
                       result);
        }

        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            foreach (var guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static IEnumerable<Project> GetAllProjects(this Solution solution)
        {
            return solution.Projects
                  .Cast<Project>()
                  .SelectMany(GetChildProjects)
                  .Union(solution.Projects.Cast<Project>())
                  .Where(p => { try { return !string.IsNullOrEmpty(p.FullName); } catch { return false; } });
        }

        private static IEnumerable<Project> GetChildProjects(Project parent)
        {
            try
            {
                if (!parent.IsKind(vsProjectKindSolutionFolder) && parent.Collection == null)  // Unloaded
                    return Enumerable.Empty<Project>();

                if (!string.IsNullOrEmpty(parent.FullName))
                    return new[] { parent };
            }
            catch (COMException)
            {
                return Enumerable.Empty<Project>();
            }

            return parent.ProjectItems
                    .Cast<ProjectItem>()
                    .Where(p => p.SubProject != null)
                    .SelectMany(p => GetChildProjects(p.SubProject));
        }

        public static bool IsKind(this ProjectItem item, string kindGuid)
        {
            if (item == null)
                return false;

            return item.Kind.Equals(kindGuid, StringComparison.OrdinalIgnoreCase);
        }
    }

    public static class ProjectTypes
    {
        public const string ASPNET_5 = "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}";
        public const string DOTNET_Core = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
        public const string WEBSITE_PROJECT = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
        public const string UNIVERSAL_APP = "{262852C6-CD72-467D-83FE-5EEB1973A190}";
        public const string NODE_JS = "{9092AA53-FB77-4645-B42D-1CCCA6BD08BD}";
    }
}