using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Devices;

namespace Schiffchen.Logic
{
    public class VibrationManager
    {

        public static VibrateController Vibration;

        static VibrationManager()
        {
            Vibration = VibrateController.Default;
        }
    }
}
