using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class AdminWindow:Window
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        MainWindow m_MainWindow;
          
        //  Constructors    ///////////////////////////////////////////////////////////////////////

        public AdminWindow(MainWindow window)
        {
            InitializeComponent();
            m_MainWindow = window;

            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\CustomerLogger");

            LogDirectory_textBox.Text = reg.GetValue("Log_Directory").ToString();

            this.Activate();
        }

        public bool Authenticate()
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\CustomerLogger");

            string sStoredPassword = reg.GetValue("AdminPassword").ToString();
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.ShowDialog();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(passwordWindow.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string sHashedPassword = System.Text.Encoding.ASCII.GetString(data);

            if (sHashedPassword == sStoredPassword)
                return true;
            else
                return false;
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketSubmission_Email_En_Check_Checked(object sender, RoutedEventArgs e)
        {
            TicketSubmission_Email_En_Check.IsChecked = Cougtech_CustomerLogger.TicketSubmission_Email_Checked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketSubmission_Rest_En_Check_Checked(object sender, RoutedEventArgs e)
        {
            TicketSubmission_Rest_En_Check.IsChecked = Cougtech_CustomerLogger.TicketSubmission_Jira_Checked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketLogging_En_Check_Checked(object sender, RoutedEventArgs e)
        {
            TicketLogging_En_Check.IsChecked = Cougtech_CustomerLogger.TicketLogging_Checked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            m_MainWindow.Close();
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogDirectory_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewPassword_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewPassword_TextBox.Text != "")
            {
                //Open the registry for this program
                RegistryKey reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\CustomerLogger");

                //Hash the text within the password textbox
                byte[] data = System.Text.Encoding.ASCII.GetBytes(NewPassword_TextBox.Text);
                data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                string sHashedPassword = System.Text.Encoding.ASCII.GetString(data);

                //Save the hashed password into the registry
                reg.SetValue("AdminPassword", sHashedPassword);
            }
        }

        private void NewPassword_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NewPassword_TextBox.Text = "";
        }

        private void NewPassword_TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                NewPassword_Button_Click(sender, e);
        }
    }
}
