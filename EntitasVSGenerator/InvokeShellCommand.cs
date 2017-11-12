namespace EntitasVSGenerator
{
    using System.Management.Automation;

    class InvokeShellCommand
    {
        private readonly string _solutionDirectory;
        private readonly PowerShell _shellInstance;
        private bool _alreadyRunning;

        public InvokeShellCommand(string solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
            _shellInstance = OpenShell();
            _shellInstance.AddScript("entitas client 3333 gen -s");
        }

        private PowerShell OpenShell()
        {
            PowerShell powerShell = PowerShell.Create();
            return powerShell;
        }

        public void Generate()
        {
            if (_alreadyRunning)
                return;
            _alreadyRunning = true;
            _shellInstance.BeginInvoke<object>(null, null, ar => _alreadyRunning = false, null);
        }
    }
}
