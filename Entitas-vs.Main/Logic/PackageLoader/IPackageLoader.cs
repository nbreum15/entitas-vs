using System;

namespace EntitasVSGenerator.Logic
{
    interface IPackageLoader
    {
        event Action AfterOpenSolution;
    }
}
