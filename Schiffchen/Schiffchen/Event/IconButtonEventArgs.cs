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

namespace Schiffchen.Event
{
    public class IconButtonEventArgs
    {
        public class MenuEventArgs : EventArgs
        {
            private string msg;
            public MenuEventArgs(string s)
            {
                msg = s;
            }

            public String Message
            {
                get
                {
                    return this.msg;
                }
            }
        }
    }
}
