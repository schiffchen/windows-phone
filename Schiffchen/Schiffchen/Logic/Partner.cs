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
    /// <summary>
    /// Handles all methods to communicate with the game partner
    /// </summary>
    public class Partner
    {

        public static DateTime LastPing = DateTime.Now;
        public static PartnerState OnlineState { get; set; }

        /// <summary>
        /// Sends the dice information to the partner
        /// </summary>
        /// <param name="dice">The spots of the dice</param>
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

        /// <summary>
        /// Sends a ping to the game partner
        /// </summary>
        public static void Ping()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                MatchMessage message = new MatchMessage(MatchAction.Ping, dict);
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
           
        }

        /// <summary>
        /// Sends a shot to the game partner
        /// </summary>
        /// <param name="x">The X-Coordinate</param>
        /// <param name="y">The Y-Coordinate</param>
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

        /// <summary>
        /// Sends the game state when a match is finished
        /// </summary>
        /// <param name="looser">The Jabber-ID of the looser</param>
        public static void SendGamestate(String looser)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("state", "end");
            dict.Add("looser", looser);

            MatchMessage message = new MatchMessage(MatchAction.Gamestate, dict);

            try
            {
                AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(AppCache.XmppManager.OwnID, AppCache.CurrentMatch.PartnerJID));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        /// <summary>
        /// Transfer the results of an incoming shot to the partner
        /// </summary>
        /// <param name="x">The X-Coordinate</param>
        /// <param name="y">The Y-Coordinate</param>
        /// <param name="isHit">True, if a ship is hit</param>
        /// <param name="shipInfo">Is not null, if a ship is destroyed</param>
        public static void TransferShotResult(int x, int y, Boolean isHit, ShipInfo shipInfo)
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

            if (shipInfo != null)
            {
                message.ShipInfo = shipInfo;
            }
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
