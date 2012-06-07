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
        public Int32 MatchID;
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
                    this.MatchID = (Int32)dict["mid"];
                    break;
            }
        }
    }
}
