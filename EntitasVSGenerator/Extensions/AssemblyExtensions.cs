﻿using Entitas_vs.Contract;
using System;
using System.Reflection;

namespace EntitasVSGenerator.Extensions
{
    static class AssemblyExtensions
    {
        public static IGenerator GetGenerator(string generatorAssemblyPath, string projectPath)
        {
            string dllPath = $@"{generatorAssemblyPath}\Entitas-vs.Invoker.dll";
            Assembly asm = Assembly.LoadFrom(dllPath);
            Type type = asm.GetType("Entitas_vs.Invoker.Generator");
            IGenerator generator = (IGenerator)Activator.CreateInstance(type);
            return generator;
        }
    }
}