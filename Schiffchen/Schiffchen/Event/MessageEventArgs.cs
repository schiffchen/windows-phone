using System;
using Schiffchen.Logic.Messages;


namespace Schiffchen.Event
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(BattleshipMessage message)
        {
            this.ReceivedMessage = message;
        }       

        public BattleshipMessage ReceivedMessage
        {
            get;
            private set;
        }

        
    }
}

