using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for SummayPage.xaml.
    /// The Summary Page displays all of the ticket information for confirmation before submission.
    /// </summary>
    public partial class SummaryPage:Page
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////
        
        //Page handlers
        public event EventHandler PageFinished;                             //Event handler for when the submit button is completed

        private System.Windows.Threading.DispatcherTimer m_SubmissionTimer; //Timer to automatically click submit after the page has timed out

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SummaryPage()
        {
            InitializeComponent();

            //Focus on the submit button
            SubmitButton.Focus();
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Resets all dynamic fields within the summary page.
        /// </summary>
        public void Reset()
        {
            FirstName.Text = "";
            ConfirmationLine.Text = "Your ticket is all set!";
            StudentID.Text = "000000000";
            Problem.Text = "";
            Description.Text = "";
        }

        /// <summary>
        /// Sets the properties for each text box on the summary page.
        /// </summary>
        /// <param name="nNid">WSU NID.</param>
        /// <param name="sProblem">Problem type.</param>
        /// <param name="sDescription">Problem description.</param>
        public void SetText(bool bIsAppointment, string sFirstName, string nNid, string sProblem, string sDescription = "")
        {
            //Set customer first name
            FirstName.Text = sFirstName;

            //Set confirmation line based on the appointment status
            if (bIsAppointment)
                ConfirmationLine.Text = "Your appointment is all set!";
            else
                ConfirmationLine.Text = "Your ticket is all set!";

            //Set the rest of the text
            StudentID.Text = nNid;
            Problem.Text = sProblem;
            Description.Text = sDescription;
        }

        /// <summary>
        /// Starts the timeout counter for the summary page.
        /// </summary>
        public void StartTimer()
        {
            m_SubmissionTimer = new System.Windows.Threading.DispatcherTimer();
            m_SubmissionTimer.Interval = TimeSpan.FromSeconds(30);              //Set the timeout for 20 seconds    
            m_SubmissionTimer.IsEnabled = true;
            m_SubmissionTimer.Tick += SubmitTimer_Tick;
        }

        /// <summary>
        /// Stops the timeout counter for the summary page.
        /// </summary>
        public void StopTimer()
        {
            if (m_SubmissionTimer != null) 
            {
                m_SubmissionTimer.IsEnabled = false;    //Disable timer
                m_SubmissionTimer = null;               //Deallocate timer
            }
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Event handler for a keypress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            //If the user pressed "Enter", submit the ticket
            if (e.Key == Key.Enter)
                SubmitButton_Click(sender, e);
        }

        /// <summary>
        /// Event handler for the submission timer tick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitTimer_Tick(object sender, EventArgs e)
        {
            //Call the handler for the submit button click
            SubmitButton_Click(sender, new RoutedEventArgs());
        }

        /// <summary>
        /// //Event handler for when the submit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            m_SubmissionTimer.IsEnabled = false;            //Disable the timeout timer
            PageFinished(new object(), new EventArgs());    //Leave this page
        }
    }
}
