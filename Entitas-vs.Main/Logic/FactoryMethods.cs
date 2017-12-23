namespace Entitas_vs.Main.Logic
{
    internal static class FactoryMethods
    {
        public static IDirectoryChangeListener GetRelativeDirectoryChangeListener(string projectDirectory)
        {
            return new RelativeDirectoryChangeListener(projectDirectory);
        }

        public static IPackageLoader GetPackageLoader()
        {
            return new PackageLoader(EntitasVsPackage.VsSolution);
        }
    }
}
