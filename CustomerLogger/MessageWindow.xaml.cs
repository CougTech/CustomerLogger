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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow:Window {

        private System.Windows.Threading.DispatcherTimer openTimer;

        public MessageWindow(string message) {
            InitializeComponent();
    
            MessageTextBox.Text = message;

            // Display this window for 3 seconds
            openTimer = new System.Windows.Threading.DispatcherTimer();
            openTimer.Interval = TimeSpan.FromSeconds(3.0);
            openTimer.IsEnabled = true;
            openTimer.Tick += new EventHandler(this.close_window);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            //Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith((x) => close_window());
            this.Close();
        }

        private void close_window(Object sender, EventArgs args) {
            openTimer.IsEnabled = false;
            this.Close();
        }
    }
}
