using System;
using System.Windows.Controls;

namespace Schiffchen.Logic.Messages
{
    public class ShipInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public Orientation Orientation { get; set; }
        public Boolean Destroyed { get; set; }

    }
}
