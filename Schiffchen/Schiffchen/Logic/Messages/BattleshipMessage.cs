using System.Collections.Generic;
using System;
using System.Net.XMPP;

namespace Schiffchen.Logic.Messages
{
    public class BattleshipMessage
    {
        public Type Type;
        public JID From { get; set; }
        public JID To { get; set; }

        public BattleshipMessage(Type messageType)
        {
            this.From = AppCache.XmppManager.OwnID;
            if (AppCache.CurrentMatch != null)
            {                
                this.To = AppCache.CurrentMatch.PartnerJID;
            }
        }

       
    }
}
