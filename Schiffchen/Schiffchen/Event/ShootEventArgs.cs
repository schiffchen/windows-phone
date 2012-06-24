using System;



namespace Schiffchen.Event
{
    public class ShootEventArgs : EventArgs
    {
        public ShootEventArgs(int x, int y)
        {
            X = x;
            Y = y;
            Result = null;
        }

        public ShootEventArgs(int x, int y, string result)
        {
            X = x;
            Y = y;
            Result = result;
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
    }
}

