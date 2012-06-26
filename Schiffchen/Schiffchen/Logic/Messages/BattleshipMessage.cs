using System.Collections.Generic;
using System;
using System.Net.XMPP;

namespace Schiffchen.Logic.Messages
{
    /// <summary>
    /// Defines a message, which is sent or received over XMPP
    /// </summary>
    public class BattleshipMessage
    {
        public Type Type;
        public JID From { get; set; }
        public JID To { get; set; }

        /// <summary>
        /// The constructor of the elementary battleship message
        /// </summary>
        /// <param name="messageType">The message type of this message</param>
        public BattleshipMessage(Schiffchen.Logic.Enum.Type messageType)
        {
            this.From = AppCache.XmppManager.OwnID;
            if (AppCache.CurrentMatch != null)
            {                
                this.To = AppCache.CurrentMatch.PartnerJID;
            }
        }

       
    }
}
