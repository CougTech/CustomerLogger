using System;
using System.IO;
using System.Text.RegularExpressions;
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

            if(Cougtech_CustomerLogger.Logging_Directory != null)
                LogDirectory_textBox.Text = Cougtech_CustomerLogger.Logging_Directory;
            else
                LogDirectory_textBox.Text = Cougtech_CustomerLogger.Logging_Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cougtech_Customer_Logs";

            this.Activate();
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public bool? Logging_En { get; set; }
        public bool TicketSubmission_Email_En { get; set; }
        public bool TicketSubmission_Rest_En { get; set; }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        public bool Authenticate()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CustomerLogger");

            string sStoredPassword = Cougtech_CustomerLogger.Admin_Password_Hashed;
            sStoredPassword = Regex.Unescape(sStoredPassword);

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

        private void LogDirectory_Button_Clicked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketSubmission_Email_En_Check_Clicked(object sender, RoutedEventArgs e)
        {
            TicketSubmission_Email_En = Cougtech_CustomerLogger.TicketSubmission_Email_Checked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketSubmission_Rest_En_Check_Clicked(object sender, RoutedEventArgs e)
        {
            TicketSubmission_Rest_En = Cougtech_CustomerLogger.TicketSubmission_Jira_Checked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketLogging_En_Check_Clicked(object sender, RoutedEventArgs e)
        {
            Logging_En = Cougtech_CustomerLogger.TicketLogging_Checked();
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
            this.Hide();
        }

        private void NewPassword_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewPassword_TextBox.Text != "")
            {
                //Open the registry for this program
                RegistryKey reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\CustomerLogger");

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
