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

namespace Schiffchen.Controls
{
    public class WatermarkedTextbox
    {
        public class WatermarkTextBox : TextBox
        {
            public string WatermarkText
            {
                get { return (String)this.GetValue(WatermarkTextProperty); }
                set { this.SetValue(WatermarkTextProperty, value); }
            }
            public static readonly DependencyProperty WatermarkTextProperty =
                DependencyProperty.Register("WatermarkText", typeof(String), typeof(WatermarkTextBox), new PropertyMetadata(String.Empty));

            public SolidColorBrush WatermarkForeGroundColor = Application.Current.Resources["PhoneTextBoxReadOnlyBrush"] as SolidColorBrush;


            public WatermarkTextBox()
            {
                Loaded += new RoutedEventHandler(WatermarkTextBox_Loaded);
            }

            void WatermarkTextBox_Loaded(object sender, RoutedEventArgs e)
            {
                this.Text = !String.IsNullOrEmpty(WatermarkText) ? WatermarkText : String.Empty;
                this.Foreground = WatermarkForeGroundColor;
            }

            protected override void OnGotFocus(RoutedEventArgs e)
            {
                base.OnGotFocus(e);

                if (WatermarkText == this.Text)
                {
                    this.Text = String.Empty;
                    this.Foreground = Application.Current.Resources["PhoneTextBoxForegroundBrush"] as SolidColorBrush;
                }
            }

            protected override void OnLostFocus(RoutedEventArgs e)
            {
                base.OnLostFocus(e);

                if (String.IsNullOrEmpty(this.Text))
                {
                    this.Text = !String.IsNullOrEmpty(WatermarkText) ? WatermarkText : String.Empty;
                    this.Foreground = WatermarkForeGroundColor;
                }
            }
        }
    }
}
