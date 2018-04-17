using Motus_Unity_Plugin.VMUV_Hardware.Motus_1;
using Comms_Protocol_CSharp;
using System;

namespace Motus_Unity_Plugin
{
    static class DataStorageTable
    {
        private static Motus_1_Platform _platform = new Motus_1_Platform();
        private static RotationVector_Quat _quat = new RotationVector_Quat();

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

        public static void SetQuat(RotationVector_Quat q)
        {
            _quat = q;
        }

        public static RotationVector_Quat GetQuat()
        {
            return _quat;
        }
    }
}
