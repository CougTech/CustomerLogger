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
    public partial class MessageWindow:Window
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer m_Timer;

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="message">Message to display within the window.</param>
        /// <param name="interval">Time in seconds that the window will stay before auto-destructing.</param>
        public MessageWindow(string message, double interval)
        {
            InitializeComponent();
    
            //Set the text
            MessageTextBox.Text = message;

            //Initialize the timeout timer
            m_Timer = new System.Windows.Threading.DispatcherTimer();
            m_Timer.Interval = TimeSpan.FromSeconds(interval);
            m_Timer.IsEnabled = true;
            m_Timer.Tick += Close_Window;

            OKButton.Focus(); // focus on OK button so user can press enter to close popup
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Event handler for when the OK button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //Close the window
            Close_Window(sender, e);
        }

        /// <summary>
        /// Event handler which closes the message window when called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Close_Window(Object sender, EventArgs args)
        {
            m_Timer.IsEnabled = false;
            this.Close();
        }
    }
}
