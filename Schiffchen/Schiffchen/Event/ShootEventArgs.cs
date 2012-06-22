using System;



namespace Schiffchen.Event
{
    public class ShootEventArgs : EventArgs
    {
        public ShootEventArgs(int x, int y)
        {
            X = x;
            Y = y;
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
    }
}

