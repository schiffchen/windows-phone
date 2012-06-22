using System;



namespace Schiffchen.Event
{
    public class RollingDiceEventArgs : EventArgs
    {
        public RollingDiceEventArgs(int value)
        {
            Value = value;
        }

        public int Value
        {
            get;
            private set;
        }
    }
}

