using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibADCA.Models;

namespace LibADCA
{
    public static class Command
    {
        public static ProcessResult ExecuteCmd(string arguments)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<string> _pRet = new List<string>();
            // Create the Process Info object with the overloaded constructor
            // This takes in two parameters, the program to start and the
            // command line arguments.
            // The arguments parm is prefixed with "@" to eliminate the need
            // to escape special characters (i.e. backslashes) in the
            // arguments string and has "/C" prior to the command to tell
            // the process to execute the command quickly without feedback.
            ProcessStartInfo _info =
                new ProcessStartInfo("cmd", @"/C " + arguments)
                {

                    // The following commands are needed to redirect the
                    // standard output.  This means that it will be redirected
                    // to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    // Set UseShellExecute to false.  This tells the process to run
                    // as a child of the invoking program, instead of on its own.
                    // This allows us to intercept and redirect the standard output.
                    UseShellExecute = false,

                    // Set CreateNoWindow to true, to supress the creation of
                    // a new window
                    CreateNoWindow = true
                };

            // Create a process, assign its ProcessStartInfo and start it
            Process _p = new Process
            {
                StartInfo = _info
            };

            _p.Start();

            // Capture the results in a string
            //string _processResults = _p.StandardOutput.ReadToEnd();
            bool error = false;
            while (!_p.StandardOutput.EndOfStream)
            {
                var line = _p.StandardOutput.ReadLine();
                _pRet.Add(line);
            }
            while (!_p.StandardError.EndOfStream)
            {
                error = true;
                var line = _p.StandardError.ReadLine();
                _pRet.Add(line);
            }

            // Close the process to release system resources
            _p.Close();
            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            _pRet.ForEach(e =>
            {
                stringBuilder.AppendLine(e);
            });
            string x = stringBuilder.ToString();

            ProcessResult processResult = new ProcessResult()
            {
                Duration = Convert.ToInt32(stopwatch.ElapsedMilliseconds / 1000),
                EncounteredError = error,
                Output = _pRet,
                OutputText = x
            };
            // Return the output stream to the caller
            return processResult;
        }
    }
}
