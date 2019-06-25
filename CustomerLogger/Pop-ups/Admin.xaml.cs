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

        private bool m_bUnsavedChanges;
        private MainWindow m_MainWindow;
          
        //  Constructors    ///////////////////////////////////////////////////////////////////////

        public AdminWindow(MainWindow window)
        {
            InitializeComponent();

            m_MainWindow = window;
            if(Cougtech_CustomerLogger.Open_Time != null)
            {
                string sOpenTime = Cougtech_CustomerLogger.Open_Time;

                CTHours_HourOpen_Combo.SelectedValue = sOpenTime.Substring(0, 2);
                CTHours_MinOpen_Combo.SelectedValue = sOpenTime.Substring(3, 2);
                CTHours_AmPm_Combo.SelectedValue = sOpenTime.Substring(5);
            }
            else
            {
                CTHours_HourOpen_Combo.SelectedValue = "";
                CTHours_MinOpen_Combo.SelectedValue = "";
                CTHours_AmPm_Combo.SelectedValue = "";
            }

            if (Cougtech_CustomerLogger.Close_Time != null)
            {
                string sCloseTime = Cougtech_CustomerLogger.Close_Time;

                CTHours_HourCloses_Combo.SelectedValue = sCloseTime.Substring(0, 2);
                CTHours_MinCloses_Combo.SelectedValue = sCloseTime.Substring(3, 2);
                CTHours_AmPm_Combo2.SelectedValue = sCloseTime.Substring(5);
            }
            else
            {
                CTHours_HourCloses_Combo.SelectedValue = "";
                CTHours_MinCloses_Combo.SelectedValue = "";
                CTHours_AmPm_Combo2.SelectedValue = "";
            }

            if (Cougtech_CustomerLogger.Wsu_Database_Url != null)
                Database_URL_Textbox.Text = Cougtech_CustomerLogger.Wsu_Database_Url;
            else
                Database_URL_Textbox.Text = "";

            if(Cougtech_CustomerLogger.Ticketing_Email_Address != null)
                TicketSubmission_Email_Textbox.Text = Cougtech_CustomerLogger.Ticketing_Email_Address;
            else
                TicketSubmission_Email_Textbox.Text = "";

            if (Cougtech_CustomerLogger.Ticketing_Rest_Url != null)
                TicketSubmission_Rest_Textbox.Text = Cougtech_CustomerLogger.Ticketing_Rest_Url;
            else
                TicketSubmission_Rest_Textbox.Text = "";

            if (Cougtech_CustomerLogger.Logging_Directory != null)
                LogDirectory_textBox.Text = Cougtech_CustomerLogger.Logging_Directory;
            else
                LogDirectory_textBox.Text = Cougtech_CustomerLogger.Logging_Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Cougtech_Customer_Logs";

            UnsavedChanges = false;

            this.Activate();
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public bool? Logging_En { get; set; }

        public bool? TicketSubmission_Email_En { get; set; }

        public bool? TicketSubmission_Rest_En { get; set; }

        private bool UnsavedChanges
        {
            get { return m_bUnsavedChanges; }
            set 
            {
                if(value == true)
                {
                    Apply_Button.IsEnabled = true;
                    m_bUnsavedChanges = true;
                }
                else
                {
                    Apply_Button.IsEnabled = false;
                    m_bUnsavedChanges = false;
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Apply_Button_Click(object sender, RoutedEventArgs e)
        {
            Cougtech_CustomerLogger.Open_Time = $"{CTHours_HourOpen_Combo.Text}:{CTHours_MinOpen_Combo.Text}{CTHours_AmPm_Combo.Text}";
            Cougtech_CustomerLogger.Close_Time = $"{CTHours_HourCloses_Combo.Text}:{CTHours_MinCloses_Combo.Text}{CTHours_AmPm_Combo2.Text}";

            Cougtech_CustomerLogger.Set_CougtechHours(Cougtech_CustomerLogger.Open_Time, Cougtech_CustomerLogger.Close_Time);

            Cougtech_CustomerLogger.Logging_Directory = LogDirectory_textBox.Text;
            Cougtech_CustomerLogger.Ticketing_Email_Address = TicketSubmission_Email_Textbox.Text;
            Cougtech_CustomerLogger.Ticketing_Rest_Url = TicketSubmission_Rest_Textbox.Text;
            Cougtech_CustomerLogger.Wsu_Database_Url = Database_URL_Textbox.Text;

            UnsavedChanges = false;
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

        private void CougtechHours_Changed(object sender, EventArgs e)
        {
            UnsavedChanges = true;
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
                UnsavedChanges = true;

                //TODO move
                Cougtech_CustomerLogger.Logging_Directory = fileDialog.FileName;
            }
        }

        private void NewPassword_Button_Click(object sender, RoutedEventArgs e)
        {
            //Launch new password window
            NewPasswordWindow newPassword = new NewPasswordWindow();
            newPassword.ShowDialog();
        }

        private void Textbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UnsavedChanges = true;
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
