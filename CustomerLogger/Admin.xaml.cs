//        CCCCCCCCCCCCC     OOOOOOOOO      UUUUUUUU     UUUUUUUU       GGGGGGGGGGGGG       
//     CCC::::::::::::C   OO:::::::::OO    U::::::U     U::::::U    GGG::::::::::::G       
//   CC:::::::::::::::C OO:::::::::::::OO  U::::::U     U::::::U  GG:::::::::::::::G       
//  C:::::CCCCCCCC::::C O:::::::OOO:::::::O UU:::::U     U:::::UU G:::::GGGGGGGG::::G       
// C:::::C       CCCCCC O::::::O   O::::::O  U:::::U     U:::::U  G:::::G       GGGGGG       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G                     
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G                     
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    GGGGGGGGGG       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    G::::::::G       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    GGGGG::::G       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G        G::::G       
// C:::::C       CCCCCC O::::::O   O::::::O  U::::::U   U::::::U G:::::G       G::::G       
//  C:::::CCCCCCCC::::C O:::::::OOO:::::::O  U:::::::UUU:::::::U  G:::::GGGGGGGG::::G       
//   CC:::::::::::::::C  OO:::::::::::::OO    UU:::::::::::::UU    GG:::::::::::::::G       
//     CCC::::::::::::C    OO:::::::::OO        UU:::::::::UU        GGG::::::GGG:::G       
//        CCCCCCCCCCCCC      OOOOOOOOO            UUUUUUUUU             GGGGGG   GGGG
//
//TTTTTTTTTTTTTTTTTTTTTTT EEEEEEEEEEEEEEEEEEEEEE       CCCCCCCCCCCCC HHHHHHHHH     HHHHHHHHH
//T:::::::::::::::::::::T E::::::::::::::::::::E    CCC::::::::::::C H:::::::H     H:::::::H
//T:::::::::::::::::::::T E::::::::::::::::::::E  CC:::::::::::::::C H:::::::H     H:::::::H
//T:::::TT:::::::TT:::::T EE::::::EEEEEEEEE::::E C:::::CCCCCCCC::::C HH::::::H     H::::::HH
//TTTTTT  T:::::T  TTTTTT   E:::::E       EEEEEE C:::::C       CCCCCC   H:::::H     H:::::H  
//        T:::::T           E:::::E             C:::::C                 H:::::H     H:::::H  
//        T:::::T           E::::::EEEEEEEEEE   C:::::C                 H::::::HHHHH::::::H  
//        T:::::T           E:::::::::::::::E   C:::::C                 H:::::::::::::::::H  
//        T:::::T           E:::::::::::::::E   C:::::C                 H:::::::::::::::::H  
//        T:::::T           E::::::EEEEEEEEEE   C:::::C                 H::::::HHHHH::::::H  
//        T:::::T           E:::::E             C:::::C                 H:::::H     H:::::H  
//        T:::::T           E:::::E       EEEEEE C:::::C       CCCCCC   H:::::H     H:::::H  
//      TT:::::::TT       EE::::::EEEEEEEE:::::E  C:::::CCCCCCCC::::C HH::::::H     H::::::HH
//      T:::::::::T       E::::::::::::::::::::E   CC:::::::::::::::C H:::::::H     H:::::::H
//      T:::::::::T       E::::::::::::::::::::E     CCC::::::::::::C H:::::::H     H:::::::H
//      TTTTTTTTTTT       EEEEEEEEEEEEEEEEEEEEEE        CCCCCCCCCCCCC HHHHHHHHH     HHHHHHHHH


// CougTech Customer Logger
// V1.0
// Developed at Washington State University by
// Ryan Huard, Adam Rodriguez, and Mitchell Weholt
// Copyright 2016


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
            DirectoryTextBlock.Text = _main_window.LogPath;
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
        private void StartDayButton_Click(object sender, RoutedEventArgs e) {
            
            if (_main_window.Logging) {
                System.Windows.MessageBox.Show("The day has already been started!", "Error"); // avoid being able to start a day if it is already started
                return;
            }

            // create a csv writer if it does not exist. when you have to restart the program or it closes
            // can select the already created log and continue writing to it for the week
            if (_main_window.IsCsvNull()) {
                System.Windows.MessageBox.Show("Select a csv file to log to.", "Notice");
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.Filter = "CSV Files (*.csv)|*.csv";
                DialogResult res = fd.ShowDialog();
                if (System.Windows.Forms.DialogResult.OK == res)
                {
                    _main_window.CreateLog(fd.FileName, FileMode.Append);
                    _main_window.LogPath = Path.GetDirectoryName(fd.FileName); // update logging directory
                    DirectoryTextBlock.Text = _main_window.LogPath;
                }
                else // user did not select a file to log to
                {
                    var create = System.Windows.MessageBox.Show("No file selected. Create a new log?", "No Log Selected", MessageBoxButton.YesNo);
                    if (create == MessageBoxResult.Yes) { // create a log
                        _main_window.CreateLog("Week_of_" + DateTime.Now.ToString("MM-dd-yyyy"), FileMode.Create);
                    }
                    else { // if user says no to creating a log then don't start logging
                        System.Windows.MessageBox.Show("No file selected or created. Not starting log", "Notice");
                        return;
                    }
                }
            }

            _main_window.StartLog(); //! this defaults to setting Logging to true and EmailLogging to true

            // ask if want to enable emails
            var result = System.Windows.Forms.MessageBox.Show("Would you like to enable emails?", "Email", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _main_window.EmailLogging = false; // set to false before calling next function so it can execute correctly and enable EmailLogging
            }
            emailButton_Click(new object(), new RoutedEventArgs()); //!! this will set main windows EmailLogging accordingly

            MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
            mw.Show();
            LogGoingTextBlock.Text = "Logging: Yes";

        }

        //end the day involves ending the log (writting the totals and then closing the file) and then turning off logging
        private void EndDayButton_Click(object sender, RoutedEventArgs e) {
            
            if (false == _main_window.Logging) {
                System.Windows.MessageBox.Show("The day has not been started!", "Error"); // avoid being able to end a day if it is not started
                return;
            }
            
            _main_window.EndLog();
            MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"), 3.0);
            mw.Show();
            LogGoingTextBlock.Text = "Logging: No";
        }

        //changes the directory logs get saved to via the folder dialog
        private void LogDirectoryButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.DialogResult result = _folder_dialog.ShowDialog();
            if(System.Windows.Forms.DialogResult.OK == result) {
                _main_window.LogPath = _folder_dialog.SelectedPath.ToString();
            }
            DirectoryTextBlock.Text = _main_window.LogPath;
        }

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
                //PasswordWindow pw = new PasswordWindow();
                //pw.ShowDialog();

                _main_window.EmailPassword = "1Cougarhelp!"; // Hard coding for now
                _main_window.EmailLogging = true;
                emailSendTextBlock.Text = "Emails Sending: Yes";
                emailButton.Content = "Disable Emails";

                //System.Windows.MessageBox.Show("Email password set!");
            }
            else
            {
                _main_window.EmailPassword = ""; // clear out password when not sending email
                _main_window.EmailLogging = false;
                emailSendTextBlock.Text = "Emails Sending: No";
                emailButton.Content = "Email Login";
            }
        }

        
    }
}
