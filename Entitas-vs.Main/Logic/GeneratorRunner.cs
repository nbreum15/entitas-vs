using Entitas_vs.Contract;
using EntitasVSGenerator.Extensions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE80;

namespace EntitasVSGenerator.Logic
{
    class GeneratorRunner
    {
        private readonly string _uniqueProjectName;
        private readonly IGenerator _codeGenerator;
        private readonly DTE2 _dte;
        private string[] _oldGeneratedFiles;
        private bool _firstGenerate = true;

        public GeneratorRunner(string generatorPath, string uniqueProjectName)
        {
            _uniqueProjectName = uniqueProjectName;
            _dte = EntitasVsPackage.DTE;
            var project = _dte.Solution.FindProject(_uniqueProjectName);
            _codeGenerator = AssemblyExtensions.GetGenerator(generatorPath, project.GetDirectory(), _dte.Solution.GetDirectory());
        }

        public void Run()
        {
            Task.Run(() =>
            {
                string[] generatedFiles = Generate();
                string[] deletedFiles = GetDeletedGeneratedFiles(generatedFiles);
                RemoveItems(deletedFiles);
                AddItems(generatedFiles);
            });
        }

        private string[] Generate()
        {
            if (_firstGenerate)
            {
                _oldGeneratedFiles = GetCurrentGeneratedFileNames();
                _firstGenerate = false;
            }
            return _codeGenerator.Generate();
        }

        private string[] GetDeletedGeneratedFiles(string[] newGeneratedFiles)
        {
            if (_oldGeneratedFiles == null || newGeneratedFiles == null)
                return new string[0];
            return _oldGeneratedFiles.GetDeletedFileNames(newGeneratedFiles).ToArray();
        }

        private void RemoveItems(string[] deletedFileNames)
        {
            if (deletedFileNames == null)
                return;
            _dte.Solution.RemoveItems(deletedFileNames);
        }

        private void AddItems(string[] generatedFiles)
        {
            if (generatedFiles == null)
                return;
            _oldGeneratedFiles = generatedFiles;
            var project = _dte.Solution.FindProject(_uniqueProjectName);
            project.AddFilesToProject(_dte, generatedFiles);
        }

        private string[] GetCurrentGeneratedFileNames()
        {
            if (Directory.Exists(_codeGenerator.TargetDirectory))
                return Directory.GetFiles(_codeGenerator.TargetDirectory, "*.cs", SearchOption.AllDirectories);
            else return null;
        }
    }
}
