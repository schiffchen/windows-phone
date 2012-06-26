using System;
using Schiffchen.Logic.Messages;


namespace Schiffchen.Event
{
    /// <summary>
    /// Defines the Events of a received message. Holds the message object
    /// </summary>
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

