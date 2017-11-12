namespace EntitasVSGenerator
{
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    class InvokeShellCommand
    {
        private readonly string _solutionDirectory;
        private readonly PowerShell _shellInstance;
        private readonly Runspace _runspace;

        public InvokeShellCommand(string solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
            _shellInstance = OpenShell();
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }

        private PowerShell OpenShell()
        {
            PowerShell powerShell = PowerShell.Create();
            powerShell.AddScript($"set-location {_solutionDirectory}");
            powerShell.Invoke();
            return powerShell;
        }

        public void Generate()
        {
            _shellInstance.Runspace = _runspace;
            //_shellInstance.AddScript("entitas client 3333 gen");
            _shellInstance.AddScript("entitas gen");
            _shellInstance.BeginInvoke();
        }
    }
}
