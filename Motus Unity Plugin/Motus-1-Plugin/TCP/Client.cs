using Comms_Protocol_CSharp;
using VMUV_TCP_CSharp;

namespace Motus_Unity_Plugin.TCP
{
    static class Client
    {
        public static DataQueue _queue = new DataQueue(1024);

        private static SocketWrapper _client = new SocketWrapper(Configuration.client);

        public static void Service()
        {
            _client.ClientStartRead();
            if (_client.ClientHasData())
                _client.ClientGetRxData(_queue);
        }
    }
}
