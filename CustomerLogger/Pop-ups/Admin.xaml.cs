using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace CustomerLogger.Popup
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

            if (Cougtech_CustomerLogger.Wsu_Database_Url != null)
                Database_URL_Textbox.Text = Cougtech_CustomerLogger.Wsu_Database_Url;
            else
                Database_URL_Textbox.Text = "";

            this.Activate();
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public bool? Logging_En { get; set; }
        public bool TicketSubmission_Email_En { get; set; }
        public bool TicketSubmission_Rest_En { get; set; }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        public bool Authenticate()
        {
            string sStoredHashedPassword = Cougtech_CustomerLogger.Admin_Password_Hashed;

            if (sStoredHashedPassword == null)
            {
                //The password is not stored yet
                //Ask the user for a new one, then store it in a new registry entry
                NewPasswordWindow NewPasswordWindow = new NewPasswordWindow();
                NewPasswordWindow.ShowDialog();

                //NOW you may grab the registry entry
                sStoredHashedPassword = Cougtech_CustomerLogger.Admin_Password_Hashed;
            }

            //Unescape the string to remove duplicate '\'
            sStoredHashedPassword = Regex.Unescape(sStoredHashedPassword);

            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.ShowDialog();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(passwordWindow.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string sHashedPassword = System.Text.Encoding.ASCII.GetString(data);

            if (sHashedPassword == sStoredHashedPassword)
                return true;
            else
                return false;
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        private void Database_Url_OK_Button_Click(object sender, RoutedEventArgs e)
        {
            Cougtech_CustomerLogger.Wsu_Database_Url = Database_URL_Textbox.Text;

            System.Windows.MessageBox.Show("WSU database URL has been successfully saved.", "Success");
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

        private void LogDirectory_Browse_Button_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() 
            {
                Multiselect = false,
                Filter = "CSV Files (*.csv)|*.csv"
            };

            DialogResult result = fileDialog.ShowDialog();

            if(result == System.Windows.Forms.DialogResult.OK)
            {
                LogDirectory_textBox.Text = fileDialog.FileName;
                Cougtech_CustomerLogger.Logging_Directory = fileDialog.FileName;
            }
        }

        private void LogDirectory_OK_Button_Clicked(object sender, RoutedEventArgs e)
        {
            Cougtech_CustomerLogger.Logging_Directory = LogDirectory_textBox.Text;

            System.Windows.MessageBox.Show("Ticket-logging directory has been successfully saved.", "Success");
        }

        private void NewPassword_Button_Click(object sender, RoutedEventArgs e)
        {
            //Launch new password window
            NewPasswordWindow newPassword = new NewPasswordWindow();
            newPassword.ShowDialog();
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
    }
}
