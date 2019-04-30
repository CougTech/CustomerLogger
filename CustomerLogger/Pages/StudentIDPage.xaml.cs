using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for SudentIDPage.xaml.
    /// The student ID page accepts a nunmeric string from the customer which is resolved to their WSU NID.
    /// </summary>
    public partial class StudentIDPage:Page
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        public event EventHandler PageFinished;

        private bool m_bIsAdmin, m_bIsQuickPick;
        private string m_sNid;

        //  Constructors    ///////////////////////////////////////////////////////////////////////
    
        /// <summary>
        /// Default Constructor
        /// </summary>
        public StudentIDPage()
        {
            InitializeComponent();

            m_bIsAdmin = false;
            m_bIsQuickPick = false;

            SubmitButton.IsEnabled = false; //Disable submit button
            StudentNumberTextBox.Focus();   //Focus on the textbox
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string Nid
        {
            get { return m_sNid; }
        }

        public bool IsAdmin
        {
            get { return m_bIsAdmin; }
        }

        public bool IsQuickPick
        {
            get { return m_bIsQuickPick; }
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        public void Reset()
        {
            m_bIsAdmin = false;
            m_bIsQuickPick = false;

            StudentNumberTextBox.Text = "";

            Set_SubmitButton_Disabled();
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        private void Set_SubmitButton_Admin()
        {
            SubmitButton.Background = System.Windows.Media.Brushes.IndianRed;
            SubmitButton.Content = "Admin Login";
            SubmitButton.IsEnabled = true;
        }

        private void Set_SubmitButton_Disabled()
        {
            SubmitButton.Background = System.Windows.Media.Brushes.Gray;
            SubmitButton.Content = "Next";
            SubmitButton.IsEnabled = false;
        }

        private void Set_SubmitButton_Enabled()
        {
            SubmitButton.Background = System.Windows.Media.Brushes.White;
            SubmitButton.Content = "Next";
            SubmitButton.IsEnabled = true;
        }

        private void Set_SubmitButton_QuickPick()
        {
            SubmitButton.Background = System.Windows.Media.Brushes.RoyalBlue;
            SubmitButton.Content = "Quick-Pick";
            SubmitButton.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for when the textbox is focused.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //Select all text within the textbox in order to rewrite entry
            StudentNumberTextBox.SelectAll();
        }

        /// <summary>
        /// Event handler for a keypress within the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentNumberTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //If the user has pressed "Enter", click the submit button
            if (e.Key == Key.Enter)
                SubmitButton_Click(sender, e);
        }

        /// <summary>
        /// Event handler for when the textbox text changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Call the control layer's handler for this event and read the return code
            string sCode = Cougtech_CustomerLogger.Nid_TextChanged(StudentNumberTextBox.Text);

            //Now set the functionality of the next button depending on the return code
            switch(sCode)
            {
                case "WI":      //Standard walk-in ticket

                    //Color the submit button white and display ""Next"
                    m_bIsAdmin = m_bIsQuickPick = false;
                    Set_SubmitButton_Enabled();
                    break;

                case "QP":      //Quick-pick ticket

                    //Color the submit button green and display "Quick-Code"
                    m_bIsAdmin = false;
                    m_bIsQuickPick = true;
                    Set_SubmitButton_QuickPick();
                    break;

                case "Admin":   //The user is accessing the admin page

                    //Color the submit button crimson and display "Admin"
                    m_bIsAdmin = true;
                    m_bIsQuickPick = false;
                    Set_SubmitButton_Admin();
                    break;

                default:        //No return code was given. The data within the textbox is invalid

                    //Color the submit button gray and disable
                    m_bIsAdmin = m_bIsQuickPick = false;
                    Set_SubmitButton_Disabled();
                    break;
            }
        }

        /// <summary>
        /// Event handler for when the submit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitButton.IsEnabled)
            {
                //Set the NID string to the textbox content
                m_sNid = StudentNumberTextBox.Text;

                //Add 0 to the front of the string if the submission is 8 digits long and a number
                if (!m_bIsAdmin && !m_bIsQuickPick && (m_sNid.Length == 8))
                    m_sNid = "0" + m_sNid;

                //Leave this page
                PageFinished(new object(), new EventArgs());
            }

        }
    }
}
