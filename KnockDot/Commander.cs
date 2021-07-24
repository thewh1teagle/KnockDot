using System;
using System.Diagnostics;


namespace KnockDot
{
    class Commander
    {
        public static string RunCommand(string arguments, bool readOutput)
        {
            var output = string.Empty;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    Verb = "runas",
                    FileName = "cmd.exe",
                    Arguments = "/C " + arguments,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = false
                };

                var proc = Process.Start(startInfo);

                if (readOutput)
                {
                    output = proc.StandardOutput.ReadToEnd();
                }

                proc.WaitForExit(60000);

                return output;
            }
            catch (Exception)
            {
                return output;
            }
        }
    }
}
