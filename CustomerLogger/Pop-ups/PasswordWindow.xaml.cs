using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for PasswordWindow.xaml
    /// The password window requests an admin password from the user before opening the admin window.
    /// </summary>
    public partial class PasswordWindow : Window
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private string m_sPassword;

        //  Constructor ///////////////////////////////////////////////////////////////////////////

        public PasswordWindow()
        {
            InitializeComponent();

            m_sPassword = "";
            PwLabel.Content = "Enter Password:";

            PasswordBox.Focus();
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string Password
        {
            get { return m_sPassword; }
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Event handler for when the OK button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            m_sPassword = PasswordBox.Password;            

            this.Close();
        }

        /// <summary>
        /// Event handler for when a key is presed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OkButton_Click(sender, e);
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
