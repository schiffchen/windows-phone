using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Net.XMPP;
using Schiffchen.Logic.Messages;

namespace Schiffchen.Logic
{
    public class Matchmaker
    {
        private static JID MatchmakerJID;

        static Matchmaker() 
        {
            MatchmakerJID = new JID("matchmaker@battleship.me");
        }

        public static void Queue(XMPPManager Manager)
        {
            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.request, null);

            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));    
        }

        public static void Ping(XMPPManager Manager)
        {
            Dictionary<string, object> dict = new Dictionary<string,object>();
            dict.Add("id", Manager.QueueID);

            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.ping, dict);
            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));
        }

        public static void Assigned(XMPPManager Manager, JID partner, Int32 mid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("mid", mid);
            dict.Add("jid", partner.FullJID);

            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.assigned, dict);
            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));
        }


    }
}
