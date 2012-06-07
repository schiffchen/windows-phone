using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Net.XMPP;
using System.IO;
using Schiffchen.Logic;

namespace Schiffchen
{
    public partial class MainPage : PhoneApplicationPage
    {
        public String ReceivedLog;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            ReceivedLog = String.Empty;

            AppCache.XmppManager = new XMPPManager("berttester@jabber.ccc.de", "test", this);
        }

        // Einfacher Ereignishandler für das Klicken auf die Schaltfläche, um zur zweiten Seite zu wechseln
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (AppCache.XmppManager.Client.XMPPState == XMPPState.Ready)
            {
                button1.Content = "Trenne...";
                AppCache.XmppManager.Client.Disconnect();
            }
            else if (AppCache.XmppManager.Client.XMPPState == XMPPState.Unknown || AppCache.XmppManager.Client.XMPPState == XMPPState.AuthenticationFailed)
            {
                button1.Content = "Verbinde...";                
                AppCache.XmppManager.Client.Connect();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            /*JID to = new JID("Fensterbank@jabber.ccc.de");
            AppCache.XmppManager.Client.SendChatMessage("Hallo Welt", to);

            String strXML = "<message from='berttester@jabber.ccc.de' type='chat' to='fensterbank@jabber.ccc.de'>\n<body>Hello you Facebook contact!</body>\n</message>".Replace('\'', '\"');
            AppCache.XmppManager.Client.SendRawXML(strXML);*/

            Matchmaker.Queue(AppCache.XmppManager);
        }
    }

}
