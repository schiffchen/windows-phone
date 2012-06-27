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

using Schiffchen.Logic;
using System.Net.XMPP;

namespace Schiffchen
{
    /// <summary>
    /// The main page of the app.
    /// Handles login, contacting matchmaker and starting of games
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates the main page and initialize all components
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            CheckDisplayedComponents();
        }
        
        /// <summary>
        /// Connects to the Jabber server
        /// </summary>
        private void HandleConnect()
        {
            if (cbAnonymous.IsChecked == true)
            {
                // This is for debugging. Allows to start a match without beeing logged in
                //MessageBox.Show("Not implemented!");
                AppCache.XmppManager = new XMPPManager(tbJID.Text, tbPwd.Password, this);
                Match m = new Match("123", new JID(tbJID.Text), new JID("gegnerfdgdfg@jabber.ccc.de"));
                StartGame(m);
            }
            else
            {
                if (String.IsNullOrEmpty(tbJID.Text) || String.IsNullOrEmpty(tbPwd.Password))
                {
                    MessageBox.Show("Please insert your Jabber-ID and Password!", "Error", MessageBoxButton.OK);
                }
                else
                {
                    AppCache.XmppManager = new XMPPManager(tbJID.Text, tbPwd.Password, this);
                    AppCache.XmppManager.Client.Connect();
                }
            }
        }

        /// <summary>
        /// References AppCache.CurrentMatch to the given match and navigates to the XNA part of the game
        /// </summary>
        /// <param name="newMatch">The new match</param>
        public void StartGame(Match newMatch)
        {
            AppCache.CurrentMatch = newMatch;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Checks which components and controls are enabled and which are disabled
        /// </summary>
        public void CheckDisplayedComponents()
        {
            if (AppCache.XmppManager == null || AppCache.XmppManager.Client == null || AppCache.XmppManager.Client.XMPPState != XMPPState.Ready)
            {
                btnDirect.IsEnabled = false;
                btnSearch.IsEnabled = false;
                tbPartnerJID.IsEnabled = false;
            }
            else
            {
                btnDirect.IsEnabled = true;
                btnSearch.IsEnabled = true;
                tbPartnerJID.IsEnabled = true;
            }
        }

        #region Events

        /// <summary>
        /// Is called when the connect button is clicked
        /// </summary>
        /// <param name="sender">The clicked button</param>
        /// <param name="e">The event arguments</param>
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (AppCache.XmppManager == null)
            {
                HandleConnect();
            }
            else
            {
                if (AppCache.XmppManager.Client.XMPPState == XMPPState.Ready)
                {
                    AppCache.XmppManager.Client.Disconnect();
                }
                else if (AppCache.XmppManager.Client.XMPPState == XMPPState.Unknown || AppCache.XmppManager.Client.XMPPState == XMPPState.AuthenticationFailed)
                {
                    HandleConnect();
                }
            }
        }

        /// <summary>
        /// Is called when the checkbox for anonymous login is checked
        /// </summary>
        /// <param name="sender">The checkbox</param>
        /// <param name="e">The event arguments</param>
        private void cbAnonymous_Checked(object sender, RoutedEventArgs e)
        {
            tbPwd.IsEnabled = false;
            tbJID.IsEnabled = false;
        }

        /// <summary>
        /// Is called when the checkbox for anonymous login is unchecked
        /// </summary>
        /// <param name="sender">The checkbox</param>
        /// <param name="e">The event arguments</param>
        private void cbAnonymous_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPwd.IsEnabled = true;
            tbJID.IsEnabled = true;
        }

        /// <summary>
        /// Is called when the button for direct connection is clicked
        /// </summary>
        /// <param name="sender">The clicked button</param>
        /// <param name="e">The event arguments</param>
        private void btnDirect_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbPartnerJID.Text))
            {
                MessageBox.Show("Please enter the Jabber-ID of your game partner.", "Error", MessageBoxButton.OK);
            }
            else
            {
                try
                {
                    Match newMatch = new Match("1337", AppCache.XmppManager.OwnID, new JID(tbPartnerJID.Text));
                    StartGame(newMatch);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter a valid Jabber-ID of your game partner.", "Error", MessageBoxButton.OK);
                }
            }
        }

        /// <summary>
        /// Is called when the button for searching a partner is clicked
        /// </summary>
        /// <param name="sender">The clicked button</param>
        /// <param name="e">The event arguments</param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearch.Content.Equals("Search Partner"))
            {
                ledWaitingState.Visibility = System.Windows.Visibility.Visible;
                lblSearchState.Text = "Connecting to Matchmaker...";
                btnSearch.Content = "Stop Search";
                ledWaitingState.Fill = AppCache.cOrange;
                AppCache.XmppManager.RequestPlayerFromMatchmaker();
                AppCache.XmppManager.IncomingQueuingPing += new EventHandler<Event.MessageEventArgs>(XmppManager_IncomingPing);
            }
            else
            {
                lblSearchState.Text = "";
                ledWaitingState.Visibility = System.Windows.Visibility.Collapsed;
                btnSearch.Content = "Search Partner";
                AppCache.XmppManager.StopRequestPlayerFromMatchmaker();
                AppCache.XmppManager.IncomingQueuingPing -= XmppManager_IncomingPing;
            }
        }

        /// <summary>
        /// Is called when a ping by the matchmaker is received.
        /// Displays the state to the app.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message event arguments</param>
        void XmppManager_IncomingPing(object sender, Event.MessageEventArgs e)
        {            
            this.Dispatcher.BeginInvoke(delegate
            {
                this.lblSearchState.Text = "Searching partner. Please wait...";
                this.ledWaitingState.Fill = AppCache.cYellow;
            });
        }

        #endregion

    }
}