using System;



namespace Schiffchen.Event
{
    /// <summary>
    /// Defines the result event of a rolled dice, which holds an integer value
    /// </summary>
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

