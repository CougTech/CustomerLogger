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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Mail;
using System.Net;
using CSV;
using System.ComponentModel;

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow:Window {
        private CSVWriter _writer;
        private StudentIDPage _student_id_page;
        private DevicePage _device_page;
        private ProblemPage _problem_page;
        private SummaryPage _summary_page;
        private int _number_records;
        private string _log_path;
        private bool _logging, _email_logging;
        private string _email_pwd;

        private TimeSpan startTime = new TimeSpan(8, 0, 0); // 8:00 am (24 hour clock)
        private TimeSpan endTime = new TimeSpan(17, 0, 0); // 5:00 pm
        private System.Windows.Threading.DispatcherTimer startDayTimer, endDayTimer;


        //runs everything.
        public MainWindow() {
            InitializeComponent();
            _writer = null;
            _number_records = 0;
            //The way this works is there is one window and we have different
            //pages that we flip between. Each page is responcible for getting
            //a piece of info from the customer. Each page takes a reference to the
            //main window so if can modify the necessary data fields
            
            //so.... this is probably a terrible design
            //I am pretty sure the little windows are just supposed
            //to pass info from themselves to the main window
            //and not access the actual data in the main window.
            //so this is kinda a terrible design, like it is bad.
            //we should probably fix this
            //TODO: fix this design
            //I guess to be fair I should say I made this decision
            //to use this sub-par design because I was just trying
            //to get something to work. And this was much faster to code
            //than figure out the proper events and all that... I am a backend
            //programmer and UI's are dumb.... Anyway bad design but at least
            //you know why I did it.
            //But still TODO: fix this design
            
            //here is the page objects
            _student_id_page = new StudentIDPage(this);
            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
            
            //state variables
            _logging = false;
            _email_logging = false;
            _email_pwd = "";
            ContentFrame.Navigate(_student_id_page);
            _log_path = GetDefaultDirectory();

            //sets up timers to go at particular times
            //start of cougtech and end of cougtech working
            startDayTimer = new System.Windows.Threading.DispatcherTimer(); // Timer to automatically start logging at 8:00 am
            startDayTimer.Interval = TimeUntilNextTimer(startTime);
            startDayTimer.IsEnabled = true;
            startDayTimer.Tick += new EventHandler(AutoStartLog);

            endDayTimer = new System.Windows.Threading.DispatcherTimer(); // Timer to automatically end logging at 5:00 pm
            endDayTimer.Interval = TimeUntilNextTimer(endTime);
            endDayTimer.IsEnabled = true;
            endDayTimer.Tick += new EventHandler(AutoEndLog);

            ContentFrame.Navigated += ContentFrame_Navigated;
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

        public DevicePage DevicePage{
            get {return _device_page; }
        }

        public ProblemPage ProblemPage {
            get {return _problem_page; }
        }

        public SummaryPage SummaryPage {
            get {return _summary_page; }
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
        
        //remove histroy when done, so we don't have to worry about customer going back and seeing
        //other student ID's but also so we don't get duplicates of the same customer
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _student_id_page) { // Will clear all back history once Student ID Page has finished loading
                removeBackHistory();
            }
        }

        //starts a .csv log for the day
        //it will be given the name passed in .csv
        public void StartLog(string name) {

            _logging = true;
            _email_logging = true;
            _number_records = 0;
            //create a new writer
            try
            {
                _writer = new CSVWriter(_log_path + "\\" + name + ".csv");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Error Starting Log");
                return;
            }
            //basic header stuff for the csv
            //the name of the log, the time it started
            _writer.addToCurrent("Log for: ");
            _writer.addToCurrent(name);
            _writer.addToCurrent(" ");
            _writer.addToCurrent("Log Start Time: ");
            _writer.addToCurrent(DateTime.Now.ToShortTimeString());
            _writer.WriteLine();
            //and then header for the collumns
            _writer.addToCurrent("Time");
            _writer.addToCurrent("ID Number");
            _writer.addToCurrent("Device");
            _writer.addToCurrent("Problem");
            _writer.WriteLine();
             //woot woot for making human readable outp
        }

        //automatically starts the day when the timer is hit
        private void AutoStartLog(object sender, EventArgs e) {
            startDayTimer.IsEnabled = false; // stop timer

            // start the log and display message
            //we make the name of the log being the date in the MM-dd-yyyy format
            if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday) { // don't start logs on saturday/sunday
                StartLog(DateTime.Now.ToString("MM-dd-yyyy"));
                MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
                mw.Show();
            }

            startDayTimer.Interval = TimeUntilNextTimer(startTime); // update time to next day at 8:00 am
            startDayTimer.IsEnabled = true;
        }

        //automatically ends the log
        private void AutoEndLog(object sender, EventArgs e) {
            endDayTimer.IsEnabled = false; // stop timer

            // end the log and display message
            if (null != _writer) {
                EndLog();
                MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
                mw.Show();
            }

            endDayTimer.Interval = TimeUntilNextTimer(endTime); // update time to next day at 5:00 pm
            endDayTimer.IsEnabled = true;
        }

        //resets the timer to go for the next day
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

        //write the line for the end of the customer log
        //delagtes to the internal CSV writer object
        public void writeLine() {

            if(null != _writer) {

                _writer.addToStart(DateTime.Now.ToString("h:mm"));
                _writer.WriteLine();
                _number_records++;

            }
            else {
                if (true == _logging) {
                    MessageBox.Show("Trying to write to file but the CSV writer is set to null.", "Error Writing Record"); // Show an error message if tried to write a record but there is no CSV writer
                }
            }
        }

        //close the file (log for the day)
        //this includes tallying up the customers for the day and writing it
        //then putting the log end time and writting it
        //destroying the writer and then resetting state variables
        public void EndLog() {

            _writer.addToCurrent("Customers for today: ");
            _writer.addToCurrent(_number_records.ToString());
            _writer.WriteLine();
            _writer.addToCurrent("Log End Time: ");
            _writer.addToCurrent(DateTime.Now.ToShortTimeString());
            _writer.WriteLine();
            //close and dealocate the csv writer
            _writer = null;

            _logging = false;
            _email_logging = false;
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

        //allows us to move from one page to the next via our current page... because of my bad design
        public void changePage(Page page) {
            ContentFrame.Navigate(page);
        }

        //resets all pages and state related to signing in customer
        //usefull when someone has submitted and we do not want to keep
        //old submissions floating arround in memory.
        public void Reset() {
            _student_id_page = new StudentIDPage(this);
            changePage(_student_id_page);

            //clear navigation history
            removeBackHistory();

            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
        }

        //make sure that if the window closes for any reason (possibly other than a crash
        //this may work in a crash, but we haven't tested that, also we are planning on this not crashing)
        //we end the log, just so we have less chance of corrupted logs
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (true == _logging) {
                EndLog(); // Will finish writing the log if the program got closed with Alt+F4 (not sure if it works on crashes..)
            }
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
        public int SendTicket() {
            
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("cougtech.helpdesk@gmail.com", _email_pwd);

            MailAddress sender = new MailAddress("cougtech.helpdesk@gmail.com");
            MailAddress receiver = new MailAddress("cougtech@wsu.edu");

            MailMessage msg = new MailMessage(sender, receiver);
            msg.Subject = "##Cougtech Walk-in " + ProblemPage.Problem + ": " + StudentIDPage.StudentID;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToLongTimeString() + "\n" + StudentIDPage.StudentID + "\n" + DevicePage.Device + "\n" + ProblemPage.Problem);
            sb.AppendLine("\n[*] Please change the customer information for this ticket by selecting the | Customer | button above and enter their ID number in the Customer User field.");
            sb.AppendLine("[*] Add your notes by selecting the | Note | button above.");
            sb.AppendLine("[*] Close the ticket when you are done by selecting the | Close | button above.");
            msg.Body = sb.ToString();

            Cursor = Cursors.AppStarting;
            try {
                client.Send(msg);
            }
            catch (Exception e) {
                System.Windows.MessageBox.Show("Could not send OTRS ticket. Please let a CougTech employee know of this problem. Thank you.", "Failed To Send");
                Cursor = Cursors.Arrow;
                return -1; // -1 for error
            }
            Cursor = Cursors.Arrow;
            return 0; // 0 for success
        }
    }
}
