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
        private bool _logging;

        private TimeSpan startTime = new TimeSpan(8, 0, 0); // 8:00 am (24 hour clock)
        private TimeSpan endTime = new TimeSpan(17, 0, 0); // 5:00 pm
        private System.Windows.Threading.DispatcherTimer startDayTimer, endDayTimer;

        public MainWindow() {
            InitializeComponent();
            _writer = null;
            _number_records = 0;
            _student_id_page = new StudentIDPage(this);
            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
            _logging = false;
            ContentFrame.Navigate(_student_id_page);
            _log_path = Directory.GetCurrentDirectory();

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

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _student_id_page) { // Will clear all back history once Student ID Page has finished loading
                removeBackHistory();
            }
        }

        public string LogPath {
            set { _log_path = value; }
            get { return _log_path;}
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

        public void StartLog(string name) {

            //create a new writer
            _writer = new CSVWriter(_log_path + "\\" + name + ".csv");
            _writer.addToCurrent("Log for: ");
            _writer.addToCurrent(name);
            _writer.WriteLine();
            _writer.addToCurrent("Time");
            _writer.addToCurrent("ID Number");
            _writer.addToCurrent("Device");
            _writer.addToCurrent("Problem");
            _writer.WriteLine();
        }

        private void AutoStartLog(object sender, EventArgs e) {
            startDayTimer.IsEnabled = false; // stop timer

            // start the log and display message
            StartLog(DateTime.Now.ToString("MM-dd-yyyy"));
            MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
            mw.Show();

            startDayTimer.Interval = TimeUntilNextTimer(startTime); // update time to next day at 8:00 am
            startDayTimer.IsEnabled = true;
        }

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

        private TimeSpan TimeUntilNextTimer(TimeSpan target_time) {
            DateTime dt = DateTime.Today.Add(target_time);

            if (DateTime.Now > dt) { // if past the target time then set it for the next day
                dt = dt.AddDays(1);
            }

            return dt.Subtract(DateTime.Now);
        }

        //add to text to current line for customer log
        public void addToCurrent(string text) {

            if(null != _writer) { 
            
                _writer.addToCurrent(text);
            }
        }

        //write the line for the end of the customer log
        public void writeLine() {

            if(null != _writer) { 
            
                _writer.addToStart(DateTime.Now.ToString("h:mm"));
                _writer.WriteLine();
                _number_records++;
            }
        }

        //close the file (log for the day)
        public void EndLog() {

            _writer.addToCurrent("Customers for today: ");
            _writer.addToCurrent(_number_records.ToString());
            _writer.WriteLine();
            _number_records = 0;
            //close and dealocate the csv writer
            _writer = null;
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e) {

            AdminPassword ap = new AdminPassword();
            ap.ShowDialog();

            if (ap.IsCorrect) { // open admin window only if password is correct
                AdminWindow aw = new AdminWindow(this);
                aw.ShowDialog(); // keeps admin window on top of all other windows, preventing multiple admin windows from opening
            }
        }

        public void changePage(Page page) {
            ContentFrame.Navigate(page);
        }

        public void Reset() {
            _student_id_page = new StudentIDPage(this);
            changePage(_student_id_page);

            //clear navigation history
            removeBackHistory();

            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
        }

        private void removeBackHistory() {
            var entry = ContentFrame.NavigationService.RemoveBackEntry();
            while (entry != null)
            {
                entry = ContentFrame.NavigationService.RemoveBackEntry();
            }
        }

        //TODO:
        //Make sure valid ID Numbers only

        //TODO:
        //Find way to truly clear all of history

        //TODO:
        //Make Pretty
    }
}
