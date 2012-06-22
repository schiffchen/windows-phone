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
    public class QueuingMessage : BattleshipMessage
    {
        public QueueingAction Action;
        public JID JID;
        public String MatchID;
        public Int32 ID;

        public QueuingMessage(QueueingAction action, Dictionary<string, object> dict) : base(Type.Queueing)
        {
            this.Action = action;
            switch (action)
            {
                case QueueingAction.success:
                case QueueingAction.ping:
                    this.ID = (Int32)dict["id"];
                    break;
                case QueueingAction.assign:
                case QueueingAction.assigned:
                    this.JID = (JID)dict["jid"];
                    this.MatchID = (String)dict["mid"];
                    break;
            }
        }

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
