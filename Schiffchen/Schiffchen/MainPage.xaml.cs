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

namespace Schiffchen
{
    public partial class MainPage : PhoneApplicationPage
    {
   

        XMPPClient XMPPClient;
        public String ReceivedLog;
        public Brush cRed;
        public Brush cYellow;
        public Brush cGreen;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            ReceivedLog = String.Empty;
            cRed = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 190, 0, 0));
            cGreen = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 68, 140, 0));
            cYellow = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 255, 224, 98));
            InitializeXMPPClient();
        }

        private void InitializeXMPPClient()
        {
            XMPPClient = new XMPPClient();

            XMPPClient.UserName = "berttester";
            XMPPClient.Password = "test";
            XMPPClient.Server = "jabber.ccc.de";
            XMPPClient.Domain = "jabber.ccc.de";
            XMPPClient.Resource = Guid.NewGuid().ToString();
            XMPPClient.Port = 5223;

            XMPPClient.OnServerDisconnect += new EventHandler(XMPPClient_OnServerDisconnect);
            XMPPClient.OnXMLSent +=new System.Net.XMPP.XMPPClient.DelegateString(XMPPClient_OnXMLSent);
            XMPPClient.OnXMLReceived += new System.Net.XMPP.XMPPClient.DelegateString(XMPPClient_OnXMLReceived);
            XMPPClient.JingleSessionManager.OnNewSession += new System.Net.XMPP.Jingle.JingleSessionManager.DelegateJingleSessionEventWithInfo(JingleSessionManager_OnNewSession);
            XMPPClient.OnStateChanged += new EventHandler(XMPPClient_OnStateChanged);
            XMPPClient.OnRetrievedRoster += new EventHandler(XMPPClient_OnRetrievedRoster);
           
            XMPPClient.UseOldStyleTLS = true;
            XMPPClient.UseTLS = true;

            XMPPClient.AutoAcceptPresenceSubscribe = false;
            XMPPClient.AutomaticallyDownloadAvatars = false;
            XMPPClient.RetrieveRoster = true;
            XMPPClient.AutoReconnect = true;
            
        }

        void XMPPClient_OnRetrievedRoster(object sender, EventArgs e)
        {
            int i = 0;
        }

        // Einfacher Ereignishandler für das Klicken auf die Schaltfläche, um zur zweiten Seite zu wechseln
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (XMPPClient.XMPPState == XMPPState.Ready)
            {
                button1.Content = "Trenne...";
                XMPPClient.Disconnect();
            }
            else if (XMPPClient.XMPPState == XMPPState.Unknown || XMPPClient.XMPPState == XMPPState.AuthenticationFailed)
            {
                button1.Content = "Verbinde...";                
                XMPPClient.Connect();
            }
        }

            

        void JingleSessionManager_OnNewSession(string strSession, System.Net.XMPP.Jingle.JingleIQ iq, XMPPClient client)
        {
            int i = 0;
        }

        void XMPPClient_OnXMLReceived(XMPPClient client, string strXML)
        {
            Dispatcher.BeginInvoke(delegate
            {                
                this.tbReceived.Text = strXML;                
            });
        }



        void XMPPClient_OnXMLSent(XMPPClient client, string strXML)
        {
            Dispatcher.BeginInvoke(delegate
            {
                tbSent.Text = strXML;
            });
        }

        void XMPPClient_OnServerDisconnect(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(delegate
            {
                this.button1.Content = "Nicht verbunden";
            });
        }

        void XMPPClient_OnStateChanged(object sender, EventArgs e)
        {
            
            switch (XMPPClient.XMPPState)
            {
                case XMPPState.Ready:
                    XMPPClient.PresenceStatus.PresenceType = PresenceType.available;
                    XMPPClient.PresenceStatus.Status = "online";
                    XMPPClient.PresenceStatus.PresenceShow = PresenceShow.chat;
                    XMPPClient.UpdatePresence();
                    Dispatcher.BeginInvoke(delegate
                    {
                        this.ledState.Fill = cGreen;
                        this.button1.Content = "Trennen";
                    });
                    break;
                case XMPPState.Unknown:
                case XMPPState.AuthenticationFailed:
                    Dispatcher.BeginInvoke(delegate
                    {
                        this.ledState.Fill = cRed;
                        this.button1.Content = "Verbinden";
                    });
                    break;
                default:
                    Dispatcher.BeginInvoke(delegate
                    {
                        this.ledState.Fill = cYellow;
                    });
                    break;
            }
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            JID to = new JID("Fensterbank@jabber.ccc.de");
            XMPPClient.SendChatMessage("Hallo Welt", to);

            String strXML = "<message from='berttester@jabber.ccc.de' type='chat' to='fensterbank@jabber.ccc.de'>\n<body>Hello you Facebook contact!</body>\n</message>".Replace('\'', '\"');
            XMPPClient.SendRawXML(strXML);

        }
    }

}
