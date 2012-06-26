using System;
using System.Windows.Controls;

namespace Schiffchen.Logic.Messages
{
    /// <summary>
    /// Defines simple information of a ship
    /// Is used to send or receive over a MatchMessage for the shot result.
    /// </summary>
    public class ShipInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public Orientation Orientation { get; set; }
        public Boolean Destroyed { get; set; }

    }
}
