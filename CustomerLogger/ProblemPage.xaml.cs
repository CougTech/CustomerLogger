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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for ProblemPage.xaml
    /// </summary>
    
    //allows the user to choose a problem from the list
    public partial class ProblemPage:Page {

        private MainWindow _main_window;

        private string _problem;

        public ProblemPage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
            NextButton.IsEnabled = false;
        }

        public string Problem {
            get { return _problem; }
        }
        
        //when they have made an option customer can move on
        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if(NextButton.IsEnabled) {
                _main_window.SummaryPage.setText();
                _main_window.changePage(_main_window.SummaryPage);
            }
        }

        //all of these radial buttons are pretty similar. The customer
        //selects there issue. Then we save the text of the problem
        //and then we enable the next button.
        private void HardwareButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Hardware";
            NextButton.IsEnabled = true;
        }

        private void SoftwareButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Software";
            NextButton.IsEnabled = true;
        }

        private void WirelessButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Wireless";
            NextButton.IsEnabled = true;
        }

        private void EmailButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Email";
            NextButton.IsEnabled = true;
        }

        private void PasswordButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Password";
            NextButton.IsEnabled = true;
        }

        private void WSUServiceButton_Click(object sender, RoutedEventArgs e) {
            _problem = "WSU Service";
            NextButton.IsEnabled = true;
        }

        private void ApplicationButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Application";
            NextButton.IsEnabled = true;
        }

        private void Other_Click(object sender, RoutedEventArgs e) {
            _problem = "Other";
            NextButton.IsEnabled = true;
        }

        private void RentalButton_Click(object sender, RoutedEventArgs e) {
            _problem = "Rent/Checkout/Extend Rental";
            NextButton.IsEnabled = true;
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                NextButton_Click(sender, e);
            }
        }
    }
}
