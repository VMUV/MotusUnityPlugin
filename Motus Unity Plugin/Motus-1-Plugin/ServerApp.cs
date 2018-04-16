using System.Diagnostics;
using System;
using Trace_Logger_CSharp;

namespace Motus_Unity_Plugin
{
    class ServerApp
    {
        public const string fname = "C:/Program Files (x86)/VMUV LLC/Motus/Motus Hardware Interface.exe";
        public const string appName = "Motus Hardware Interface";

        public void LaunchProcess(string name)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = name;
                process.Start();
            }
            catch (ArgumentNullException e0)
            {
                Logger.LogMessage(e0.Message + e0.StackTrace);
            }
            catch (InvalidOperationException e1)
            {
                Logger.LogMessage(e1.Message + e1.StackTrace);
            }
            catch (System.ComponentModel.Win32Exception e2)
            {
                Logger.LogMessage(e2.Message + e2.StackTrace);
            }
        }
    }
}
