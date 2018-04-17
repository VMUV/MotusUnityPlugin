using Comms_Protocol_CSharp;
using VMUV_TCP_CSharp;

namespace Motus_Unity_Plugin.TCP
{
    static class Client
    {
        private static SocketWrapper _client = new SocketWrapper(Configuration.client);
        private static DataQueue _queue = new DataQueue();
        private static Motus_1_RawDataPacket _motus = new Motus_1_RawDataPacket();
        private static RotationVectorRawDataPacket _rot = new RotationVectorRawDataPacket();

        public static void Service()
        {
            _client.ClientStartRead();
            if (_client.ClientHasData())
            {
                _client.ClientGetRxData(_queue);
                UpdateCurrentValues();
            }
        }

        private static void UpdateCurrentValues()
        {
            while (!_queue.IsEmpty())
            {
                DataPacket p = _queue.Get();
                switch (p.Type)
                {
                    case ValidPacketTypes.motus_1_raw_data_packet:
                        _motus.Payload = p.Payload;
                        short[] rawData = _motus.DeSerialize();
                        int[] data = new int[rawData.Length];
                        for (int i = 0; i < rawData.Length; i++)
                            data[i] = (int)rawData[i];
                        DataStorageTable.SetMotus_1_Data(data);
                        break;
                    case ValidPacketTypes.rotation_vector_raw_data_packet:
                        _rot.Payload = p.Payload;
                        DataStorageTable.SetQuat(_rot.GetQuat());
                        break;
                }
            }
        }
    }
}
