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

namespace Schiffchen.Logic
{
    public class AppCache
    {
        public static XMPPManager XmppManager;
        public static Match CurrentMatch;
        public static System.Windows.Media.SolidColorBrush cRed;
        public static  System.Windows.Media.SolidColorBrush cGreen;
        public static System.Windows.Media.SolidColorBrush cYellow;

        static AppCache()
        {
            cRed = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 190, 0, 0));
            cGreen = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 68, 140, 0));
            cYellow = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 255, 224, 98));
        }
        
    }


}
