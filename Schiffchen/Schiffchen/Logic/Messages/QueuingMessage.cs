using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Schiffchen.Logic.Enum;
using System.Net.XMPP;


namespace Schiffchen.Logic.Messages
{
    /// <summary>
    /// Defines a queue message, which is sent or received over XMPP (mostly with the matchmaker)
    /// </summary>
    public class QueuingMessage : BattleshipMessage
    {
        public QueueingAction Action;
        public JID JID;
        public String MatchID;
        public Int32 ID;

        /// <summary>
        /// Creates a new entity of a QueuingMessage
        /// </summary>
        /// <param name="action">The current action</param>
        /// <param name="dict">The dictionary with parameters</param>
        public QueuingMessage(QueueingAction action, Dictionary<string, object> dict)
            : base(Schiffchen.Logic.Enum.Type.Queueing)
        {
            this.Action = action;
            switch (action)
            {
                case QueueingAction.success:
                case QueueingAction.ping:
                    if (dict.ContainsKey("id"))
                    {
                        this.ID = (Int32)dict["id"];
                    }
                    break;
                case QueueingAction.assign:
                case QueueingAction.assigned:
                    this.JID = (JID)dict["jid"];
                    this.MatchID = (String)dict["mid"];
                    break;
            }
        }

        /// <summary>
        /// Generates the XML for sending over XMPP
        /// </summary>
        /// <returns>An XML string</returns>
        public String ToSendXML()
        {
            return this.ToSendXML(this.From, this.To);
        }

        /// <summary>
        /// Generates the XML for sending over XMPP
        /// </summary>
        /// <param name="from">The Jabber-ID of the sender</param>
        /// <param name="to">The Jabber-ID of the receiver</param>
        /// <returns>An XML string</returns>
        public String ToSendXML(JID from, JID to)
        {
            String s = "<message from=\"" + from.FullJID + "\" id=\"" + Guid.NewGuid() + "\" to=\"" + to.FullJID + "\" type=\"normal\">\n<battleship xmlns=\"http://battleship.me/xmlns/\">";
            switch (Action) {
                case QueueingAction.request:
                    s += "<queueing action=\"request\" />";
                        break;
                case QueueingAction.ping:
                        s += "<queueing action=\"ping\" id=\"" + this.ID + "\" />";
                        break;
                case QueueingAction.assigned:
                        s += "<queueing action=\"assigned\" jid=\"" + this.JID + "\" mid=\"" + this.MatchID + "\" />";
                        break;
                default:
                    throw new Exception("This Queuing Message Type is not for sending!");
            }
            s += "</battleship>\n</message>";
            return s;
        }
    }
}
