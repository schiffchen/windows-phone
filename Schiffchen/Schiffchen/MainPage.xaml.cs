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
using Matrix.Xmpp;
using System.IO;

namespace Schiffchen
{
    public partial class MainPage : PhoneApplicationPage
    {
        string debug;
        Matrix.Xmpp.Client.XmppClient client;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
        }

        // Einfacher Ereignishandler für das Klicken auf die Schaltfläche, um zur zweiten Seite zu wechseln
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            client = new Matrix.Xmpp.Client.XmppClient();
            client.SetUsername("Fensterbank");
            client.SetXmppDomain("jabber.ccc.de");
            client.Password = "test123";
            client = true;
            client.Status = "I'm chatty";
            client.Show = Matrix.Xmpp.Show.chat;
            client.OnError += new EventHandler<Matrix.ExceptionEventArgs>(client_OnError);
            client.OnReceiveXml += new EventHandler<Matrix.TextEventArgs>(client_OnReceiveXml);
            
            client.OnSendXml += new EventHandler<Matrix.TextEventArgs>(client_OnSendXml);
           
            client.OnXmlError += new EventHandler<Matrix.ExceptionEventArgs>(client_OnXmlError);
            client.OnStreamError += new EventHandler<Matrix.StreamErrorEventArgs>(client_OnStreamError);
            client.OnLogin += new EventHandler<Matrix.EventArgs>(client_OnLogin);
            client.OnAuthError += new EventHandler<Matrix.Xmpp.Sasl.SaslEventArgs>(client_OnAuthError);
            client.OnBeforeSendPresence += new EventHandler<Matrix.Xmpp.Client.PresenceEventArgs>(client_OnBeforeSendPresence);
            client.OnBind += new EventHandler<Matrix.JidEventArgs>(client_OnBind);
            client.OnBindError += new EventHandler<Matrix.Xmpp.Client.IqEventArgs>(client_OnBindError);
          
            client.Open();
            /*
            client.UserName = "Fensterbank";
            client.Password = "test123";
            client.Server = "jabber.ccc.de";
            client.Domain = "jabber.ccc.de";
            client.Resource = "Phone7App";
         */
            /*
             client.UserName = "plainuser";
             client.Server = "www.dustboystudios.de";
             client.Domain = "";
             client.Resource = "Phone7App";
             client.Password = "plainuserspw";
             client.Port = 5223;
       
         
             client.UseTLS = false;

             client.UseOldStyleTLS = false;
             client.AutoReconnect = true;
             client.AutoAcceptPresenceSubscribe = false;
             client.AutomaticallyDownloadAvatars = false;
             client.RetrieveRoster = false;

             client.OnStateChanged += new EventHandler(XMPPClient_OnStateChanged);
             client.OnXMLReceived += new XMPPClient.DelegateString(client_OnXMLReceived);
             client.OnXMLSent += new XMPPClient.DelegateString(client_OnXMLSent);
             client.OnRetrievedRoster += new EventHandler(client_OnRetrievedRoster);
             client.OnServerDisconnect += new EventHandler(client_OnServerDisconnect);
             client.Connect();
            */

            button1.Content = "Verbinde...";

        }

        void client_OnBindError(object sender, Matrix.Xmpp.Client.IqEventArgs e)
        {
            throw new NotImplementedException();
        }

        void client_OnBind(object sender, Matrix.JidEventArgs e)
        {
            throw new NotImplementedException();
        }

        void client_OnBeforeSendPresence(object sender, Matrix.Xmpp.Client.PresenceEventArgs e)
        {
            
            throw new NotImplementedException();
        }

        void client_OnAuthError(object sender, Matrix.Xmpp.Sasl.SaslEventArgs e)
        {
            throw new NotImplementedException();
        }

        void client_OnLogin(object sender, Matrix.EventArgs e)
        {
            
            throw new NotImplementedException();
        }

        void client_OnStreamError(object sender, Matrix.StreamErrorEventArgs e)
        {
            
            throw new NotImplementedException();
        }

        void client_OnXmlError(object sender, Matrix.ExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        void client_OnSendXml(object sender, Matrix.TextEventArgs e)
        {
            debug += e.Text;
        }

        void client_OnReceiveXml(object sender, Matrix.TextEventArgs e)
        {
            debug += e.Text;
        }

        void client_OnError(object sender, Matrix.ExceptionEventArgs e)
        {
            int i = 0;
        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {

            client.SendPresence(client.Show, "Hallo Welt");

        }
    }
}