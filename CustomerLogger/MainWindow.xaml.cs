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

        private TimeSpan startTime = new TimeSpan(8, 0, 0); // 8:00 am (24 hour clock)
        private TimeSpan endTime = new TimeSpan(17, 0, 0); // 5:00 pm
        private System.Windows.Threading.DispatcherTimer startDayTimer, endDayTimer; // fires an event to start/end the log for the day

        public MainWindow() {
            InitializeComponent();
            _writer = null;
            _number_records = 0;
            _student_id_page = new StudentIDPage(this);
            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
            ContentFrame.Navigate(_student_id_page);
            _log_path = Directory.GetCurrentDirectory();

            startDayTimer = new System.Windows.Threading.DispatcherTimer();
            //startDayTimer.Interval = startTime;
            startDayTimer.IsEnabled = true;
            //startDayTimer.Tick += handlerhere()

            endDayTimer = new System.Windows.Threading.DispatcherTimer();
            //endDayTimer.Interval = endTime;
            endDayTimer.IsEnabled = true;
            //endDayTimer.Tick += handlerhere()
        }

        public string LogPath {
            set { _log_path = value; }
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
            //startDayTimer.Interval = updatedtime

            startDayTimer.IsEnabled = true;
        }

        private void AutoEndLog(object sender, EventArgs e)
        {
            endDayTimer.IsEnabled = false; // stop timer

            // end the log and display message
            //endDayTimer.Interval = updatedtime

            endDayTimer.IsEnabled = true;
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
            AdminWindow aw = new AdminWindow(this);

            aw.Show();
        }

        public void changePage(Page page) {
            ContentFrame.Navigate(page);
        }

        public void Reset() {
            _student_id_page = new StudentIDPage(this);
            changePage(_student_id_page);
            
            //clear navigation history
            var entry = ContentFrame.NavigationService.RemoveBackEntry();
            while(entry != null) {

                entry = ContentFrame.NavigationService.RemoveBackEntry();
            }

            _device_page = new DevicePage(this);
            _problem_page = new ProblemPage(this);
            _summary_page = new SummaryPage(this);
        }

        //TODO:
        //Make sure valid ID Numbers only

        //TODO:
        //Find way to truly clear all of history

        //TODO:
        //Find Automation for start and end of days

        //TODO:
        //Make Pretty
    }
}
