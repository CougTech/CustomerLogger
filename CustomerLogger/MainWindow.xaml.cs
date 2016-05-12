using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.IO;
using System.Net.Mail;
using System.Net;
using CSV;
using System.ComponentModel;
using CustomerLogger.Pages;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow:Window {
        private CSVWriter _writer;
        private StudentIDPage _student_id_page;
        private AppointmentPage _appt_page;
        private AppointmentProblemPage _appt_prob_page;
        private ProblemPage _problem_page;
        private SummaryPage _summary_page;
        private string _log_path;
        private bool _logging, _email_logging;
        private string _email_pwd;

        private TimeSpan startTime = new TimeSpan(7, 30, 0); // 7:30 am (24 hour clock)
        private TimeSpan endTime = new TimeSpan(17, 30, 0); // 5:30 pm
        private System.Windows.Threading.DispatcherTimer startDayTimer, endDayTimer;


        //runs everything.
        public MainWindow() {
            InitializeComponent();
            _writer = null;

            //state variables
            _logging = false;
            _email_logging = false;
            _email_pwd = "";
            _log_path = GetDefaultDirectory();

            //create the page objects
            CreatePages();
            ContentFrame.Navigate(_student_id_page);

            //sets up timers to go at particular times
            //start of cougtech and end of cougtech working hours
            startDayTimer = new System.Windows.Threading.DispatcherTimer(); // Timer to automatically start logging at 8:00 am
            startDayTimer.Interval = TimeUntilNextTimer(startTime);
            startDayTimer.IsEnabled = true;
            startDayTimer.Tick += new EventHandler(AutoStartLog);

            endDayTimer = new System.Windows.Threading.DispatcherTimer(); // Timer to automatically end logging at 5:00 pm
            endDayTimer.Interval = TimeUntilNextTimer(endTime);
            endDayTimer.IsEnabled = true;
            endDayTimer.Tick += new EventHandler(AutoEndLog);

            ContentFrame.Navigated += ContentFrame_Navigated;
            ContentFrame.Navigating += ContentFrame_Navigating;

        }

        //holds the path to where csv logs are saved
        public string LogPath {
            set { _log_path = value; }
            get { return _log_path;}
        }

        //saves password used for emailing otrs
        public string EmailPassword {
            set { _email_pwd = value; }
            get { return _email_pwd; }
        }

        public StudentIDPage StudentIDPage{
            get {return _student_id_page; }
        }

        public AppointmentProblemPage AppointmentProbPage {
            get { return _appt_prob_page; }
        }

        public ProblemPage ProblemPage {
            get {return _problem_page; }
        }

        public SummaryPage SummaryPage {
            get {return _summary_page; }
        }

        public AppointmentPage AppointmentPage
        {
            get { return _appt_page; }
        }

        public bool Logging {
            get {return _logging; }
            set {_logging = value; }
        }

        public bool EmailLogging
        {
            get { return _email_logging; }
            set { _email_logging = value; }
        }


        // reads in a file to set the default directory
        private string GetDefaultDirectory()
        {
            try {
                using (FileStream fs = new FileStream("defaultdir.cfg", FileMode.Open)) {
                    using (StreamReader sr = new StreamReader(fs)) {
                        return sr.ReadLine(); // directory should be first and only line in file
                    }
                }
            }
            catch (Exception e) {
                return Directory.GetCurrentDirectory(); // return current directory if failed to read file
            }
        }

        private void CreatePages()
        {
            _student_id_page = new StudentIDPage();
            _student_id_page.PageFinished += _page_PageFinished;

            _appt_page = new AppointmentPage();
            _appt_page.PageFinished += _page_PageFinished;

            _appt_prob_page = new AppointmentProblemPage();
            _appt_prob_page.PageFinished += _page_PageFinished;
            
            _problem_page = new ProblemPage();
            _problem_page.PageFinished += _page_PageFinished;

            _summary_page = new SummaryPage();
            _summary_page.PageFinished += _summary_page_PageFinished;
        }

        /// <summary>
        /// Changes page when user clicks 'next' on any of the pages except SummaryPage
        /// </summary>
        /// <param name="sender"> not used </param>
        /// <param name="e"> not used </param>
        private void _page_PageFinished(object sender, EventArgs e)
        {
            if (ContentFrame.Content == StudentIDPage)
            {
                ContentFrame.Navigate(AppointmentPage); 
            }
            else if (ContentFrame.Content == AppointmentPage)
            {
                if (AppointmentPage.HasAppointment == true) // @ appointment page
                {
                    
                    ContentFrame.Navigate(AppointmentProbPage);
                }
                else
                {
                    ContentFrame.Navigate(ProblemPage); 
                }
            }
            else if (ContentFrame.Content == ProblemPage)
            {
                SummaryPage.SetText(StudentIDPage.StudentID, ProblemPage.Problem, ProblemPage.Description);
                SummaryPage.StartTimer(); // starts a 10 sec timer to auto close summary page
                ContentFrame.Navigate(SummaryPage); // @ problem page, go to summary next
            }
            else if (ContentFrame.Content == AppointmentProbPage) 
            {
                SendAppointment(_appt_prob_page.Problem); // send in ticket and reset if user has one
            }
        }

        // User submits their info and it gets written to the csv file and sent to OTRS if enabled
        private void _summary_page_PageFinished(object sender, EventArgs e)
        {

            /// Removed rental and Device
            if (EmailLogging == true)
            { // don't send tickets that are rentals (ramsay creates one)
                int result = SendTicket(StudentIDPage.StudentID, ProblemPage.Problem, ProblemPage.Description, false); // send in otrs ticket 
                if (result < 0)
                {
                    return; // Don't write to file if attempt to send emails.. this will prevent duplicates and keep the summary page open
                }
            }

            if (Logging == true && _writer != null)
            {
                //write to csv file
                addToCurrent(StudentIDPage.StudentID);
                addToCurrent(ProblemPage.Problem);
                addToCurrent(ProblemPage.Description);
                writeLine();
            }

            //let customer know they can sit down
            MessageWindow mw = new MessageWindow("Thank you, please take a seat at a table and someone will help you shortly.", 4.0);
            mw.ShowDialog();

            //get back to first state
            Reset();
        }

        //remove histroy when done, so we don't have to worry about customer going back and seeing
        //other student ID's but also so we don't get duplicates of the same customer
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _student_id_page) { // Will clear all back history once Student ID Page has finished loading
                removeBackHistory();
            }
        }

        // stops the timer on the summary page if the user decides to go back for some reason
        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                SummaryPage.StopTimer();
            }
        }

        public bool IsCsvNull()
        {
            return (_writer == null);
        }

        /// <summary>
        /// enable logging and email logging for writing to the csv file and send tickets to otrs
        /// </summary>
        public void StartLog() {
            _logging = true;
            _email_logging = true;
        }

        /// <summary>
        /// Disables logging to csv and emailing tickets
        /// </summary>
        public void EndLog()
        {
            _logging = false;
            _email_logging = false;
        }

        /// <summary>
        /// Creates a new CSVWriter object which will write to a new file
        /// This is performed weekly every monday or if a file does not exist for the week
        /// </summary>
        /// <param name="file_name">name of csv file</param>
        public void CreateLog(string file_name, FileMode mode) {
            //create a new writer
            try
            {
                if (mode == FileMode.Create)
                {
                    int i = 0;
                    string fname = _log_path + "\\" + file_name + "-" + i.ToString() + ".csv";
                    while (File.Exists(fname)) // this loop guarantees that it will not overwrite the file, but instead append a number to the end and create a new file
                    {
                        i += 1;
                        fname = _log_path + "\\" + file_name + "-" + i.ToString() + ".csv";
                    }
                    _writer = new CSVWriter(fname, mode);

                    //basic header stuff for the csv
                    //the name of the log, the time it started
                    _writer.addToCurrent("Log for: ");
                    _writer.addToCurrent(file_name);
                    _writer.addToCurrent(" ");
                    _writer.addToCurrent("Log Start Time: ");
                    _writer.addToCurrent(DateTime.Now.ToShortTimeString());
                    _writer.WriteLine();
                    //and then header for the collumns
                    _writer.addToCurrent("Time");
                    _writer.addToCurrent("ID Number");
                    _writer.addToCurrent("Problem");
                    _writer.addToCurrent("Description");
                    _writer.WriteLine();
                }
                else if (mode == FileMode.Append)
                {
                    _writer = new CSVWriter(file_name, mode);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Creating Log");
                return;
            }
        }

        /// <summary>
        /// Writes time log finished and deallocates the CSVWriter
        /// </summary>
        public void FinishLog()
        {
            _writer.addToCurrent("Log End Time: ");
            _writer.addToCurrent(DateTime.Now.ToShortTimeString());
            _writer.WriteLine();
            //close and dealocate the csv writer
            _writer = null;
        }

        //automatically starts the day when the timer is hit
        private void AutoStartLog(object sender, EventArgs e) {
            startDayTimer.IsEnabled = false; // stop timer

            // start the log and display message
            // if it is Monday then we create a new log for the week.
            //we make the name of the log being the date in the MM-dd-yyyy format
            if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday) { // don't start logs on saturday/sunday
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    CreateLog("Week_of_" + DateTime.Now.ToString("MM-dd-yyyy"), FileMode.Create); // creates a new .csv file for the week
                }

                StartLog(); // enable Logging & EmailLogging
                MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
                mw.Show();
            }

            startDayTimer.Interval = TimeUntilNextTimer(startTime); // update time to next day at 7:30 am
            startDayTimer.IsEnabled = true;
        }

        //automatically ends the log
        private void AutoEndLog(object sender, EventArgs e) {
            endDayTimer.IsEnabled = false; // stop timer

            // end the log and display message
            if (null != _writer) {
                EndLog();

                if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    FinishLog(); // write final stats to weekly file if end of day on Friday
                }

                MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
                mw.Show();
            }

            endDayTimer.Interval = TimeUntilNextTimer(endTime); // update time to next day at 5:00 pm
            endDayTimer.IsEnabled = true;
        }

        //get the time for the next day
        private TimeSpan TimeUntilNextTimer(TimeSpan target_time) {
            DateTime dt = DateTime.Today.Add(target_time);

            if (DateTime.Now > dt) { // if past the target time then set it for the next day
                dt = dt.AddDays(1);
            }

            return dt.Subtract(DateTime.Now);
        }

        //add to text to current line for customer log
        //delagtes to the internal CSV writer object
        public void addToCurrent(string text) {

            if(null != _writer) { 
            
                _writer.addToCurrent(text);
            }
        }

        /// <summary>
        /// writes customer details to csv file
        /// </summary>
        public void writeLine() {

            if(null != _writer) {
                _writer.addToStart(DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                _writer.WriteLine();
            }
            else {
                if (true == _logging) {
                    MessageBox.Show("Trying to write to file but the CSV writer is set to null.", "Error Writing Record"); // Show an error message if tried to write a record but there is no CSV writer
                }
            }
        }

        //opens the admin button, after the user successfully puts in our secret password
        private void AdminButton_Click(object sender, RoutedEventArgs e) {

            PasswordWindow ap = new PasswordWindow();
            ap.ShowDialog();

            //This is also terrible, but easy to use... but still, we should move this out to a file somewhere
            //because this is dumb
            //and bad, and should be fixed if we want to do this correctly
            //woot woot for hardcoded passwords....
            if (ap.Password == "couglolz") { // open admin window only if password is correct
                AdminWindow aw = new AdminWindow(this);
                aw.ShowDialog(); // keeps admin window on top of all other windows, preventing multiple admin windows from opening
            }
        }

        //resets all pages and state related to signing in customer
        //usefull when someone has submitted and we do not want to keep
        //old submissions floating arround in memory.
        public void Reset() {
            CreatePages(); // recreate pages to clear out data
            ContentFrame.Navigate(StudentIDPage);
        }

        //remove page history so customer can't go back after the final submission
        //this way we don't get duplicates and people need to sign up
        private void removeBackHistory() {
            var entry = ContentFrame.NavigationService.RemoveBackEntry();
            while (entry != null)
            {
                entry = ContentFrame.NavigationService.RemoveBackEntry();
            }
        }

        //sends ticket to orts via email
        //this way we can make notes and all that good otrs stuff....
        //this is the code that actually takes our customer logger info and sends it to otrs
        public int SendTicket(string id, string prob, string descr, bool isAppt) {
            
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("cougtech.helpdesk@gmail.com", _email_pwd);

            MailAddress sender = new MailAddress("cougtech.helpdesk@gmail.com");
            MailAddress receiver = new MailAddress("cougtech@wsu.edu");

            MailMessage msg = new MailMessage(sender, receiver);

            // If the ticket is an appointment then changes the subject
            if (StudentIDPage.isTest)
            {
                msg.Subject = "##CTtest : " + prob + " : " + id;
            }
            else if (isAppt)
            {
                msg.Subject = "##CTapt : " + prob + " : " + id;
            }
            else
            {
                msg.Subject = "##CTwi : " + prob + " : " + id;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLongTimeString() + "\n" + id + "\n" + prob + "\n\n" + descr);
            sb.AppendLine("\n\n[*] Please change the customer information for this ticket by selecting the | Customer | button above and enter their ID number in the Customer User field.");
            sb.AppendLine("[*] Add your notes by selecting the | Note | button above.");
            sb.AppendLine("[*] Close the ticket when you are done by selecting the | Close | button above.");
            msg.Body = sb.ToString();

            Cursor = Cursors.AppStarting;
            try {
                client.Send(msg);
            }
            catch (Exception e) {
                MessageBox.Show("Could not send OTRS ticket. Email password may be incorrect. Please let a CougTech employee know of this problem. Thank you.", "Failed To Send");
                Cursor = Cursors.Arrow;
                return -1; // -1 for error
            }
            Cursor = Cursors.Arrow;
            return 0; // 0 for success
        }

        // this will create a ticket specifically for appointments and will skip the rest of the questions
        // also writes to csv file if logging is enabled
        private void SendAppointment(string prob)
        {
            if (EmailLogging == true)
            {
                int result = SendTicket(StudentIDPage.StudentID, prob, prob, true); // send in otrs ticket 
                if (result < 0)
                {
                    return; // Don't write to file if attempt to send emails.. this will prevent duplicates and keep the summary page open
                }
            }

            if (Logging == true && _writer != null)
            {
                //write to csv file
                addToCurrent(StudentIDPage.StudentID);
                addToCurrent("Appointment");
                addToCurrent(AppointmentProbPage.Problem);
                writeLine();
            }

            //let customer know they can sit down
            MessageWindow mw = new MessageWindow("Thank you, please take a seat at a table and someone will help you shortly.", 4.0);
            mw.ShowDialog();

            //get back to first state
            Reset();
        }
    }
}
