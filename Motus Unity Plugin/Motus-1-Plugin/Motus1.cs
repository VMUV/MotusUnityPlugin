using Motus_Unity_Plugin.TCP;
using Motus_Unity_Plugin.VMUV_Hardware.Motus_1;
using VMUV_TCP_CSharp;
using Trace_Logger_CSharp;
using Comms_Protocol_CSharp;

namespace Motus_Unity_Plugin
{
    public static class Motus1
    {
        private static bool _isInitalized = false;
        private static string _versionInfo = "2.0.1.0";

        public static void Initialize(bool rawDataLog = false)
        {
            if (!_isInitalized)
            {
                Logger.CreateLogFile();
                Logger.LogMessage("Unity Motus Plugin v" + _versionInfo);
                Logger.LogMessage("Client side TCP v" + SocketWrapper.version);

                _isInitalized = true;
            }

            ServerApp appLauncher = new ServerApp();
            appLauncher.LaunchProcess(ServerApp.fname);
        }

        public static void Service()
        {
            Client.Service();
        }

        public static DataQueue GetData()
        {
            DataQueue rtn = new DataQueue(1024);
            rtn.TransferAll(Client._queue);
            return rtn;
        }
    }
}
