using System;
using Schiffchen.Logic.Messages;


namespace Schiffchen.Event
{
    /// <summary>
    /// Defines the ShootEvent which holds coordinates and additional information
    /// </summary>
    public class ShootEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of ShootEventArgs
        /// </summary>
        /// <param name="x">The X-Coordinate of the shot</param>
        /// <param name="y">The Y-Coordinate of the shot</param>
        public ShootEventArgs(int x, int y)
        {
            X = x;
            Y = y;
            Result = null;
        }

        /// <summary>
        /// Creates a new instance of ShootEventArgs
        /// </summary>
        /// <param name="x">The X-Coordinate of the shot</param>
        /// <param name="y">The Y-Coordinate of the shot</param>
        /// <param name="result">The result ("ship" or "water") of the shot</param>
        public ShootEventArgs(int x, int y, string result)
        {
            X = x;
            Y = y;
            Result = result;
        }

        /// <summary>
        /// Creates a new instance of ShootEventArgs
        /// </summary>
        /// <param name="x">The X-Coordinate of the shot</param>
        /// <param name="y">The Y-Coordinate of the shot</param>
        /// <param name="result">The result ("ship" or "water") of the shot</param>
        /// <param name="shipInfo">The optional information of a destroyed ship</param>
        public ShootEventArgs(int x, int y, string result, ShipInfo shipInfo)
        {
            X = x;
            Y = y;
            Result = result;
            ShipInfo = shipInfo;
        }


        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public string Result
        {
            get;
            private set;
        }

        public ShipInfo ShipInfo
        {
            get;
            private set;
        }
    }
}

