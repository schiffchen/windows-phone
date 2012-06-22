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
using Schiffchen.Logic.Enum;

namespace Schiffchen.Logic
{
    public class Partner
    {
    

        public static void Dice(int dice)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("dice", dice);
            MatchMessage message = new MatchMessage(MatchAction.diceroll, dict);
            AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.CurrentMatch.OwnJID, AppCache.CurrentMatch.PartnerJID));
        }

        public static void Ping()
        {
            Dictionary<string, object> dict = new Dictionary<string,object>();
            dict.Add("id", AppCache.XmppManager.QueueID);

            QueuingMessage message = new QueuingMessage(Enum.QueueingAction.ping, dict);
            AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
        }

        public static void Shoot(int x, int y)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);

            MatchMessage message = new MatchMessage(MatchAction.shoot, dict);
            AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
        }
    }
}
