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
    /// <summary>
    /// Handles all methods to communicate with the Matchmaker
    /// </summary>
    public class Matchmaker
    {
        private static JID MatchmakerJID;

        /// <summary>
        /// Sets the Jabber-ID of the Matchmaker
        /// </summary>
        static Matchmaker() 
        {
            MatchmakerJID = new JID("matchmaker@battleship.me");
        }

        /// <summary>
        /// Request a queue id from the Matchmaker
        /// </summary>
        /// <param name="Manager">The xmpp manager</param>
        public static void Queue(XMPPManager Manager)
        {
            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.request, null);

            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));    
        }

        /// <summary>
        /// Sends a ping to the Matchmaker
        /// </summary>
        /// <param name="Manager">The xmpp manager</param>
        public static void Ping(XMPPManager Manager)
        {
            Dictionary<string, object> dict = new Dictionary<string,object>();
            dict.Add("id", Manager.QueueID);

            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.ping, dict);
            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));
        }

        /// <summary>
        /// Commits the assignement of two players to the Matchmaker
        /// </summary>
        /// <param name="Manager">The xmpp manager</param>
        /// <param name="partner">The Jabber-ID of the partner</param>
        /// <param name="mid">The Match-ID of the upcoming match</param>
        public static void Assigned(XMPPManager Manager, JID partner, String mid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("mid", mid);
            dict.Add("jid", partner);

            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.assigned, dict);
            Manager.Client.SendRawXML(message.ToSendXML(Manager.OwnID, MatchmakerJID));
        }


    }
}
