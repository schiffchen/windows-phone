using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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
            String xml = "<message from=\"" + Manager.OwnID.FullJID + "\" id=\"" + Guid.NewGuid() + "\" to=\"" + MatchmakerJID.BareJID + "\" type=\"normal\">\n<battleship xmlns=\"http://battleship.me/xmlns/\">\n<queueing action=\"request\" /></battleship></message>";
            //IncomingMessage m = new IncomingMessage();
            Manager.Client.SendRawXML(xml);
            
        }


    }
}
