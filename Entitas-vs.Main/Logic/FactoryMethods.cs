namespace EntitasVSGenerator.Logic
{
    internal static class FactoryMethods
    {
        public static IDirectoryChangeListener GetRelativeDirectoryChangeListener(string projectDirectory)
        {
            return new RelativeDirectoryChangeListener(projectDirectory);
        }

        public static IFileIgnorer GetFileIgnorer()
        {
            return new FileIgnorer(EntitasVsPackage.VsFileChangeEx);
        }

        public static IPackageLoader GetPackageLoader()
        {
            return new PackageLoader(EntitasVsPackage.VsSolution);
        }
    }
}
