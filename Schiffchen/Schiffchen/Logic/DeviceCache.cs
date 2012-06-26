using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Schiffchen
{
    /// <summary>
    /// Defines the cache of the game, where global variables for the current rendering environment and device are stored
    /// </summary>
    public class DeviceCache
    {
        public static Int32 ScreenWidth;
        public static Int32 ScreenHeight;
 
        public static Int32 BelowGrid;
        public static Int32 BelowSmallGrid;
        public static Vector2 RightOfMinimap;
       
    }
}
