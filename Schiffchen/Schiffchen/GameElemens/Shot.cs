﻿using System;
using System.Net;

using Schiffchen.Logic.Messages;

namespace Schiffchen.GameElemens
{
    public class Shot
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public String Result { get; set; }
        public ShipInfo ShipInfo { get; set; }

        public Shot(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
