using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    
    //Displays an arbitrary message
    //used for errors or feedback or lamas, or whatever
    //just keep it classy
    public partial class MessageWindow:Window
    {

        private System.Windows.Threading.DispatcherTimer openTimer;

        //message is the text you want to display
        //If you put a new line in the message string then the message
        //should be displayed on the two lines.
        //interval is the seconds you want it to desplay for
        public MessageWindow(string message, double interval)
        {
            InitializeComponent();
    
            MessageTextBox.Text = message;

            // Display this window for interval seconds
            openTimer = new System.Windows.Threading.DispatcherTimer();
            openTimer.Interval = TimeSpan.FromSeconds(interval);
            openTimer.IsEnabled = true;
            openTimer.Tick += new EventHandler(this.close_window);

            OKButton.Focus(); // focus on OK button so user can press enter to close popup
        }

        //when you are done click ok
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //auto closed after time is done
        private void close_window(Object sender, EventArgs args)
        {
            openTimer.IsEnabled = false;
            this.Close();
        }
    }
}
