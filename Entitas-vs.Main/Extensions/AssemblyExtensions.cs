﻿using Entitas_vs.Contract;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace EntitasVSGenerator.Extensions
{
    static class AssemblyExtensions
    {
        private const string DllsFolder = "DllToCopy";

        public static IGenerator GetGenerator(string generatorAssemblyPath, string projectPath, string solutionDirectory)
        {
            string dllPath = $@"{solutionDirectory}\{generatorAssemblyPath}\Entitas-vs.Invoker.dll";
            Assembly asm = Assembly.LoadFrom(dllPath);
            Type type = asm.GetType("Entitas_vs.Invoker.Generator");
            IGenerator generator = (IGenerator)Activator.CreateInstance(type, projectPath);
            return generator;
        }
        
        public static void CopyDllsToGeneratorDirectory(string codeGeneratorPath, string solutionDirectory)
        {
            string fullGeneratorPath = $@"{solutionDirectory}\{codeGeneratorPath}";
            string dllToCopyFolder = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{DllsFolder}";
            foreach(string dllFilePath in Directory.GetFiles(dllToCopyFolder))
            {
                var fullNameDestination = $@"{fullGeneratorPath}\{Path.GetFileName(dllFilePath)}";
                string fullNameSource = Path.GetFullPath(dllFilePath);
                if (File.Exists(fullNameDestination))
                {
                    FileVersionInfo versionDestination = FileVersionInfo.GetVersionInfo(fullNameDestination);
                    FileVersionInfo versionSource = FileVersionInfo.GetVersionInfo(fullNameSource);
                    if(versionDestination.FileVersion != versionSource.FileVersion)
                    {
                        File.Copy(dllFilePath, fullNameDestination);
                    }
                }
                else
                {
                    File.Copy(dllFilePath, fullNameDestination);
                }
            }
        }
    }
}