namespace EntitasVSGenerator.Logic
{
    internal static class FactoryMethods
    {
        public static IDirectoryChangeListener GetRelativeDirectoryChangeListener(string projectDirectory)
        {
            using (var directoryChangeListener = new RelativeDirectoryChangeListener(EntitasVsPackage.VsFileChangeEx, projectDirectory))
            {
                return directoryChangeListener;
            }
        }

        public static IFileIgnorer GetFileIgnorer()
        {
            return new FileIgnorer(EntitasVsPackage.VsFileChangeEx);
        }

        public static IPackageLoader GetPackageLoader()
        {
            return new PackageLoader(EntitasVsPackage.VsSolution);
            //using (var packageLoader = new PackageLoader(EntitasVsPackage.VsSolution))
            //{
            //    return packageLoader;
            //}
        }
    }
}
