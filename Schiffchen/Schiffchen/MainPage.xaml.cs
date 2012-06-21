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
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            CheckDisplayedComponents();
        }

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

        private void HandleConnect()
        {
            if (cbAnonymous.IsChecked == true)
            {
                MessageBox.Show("Not implemented!");
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

        private void cbAnonymous_Checked(object sender, RoutedEventArgs e)
        {
            tbPwd.IsEnabled = false;
            tbJID.IsEnabled = false;
        }

        private void cbAnonymous_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPwd.IsEnabled = true;
            tbJID.IsEnabled = true;
        }

        private void btnDirect_Click(object sender, RoutedEventArgs e)
        {
            lblSearchState.Text = "Connecting to Matchmaker...";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            AppCache.XmppManager.RequestPlayerFromMatchmaker();
        }

        public void StartGame(Match newMatch)
        {
            AppCache.CurrentMatch = newMatch;
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

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
 
    }
}