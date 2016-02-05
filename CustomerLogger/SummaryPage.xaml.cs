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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for SummayPage.xaml
    /// </summary>
    
    //Page that will display the information customer has signed in with
    //displayed as last step of sign in
    public partial class SummaryPage:Page {
        MainWindow _main_window;
        public SummaryPage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
            SubmitButton.IsEnabled = true;
            SubmitButton.Focus();
        }

        public void setText() {

            StudentID.Text = _main_window.StudentIDPage.StudentID;
            Device.Text = _main_window.DevicePage.Device;
            Problem.Text = _main_window.ProblemPage.Problem;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            //first make sure they can submit
            if(SubmitButton.IsEnabled) {
                //if we are logging
                if (_main_window.EmailLogging == true && (Device.Text != "Rental" && Problem.Text != "Rent/Checkout/Extend Rental")) { // don't send tickets that are rentals (ramsay creates one)
                    int result = _main_window.SendTicket(); // send in otrs ticket 
                    if (result < 0) {
                        return; // Don't write to file if attempt to send emails.. this will prevent duplicates and keep the summary page open
                    }
                }

                //write to csv file
                _main_window.addToCurrent(StudentID.Text);
                _main_window.addToCurrent(Device.Text);
                _main_window.addToCurrent(Problem.Text);
                _main_window.writeLine();

                //let customer know they can sit down
                MessageWindow mw = new MessageWindow("Thank you, please have a seat and we will be right with you", 4.0);
                mw.ShowDialog();

                //get back to first state
                _main_window.Reset();
                SubmitButton.IsEnabled = false;
            }
        }

        //allow customer to hit enter or click submit button
        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }

    }
}
