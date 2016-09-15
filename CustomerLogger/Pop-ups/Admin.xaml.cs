using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>


    //allows for admin controls of the program.
    //This includes starting/stopping logging, changing directories were things are saved and other usefull things
    public partial class AdminWindow:Window {

        MainWindow _main_window;
        System.Windows.Forms.FolderBrowserDialog _folder_dialog;
          
        public AdminWindow(MainWindow main_window) {
            InitializeComponent();
            _main_window = main_window;
            //displaying the current LogPath
            //DirectoryTextBlock.Text = _main_window.LogPath;
            //display if we are currently logging
            if(_main_window.Logging) { 
            
                LogGoingTextBlock.Text = "Logging: Yes";
            } else {
                LogGoingTextBlock.Text = "Logging: No";
            }

            //display if emails are being sent when a customer logs in
            //this is how we get the tickets into ORTS
            if (_main_window.EmailLogging) {
                emailSendTextBlock.Text = "Emails Sending: Yes";
                emailButton.Content = "Disable Emails";
            }
            else {
                emailSendTextBlock.Text = "Emails Sending: No";
            }

            _folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.Activate();
        }

        // starting the day includes starting logging
        // if there is no csv writer then you can select or create one
        // it will also prompt you if you want to enable emails for OTRS
        private void LoggingButton_Click(object sender, RoutedEventArgs e) {

            // If logging is NOT on
            if (!_main_window.Logging)
            {

                // create a csv writer if it does not exist. when you have to restart the program or it closes
                // can select the already created log and continue writing to it for the week
                if (_main_window.IsCsvNull())
                {
                    System.Windows.MessageBox.Show("Select a csv file to log to.", "Notice");
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Multiselect = false;
                    fd.Filter = "CSV Files (*.csv)|*.csv";
                    DialogResult res = fd.ShowDialog();
                    if (System.Windows.Forms.DialogResult.OK == res)
                    {
                        _main_window.CreateLog(fd.FileName, FileMode.Append);
                        _main_window.LogPath = Path.GetDirectoryName(fd.FileName); // update logging directory
                        //DirectoryTextBlock.Text = _main_window.LogPath;
                    }
                    else // user did not select a file to log to
                    {
                        var create = System.Windows.MessageBox.Show("No file selected. Create a new log?", "No Log Selected", MessageBoxButton.YesNo);
                        if (create == MessageBoxResult.Yes)
                        { // create a log
                            _main_window.CreateLog("Week_of_" + DateTime.Now.ToString("MM-dd-yyyy"), FileMode.Create);
                        }
                        else
                        { // if user says no to creating a log then don't start logging
                            System.Windows.MessageBox.Show("No file selected or created. Not starting log", "Notice");
                            return;
                        }
                    }
                }

                _main_window.StartLog(); //! this defaults to setting Logging to true and EmailLogging to true

                // ask if want to enable emails
                //var result = System.Windows.Forms.MessageBox.Show("Would you like to enable emails?", "Email", MessageBoxButtons.YesNo);
                //if (result == System.Windows.Forms.DialogResult.Yes)
                //{
                //    _main_window.EmailLogging = false; // set to false before calling next function so it can execute correctly and enable EmailLogging
                //}
                //emailButton_Click(new object(), new RoutedEventArgs()); //!! this will set main windows EmailLogging accordingly

                //MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
               // mw.Show();
                LogGoingTextBlock.Text = "Logging: Yes";
                LoggingButton.Content = "Disable Logging";
            }

            // Logging IS already on
            else {
                _main_window.EndLog();
                LogGoingTextBlock.Text = "Logging: No";
                LoggingButton.Content = "Enable Logging";

            }
        }

        //end the day involves ending the log (writting the totals and then closing the file) and then turning off logging
        //private void EndDayButton_Click(object sender, RoutedEventArgs e) {
            
        //    if (false == _main_window.Logging) {
        //        System.Windows.MessageBox.Show("The day has not been started!", "Error"); // avoid being able to end a day if it is not started
        //        return;
        //    }
            
        //    _main_window.EndLog();
        //    MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
        //    mw.Show();
        //    LogGoingTextBlock.Text = "Logging: No";
        //}

        //changes the directory logs get saved to via the folder dialog
        //private void LogDirectoryButton_Click(object sender, RoutedEventArgs e) {
        //    System.Windows.Forms.DialogResult result = _folder_dialog.ShowDialog();
        //    if(System.Windows.Forms.DialogResult.OK == result) {
        //        _main_window.LogPath = _folder_dialog.SelectedPath.ToString();
        //    }
        //    //DirectoryTextBlock.Text = _main_window.LogPath;
        //}

        //stops the program. Like all of it. Like it will shut down the program
        //don't click this unless you want the program to not be running any more.
        //I mean it
        //it will not be running after you click this button
        //you will need to restart it
        private void StopButton_Click(object sender, RoutedEventArgs e) {
            _main_window.Close();
            this.Close();
        }


        //handles the emailing part of admin
        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            if (_main_window.EmailLogging == false)
            {
                _main_window.EmailLogging = true;
                emailSendTextBlock.Text = "Emails Sending: Yes";
                emailButton.Content = "Disable Emails";
                
            }
            else
            {
               
                _main_window.EmailLogging = false;
                emailSendTextBlock.Text = "Emails Sending: No";
                emailButton.Content = "Enable Emails";
            }
        }
    }
}
