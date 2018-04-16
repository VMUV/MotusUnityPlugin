using Motus_Unity_Plugin.VMUV_Hardware.Motus_1;
using System;

namespace Motus_Unity_Plugin
{
    static class DataStorageTable
    {
        private static Motus_1_Platform _platform = new Motus_1_Platform();
        private static float[] _quat = new float[4];

        public static void SetMotus_1_Data(int[] data)
        {
            _platform.SetAllSensorElementValues(data);
        }

        public static Motus_1_MovementVector GetMotionInput()
        {
            return _platform.GetDirectionalVector();
        }

        public static Motus_1_Platform GetPlatformObject()
        {
            return _platform;
        }

        public static void SetQuat(float[] data)
        {
            if (data.Length == 4)
                Buffer.BlockCopy(data, 0, _quat, 0, 16);
        }

        public static float[] GetQuat()
        {
            return _quat;
        }
    }
}
