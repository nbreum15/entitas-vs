namespace EntitasVSGenerator
{
    using System.Diagnostics;
    using System.Management.Automation;

    class InvokeShellCommand
    {
        private readonly string _solutionDirectory;
        private readonly PowerShell _shellInstance;
        private bool _alreadyRunning;
        private Process _process;

        public InvokeShellCommand(string solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
            _shellInstance = InitializeClient();
        }

        public void StartServer()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = _solutionDirectory;
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K entitas server";
            process.StartInfo = startInfo;
            process.Start();
            _process = process;
        }

        public void StopServer()
        {
            if (!_process.HasExited)
            {
                _process.CloseMainWindow();
                _process.Close();
            }
        }

        private PowerShell InitializeClient()
        {
            return PowerShell.Create().AddScript("entitas client 3333 gen -s");
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
