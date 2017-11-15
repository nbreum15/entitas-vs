using EnvDTE;

namespace EntitasVSGenerator.Extensions
{
    static class PathUtil
    {
        public const string SettingsName = "entitas-vs.cfg";

        public static string GetSolutionDirectory(DTE dte)
        {
#if DEBUG
            return @"C:\Users\nickl\Desktop\entitas-test";
#else
            return dte.Solution.FullName;
#endif
        }

        public static string GetSettingsPath(DTE dte)
        {
            return $@"{GetSolutionDirectory(dte)}\{SettingsName}";
        }
    }
}
