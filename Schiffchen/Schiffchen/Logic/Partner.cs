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

        public static DateTime LastPing = DateTime.Now;

        public static PartnerState OnlineState { get; set; }

        public static void Dice(int dice)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("dice", dice);
                MatchMessage message = new MatchMessage(MatchAction.Diceroll, dict);
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.CurrentMatch.OwnJID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }        
        }

        public static void Ping()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("id", AppCache.XmppManager.QueueID);

                QueuingMessage message = new QueuingMessage(Enum.QueueingAction.ping, dict);
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        public static void Shoot(int x, int y)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("x", x);
                dict.Add("y", y);

                MatchMessage message = new MatchMessage(MatchAction.Shot, dict);
            try
            {                
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        public static void TransferShotResult(int x, int y, Boolean isHit)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            if (isHit)
            {
                dict.Add("result", "ship");
            }
            else
            {
                dict.Add("result", "water");
            }
            MatchMessage message = new MatchMessage(MatchAction.Shotresult, dict);
            try
            {
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
