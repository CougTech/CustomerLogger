using CSV;
using Jira_REST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using WSU_Database;

namespace CustomerLogger
{
    public static class Cougtech_CustomerLogger
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private static bool m_bTicketing_Email, m_bTicketing_Jira, m_bStartup;
        private static string m_sLogFilePath;

        private static Dictionary<string, QuickCode_t> m_QuickCodes = new Dictionary<string, Cougtech_CustomerLogger.QuickCode_t>();

        private static CougtechTicket m_CustomerTicket;                                     //Customer ticket
        private static JiraRestRequestHandler m_RestClient = new JiraRestRequestHandler();  //Jira REST client
        private static TicketLogger m_TicketLogger = new TicketLogger();                    //Ticket logger

        private static TimeSpan m_StartTime = new TimeSpan(8, 00, 0);                      //Time to start logging each day (8am)
        private static TimeSpan m_EndTime = new TimeSpan(17, 00, 0);                       //Time to end logging each day (5pm)
        private static System.Windows.Threading.DispatcherTimer m_StartTimer, m_EndTimer;  //Thread timers

        //  Structs ///////////////////////////////////////////////////////////////////////////////

        public struct QuickCode_t
        {
            //Fields
            readonly public string f_sCode, f_sProblem, f_sDescription;

            //Constructor
            public QuickCode_t(string sCode, string sProblem, string sDescription)
            {
                f_sCode = sCode;
                f_sProblem = sProblem;
                f_sDescription = sDescription;
            }
        }

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        static Cougtech_CustomerLogger()
        {
            m_bTicketing_Email = false;
            m_bTicketing_Jira = false;
            m_bStartup = true;
            m_sLogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //TODO create new log path

            //Setup quick codes
            m_QuickCodes.Add("GI", new QuickCode_t("GI", "General Information", "Customer is looking for general information."));
            m_QuickCodes.Add("CT", new QuickCode_t("CT", "Coretech", "Customer is looking for Coretech."));
            m_QuickCodes.Add("WC", new QuickCode_t("WC", "Writing Center", "Customer is looking for the Writing Center."));
            m_QuickCodes.Add("RF", new QuickCode_t("RF", "Reference", "Customer is looking for a technical referral."));

            //Set the thread timers to the start-of-day and end-of-day times specified above
            m_StartTimer = new System.Windows.Threading.DispatcherTimer();
            m_StartTimer.Interval = Set_Timespan(m_StartTime);        //Timer to automatically start logging at 7:30am
            m_StartTimer.IsEnabled = true;
            m_StartTimer.Tick += new EventHandler(AutoStartLog);

            m_EndTimer = new System.Windows.Threading.DispatcherTimer();
            m_EndTimer.Interval = Set_Timespan(m_EndTime);            //Timer to automatically end logging at 5:30pm
            m_EndTimer.IsEnabled = true;
            m_EndTimer.Tick += new EventHandler(AutoEndLog);
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public static CougtechTicket CustomerTicket
        {
            get { return m_CustomerTicket; }
        }

        public static bool Logging_En
        {
            get { return m_TicketLogger.IsEnabled(); }
        }

        public static bool Ticketing_Email_En
        {
            get { return m_bTicketing_Email; }
            set { m_bTicketing_Email = value; }
        }

        public static bool Ticketing_Jira_En
        {
            get { return m_bTicketing_Jira; }
            set { m_bTicketing_Jira = value; }
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates a new instance of customerTicket for use with a new customer.
        /// </summary>
        /// <param name="sNid">WSU NID of the customer.</param>
        public static void CreateCustomerTicket(string sNid)
        {
            int n;

            if (int.TryParse(sNid, out n)) //If the NID string contains a number
            {
                //Create a normal ticket for walk-in customer
                string sFirstname = Wsu_Database.Get_FirstName(sNid);
                string sWsuEmail = Wsu_Database.Get_WsuEmail(sNid);

                m_CustomerTicket = new CougtechTicket(sNid, sFirstname, sWsuEmail);
            }
            else
                m_CustomerTicket = new CougtechTicket("000000000", "Butch", "Cougtech@wsu.edu", m_QuickCodes[sNid].f_sCode,  //Create a quick-pick ticket
                                                        m_QuickCodes[sNid].f_sDescription, false, true);

            //Turn off startup boolean
            m_bStartup = false;                
        }

        /// <summary>
        /// Sets the appointment flag within the current customer's ticket.
        /// </summary>
        /// <param name="bSet">Boolean value which designates what the flag will be set to.</param>
        /// <returns>True if this operation was successful. False if the ticket does not exist.</returns>
        public static bool Appointment_Set(bool bSet)
        {
            if (!m_bStartup && (m_CustomerTicket == null))
                return false;
            else if (m_CustomerTicket != null)
                m_CustomerTicket.IsAppointment = bSet;

            return true;
        }

        /// <summary>
        /// Sets the problem description within the current customer's ticket.
        /// </summary>
        /// <param name="sDescription"></param>
        /// <returns>True if this operation was successful. False if the ticket does not exist.</returns>
        public static bool Description_TextChanged(string sDescription)
        {
            if (!m_bStartup && (m_CustomerTicket == null))
                return false;
            else if(m_CustomerTicket != null)
                m_CustomerTicket.Description = sDescription;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool TicketLogging_Checked()
        {
            //Check to see if logging is enabled
            if (!Logging_En)
            {
                System.Windows.MessageBox.Show("Select a csv file to log to.", "Notice");

                OpenFileDialog fd = new OpenFileDialog {
                    Multiselect = false,
                    Filter = "CSV Files (*.csv)|*.csv"
                };

                DialogResult result = fd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)                 //If the user entered a log file to open
                {
                    m_TicketLogger.GenerateLogFile(fd.FileName, FileMode.Append);        //Generate log file with the provided path
                    return true;
                }
                else                                                                //Else
                {
                    //Inform the user that no file was selected
                    //Create a new log file in the default directory
                    var create = System.Windows.MessageBox.Show("No file selected. Create a new log?", "No Log File Selected", MessageBoxButton.YesNo);

                    if (create == MessageBoxResult.Yes)
                    {
                        StartLog(FileMode.Create);
                        return true;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("No file selected or created./nLogging has been disabled.", "Notice");
                        m_TicketLogger.CloseLog();
                        return false;
                    }
                }
            }
            else
            {
                m_TicketLogger.CloseLog();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool TicketSubmission_Email_Checked()
        {
            m_bTicketing_Email = !m_bTicketing_Email;
            return m_bTicketing_Email;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool TicketSubmission_Jira_Checked()
        {
            m_bTicketing_Jira = !m_bTicketing_Jira;
            return m_bTicketing_Jira;
        }

        /// <summary>
        /// Control logic to handle the input data from the Student ID page.
        /// </summary>
        /// <param name="sNid"></param>
        /// <returns>String containing an operation code which instructs the Student ID page to act accordingly.</returns>
        public static string Nid_TextChanged(string sNid)
        {
            bool bCorrectLength;
            bool bIsNum;
            QuickCode_t quickCode;

            //Check for quick-codes or admin access
            if (m_QuickCodes.TryGetValue(sNid, out quickCode))  //If sNid is a quick-code
                return "QP";                                        //Return valid quick-code string
            else if (sNid.ToLower() == "admin")                 //Else if sNid says "Admin"
                return "Admin";                                     //Return valid admin code string

            //If neither of the above contions are met, check for valid NID
            //Make sure we have a valid length, either 8 without a leading 0 or nine with a leading 0
            if ((sNid.Length == 8) || ((sNid.Length == 9) && (sNid[0] == '0'))) //If the string is of correct length
                bCorrectLength = true;                                              //Set the correct length flag to true
            else                                                                //Else
                bCorrectLength = false;                                             //Set the flag to false

            //Verify that the value has no alphabetic characters or punctuation
            int n;
            bIsNum = int.TryParse(sNid, out n);

            if (bIsNum && bCorrectLength)   //If the string is a valid NID
                return "WI";                    //Return valid walk-in string
            else                            //Else
                return null;                    //Return null
        }

        /// <summary>
        /// Sets the problem type within the current customer's ticket.
        /// </summary>
        /// <param name="sProblem"></param>
        /// <returns>True if this operation was successful. False if the ticket does not exist.</returns>
        public static bool Problem_SelectionChanged(string sProblem)
        {
            if (m_CustomerTicket == null)
                return false;

            m_CustomerTicket.Problem = sProblem;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static void SubmitTicket()
        {
            bool bSuccess = false;

            if (m_bTicketing_Email)
                bSuccess = SubmitTicket_Email();
            if (m_bTicketing_Jira)
                bSuccess = SubmitTicket_Rest();

            if (bSuccess)
            {
                if (m_bTicketing_Jira)
                {
                    string sMessage = $"Thank you!/nPlease take a seat and a technician will help you shortly./n/n{m_CustomerTicket.SelfUrl}";
                    MessageWindow msg = new MessageWindow(sMessage, 30);
                }
                else
                {
                    string sMessage = "Thank you!/nPlease take a seat and a technician will help you shortly./n/n";
                    MessageWindow msg = new MessageWindow(sMessage, 30);
                }
            }
            else
            {
                MessageWindow msg = new MessageWindow("A problem occured. Please try again.", 30);
            }
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Event handler for the end-of-day timer which ticks at the end of every Cougtech workday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AutoEndLog(object sender, EventArgs e)
        {
            m_EndTimer.IsEnabled = false; //Disable timer

            if (Logging_En)
            {
                //Check for weekday status
                if ((DateTime.Today.DayOfWeek != DayOfWeek.Saturday) && (DateTime.Today.DayOfWeek != DayOfWeek.Sunday))
                {
                    m_TicketLogger.CloseLog();  //Close logger file

                    //Display goodnight message if it is a weekday
                    MessageWindow mw = new MessageWindow("Good Night!/nLog file has been closed for " + DateTime.Now.ToString("MM/dd/yyyy"), 5.0);
                    mw.Show();
                }
            }

            //Update the end-of-day timer and enable it for the next day
            m_EndTimer.Interval = Set_Timespan(m_EndTime);
            m_EndTimer.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for the start-of-day timer which ticks at the beginning of every Cougtech workday.
        /// </summary>
        /// <remarks>
        /// Auto-generates a log file for the day given that it is a week day.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private static void AutoStartLog(object sender, EventArgs e)
        {
            m_StartTimer.IsEnabled = false; //Disable timer

            if (Logging_En)
            {
                //Generate the log file
                //Check for weekday status
                if ((DateTime.Today.DayOfWeek != DayOfWeek.Saturday) && (DateTime.Today.DayOfWeek != DayOfWeek.Sunday))
                {
                    //Begin logging for the day
                    StartLog(FileMode.Create);

                    //Display good morning message
                    MessageWindow mw = new MessageWindow("Good Morning!/nLog file has been created for " + DateTime.Now.ToString("MM/dd/yyyy"), 5.0);
                    mw.Show();
                }
            }

            //Update the start-of-day timer and enable for the next day
            m_StartTimer.Interval = Set_Timespan(m_StartTime);
            m_StartTimer.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime GetFirstDayofWeek(DateTime dt)
        {
            var diff = dt.DayOfWeek - DayOfWeek.Monday;

            if (diff < 0)
                diff += 7;

            return dt.AddDays(-diff).Date;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void PopulateRestRequest()
        {
            string sSummary;

            //Populate summary depending on the type of ticket
            if (m_CustomerTicket.IsQuickPick)
                sSummary = $"CTWI : {m_CustomerTicket.Problem}";
            else if (m_CustomerTicket.IsAppointment)
                sSummary = $"CTApt : {m_CustomerTicket.Problem} : {m_CustomerTicket.Nid} : {m_CustomerTicket.CustomerName}";
            else
                sSummary = $"CTWI : {m_CustomerTicket.Problem} : {m_CustomerTicket.Nid} : {m_CustomerTicket.CustomerName}";

            //Populate the REST request body with data
            m_RestClient.Request.Populate(sSummary, m_CustomerTicket.Description);
        }

        /// <summary>
        /// Determines the interval of time between now and the target time.
        /// </summary>
        /// <param name="targetTime">The end of the interval.</param>
        /// <returns>The time interval between now and the target time.</returns>
        private static TimeSpan Set_Timespan(TimeSpan targetTime)
        {
            //Create an instance of the target time for today
            DateTime dt = DateTime.Today.Add(targetTime);

            if (DateTime.Now > dt)  //If currently past the target time for today
                dt = dt.AddDays(1);     //Set it for tomorrow

            //Calculate the span of time between the target time and now
            //Return
            return dt.Subtract(DateTime.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        private static void StartLog(FileMode mode)
        {
            string sWeekOf;

            //Check for Monday status
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                //If it is monday, generate a new directory for this week's log files
                sWeekOf = "Week_of_" + DateTime.Now.ToString("MM-dd-yyyy");
                Directory.CreateDirectory(sWeekOf);
            }
            else
            {
                //Else, determine what the name of the directory for this week is
                DateTime monday = GetFirstDayofWeek(DateTime.Today);
                sWeekOf = "Week_of_" + monday.ToString("MM-dd-yyyy");
            }

            //Create log file for today
            m_TicketLogger.GenerateLogFile(sWeekOf + "//" + DateTime.Now.ToString("MM-dd-yyyy"), mode);
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool SubmitTicket_Email()
        {
            //New mail object
            MailMessage msg = new MailMessage();
            MailAddress mailFrom = new MailAddress("cougtech.walkin@wsu.edu" + "<" + CustomerTicket.CustomerEmail + ">");
            MailAddress mailTo = new MailAddress("cougtech.walkin@wsu.edu");

            msg.To.Add(mailTo);
            msg.From = mailFrom;

            SmtpClient client = new SmtpClient("mail.wsu.edu");


            if (CustomerTicket.IsQuickPick)         //Quickpick ticket
                msg.Subject = "##CTwi : " + CustomerTicket.Problem;
            else if (CustomerTicket.IsAppointment)  //Appointment ticket
                msg.Subject = "##CTapt : " + CustomerTicket.Problem + " : " + CustomerTicket.Nid + " : " + CustomerTicket.CustomerName;
            else                                //Standard walk-in ticket
                msg.Subject = "##CTwi : " + CustomerTicket.Problem + " : " + CustomerTicket.Nid + " : " + CustomerTicket.CustomerName;

            //Build the message body
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLongTimeString() + "\n" + CustomerTicket.Nid + "\n\n" + CustomerTicket.Problem + "\n\n" + CustomerTicket.Description);
            sb.AppendLine("\n\nVerify the Customer's information in the Reporter field.");
            sb.AppendLine("Assign to yourself, if not already assigned.");
            sb.AppendLine("Start work notes using Start Work.");
            sb.AppendLine("Finish with Close or Move.");
            msg.Body = sb.ToString();

            //Change the cursor
            //Cursor = Cursors.AppStarting;

            //Send the message
            try
            {
                client.Send(msg);
            }
            catch (Exception e)
            {
                return false;
            }

            //Change the cursor back
            //Cursor = Cursors.Arrow;

            //Return true for success
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool SubmitTicket_Rest()
        {
            //Compile REST API request body
            PopulateRestRequest();

            try
            {
                //Post request
                m_RestClient.PostRequest();

                //Grab the returned URL and display it
                m_CustomerTicket.SelfUrl = m_RestClient.Response_Url;
            }
            catch (Exception e)
            {
                return false;
            }

            //Return true for success
            return true;
        }
    };
}
