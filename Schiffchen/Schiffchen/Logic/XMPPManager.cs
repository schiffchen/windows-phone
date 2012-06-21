using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;


using System.Windows.Shapes;
using System.Net.XMPP;
using System.Windows.Threading;
using Schiffchen.Logic.Messages;

namespace Schiffchen.Logic
{
    public class XMPPManager
    {
        private XMPPClient client;
        private MainPage mainPage;
        private Int32 queueID;
        private DispatcherTimer pingTimer;
        private Boolean QueuingProcess;

        public XMPPClient Client
        {
            get { return this.client; }
        }

        public Int32 QueueID
        {
            get { return this.queueID; }
        }

        public JID OwnID
        {
            get { return this.client.JID; }
        }

        /// <summary>
        /// Constructor with login credentials
        /// </summary>
        /// <param name="jabberID">The JabberID as a simple string</param>
        /// <param name="password">The password to login to this account</param>
        /// <param name="mainPage">The MainPage-Control. I think you can give me 'this'</param>
        public XMPPManager(String jabberID, String password, MainPage mainPage)
        {
            this.client = new XMPPClient();
            this.client.JID = new JID(jabberID);
            this.client.Password = password;
            this.client.Resource = "battleshipme";
            this.mainPage = mainPage;

            InitializeClient();
        }

        /// <summary>
        /// Constructor with login credentials
        /// </summary>
        /// <param name="jabberID">The JID-Object of the JabberID</param>
        /// <param name="password">The password to login to this account</param>
        /// <param name="mainPage">The MainPage-Control. I think you can give me 'this'</param>
        public XMPPManager(JID jabberID, String password, MainPage mainPage)
        {
            this.client = new XMPPClient();
            this.client.JID = jabberID;
            this.client.Password = password;
            this.client.Resource = "battleshipme";
            this.mainPage = mainPage;

            InitializeClient();
        }

        /// <summary>
        /// Constructor provides an anonymous login using our own XMPP server
        /// </summary>
        /// <param name="mainPage">The MainPage-Control. I think you can give me 'this'</param>
        public XMPPManager(MainPage mainPage)
        {
            this.client = new XMPPClient();
            this.client.JID = new JID("anonymous@battleship.me");
            this.client.Password = String.Empty; // To be filled in!
            this.client.Resource = String.Empty;
            this.mainPage = mainPage;

            InitializeClient();
        }

        private void InitializeClient()
        {
            this.client.Server = this.client.JID.Domain;
            
            this.client.Port = 5223;
            

            this.client.OnServerDisconnect += new EventHandler(XmppClient_OnServerDisconnect);
            this.client.OnXMLSent += new XMPPClient.DelegateString(XmppClient_OnXMLSent);
            this.client.OnXMLReceived += new XMPPClient.DelegateString(XmppClient_OnXMLReceived);
            this.client.OnStateChanged += new EventHandler(XmppClient_OnStateChanged);

            this.client.UseOldStyleTLS = true;
            this.client.UseTLS = true;

            this.client.AutoAcceptPresenceSubscribe = false;
            this.client.AutomaticallyDownloadAvatars = false;
            this.client.RetrieveRoster = true;
            this.client.AutoReconnect = true;
        }

        void XmppClient_OnStateChanged(object sender, EventArgs e)
        {
            switch (client.XMPPState)
            {
                case XMPPState.Ready:
                    client.PresenceStatus.PresenceType = PresenceType.available;
                    client.PresenceStatus.Status = "online";
                    client.PresenceStatus.Priority = -128;
                    client.PresenceStatus.PresenceShow = PresenceShow.chat;
                    client.UpdatePresence();
                    mainPage.Dispatcher.BeginInvoke(delegate
                    {
                        mainPage.ledState.Fill = AppCache.cGreen;
                        mainPage.btnConnect.Content = "Disconnect";
                        mainPage.lblConnectionState.Text = "Connected";
                        mainPage.CheckDisplayedComponents();
                    });
                    break;
        
                case XMPPState.Unknown:
                case XMPPState.AuthenticationFailed:
                    mainPage.Dispatcher.BeginInvoke(delegate
                    {
                        mainPage.ledState.Fill = AppCache.cRed;
                        mainPage.btnConnect.Content = "Connect";
                        mainPage.CheckDisplayedComponents();

                        if (client.XMPPState == XMPPState.AuthenticationFailed)
                        {
                            mainPage.lblConnectionState.Text = "Authentication Failed";
                        }
                        else
                        {
                            mainPage.lblConnectionState.Text = "Disconnected";
                        }
                    });
                    break;
                default:
                    mainPage.Dispatcher.BeginInvoke(delegate
                    {
                        mainPage.ledState.Fill = AppCache.cYellow;
                        mainPage.lblConnectionState.Text = client.XMPPState.ToString();
                        mainPage.CheckDisplayedComponents();
                    });
                    break;
            }
        }

        void XmppClient_OnXMLReceived(XMPPClient client, string strXML)
        {
            BattleshipMessage bMessage = XMLManager.GetBattleshipMessage(strXML);
            if (bMessage != null)
            {
                if (bMessage is QueuingMessage)
                {
                    Match m = null;

                    QueuingMessage qMessage = (QueuingMessage)bMessage;
                    String s = "";
                    if (qMessage.Action== Enum.QueueingAction.success)
                    {

                        s = "Searching partner. Please wait...";
                        this.queueID = qMessage.ID;
                    }
                    else if (qMessage.Action == Enum.QueueingAction.ping)
                    {
                        s = "Searching partner. Please wait...";
                    }
                    else if (qMessage.Action == Enum.QueueingAction.assign)
                    {
                        if (QueuingProcess) {
                            QueuingProcess = false;
                            Matchmaker.Assigned(this, qMessage.JID, qMessage.MatchID);
                            m = new Match(qMessage.MatchID, this.OwnID, qMessage.JID);
                        }
                        s = "Assigned with partner!";
                        
                    }
                        mainPage.Dispatcher.BeginInvoke(delegate
                    {
                        mainPage.lblSearchState.Text = s;
                        if (m != null)
                        {
                            mainPage.StartGame(m);
                        }
                    });
                }
                else if (bMessage is MatchMessage)
                {
                    MatchMessage mMessage = (MatchMessage)bMessage;
                    switch (mMessage.Action)
                    {
                        case Enum.MatchAction.diceroll:
                            if (AppCache.CurrentMatch != null)
                            {
                                AppCache.CurrentMatch.PartnerDice = mMessage.Dice;
                            }
                            break;
                    }
                }
            }
        }

        void XmppClient_OnXMLSent(XMPPClient client, string strXML)
        {
            
        }

        void XmppClient_OnServerDisconnect(object sender, EventArgs e)
        {                   
            
        }

        public void RequestPlayerFromMatchmaker()
        {
            QueuingProcess = true;
            Matchmaker.Queue(this);
            pingTimer = new DispatcherTimer();
            pingTimer.Tick += new EventHandler(pingTimer_Tick);
            pingTimer.Interval = new TimeSpan(0, 0, 15);
            pingTimer.Start();
        }

        void pingTimer_Tick(object sender, EventArgs e)
        {
            if (QueuingProcess)
            {
                Matchmaker.Ping(this);
            }
            else
            {
                pingTimer.Stop();
            }
        }
    }
}
