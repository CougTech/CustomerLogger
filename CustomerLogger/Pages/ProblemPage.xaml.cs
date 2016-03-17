using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for ProblemPage.xaml
    /// </summary>

    //allows the user to choose a problem from the list
    public partial class ProblemPage:Page {

        public event EventHandler PageFinished;
        private string _problem;

        public ProblemPage() {
            InitializeComponent();
            NextButton.IsEnabled = false;
        }

        public string Problem {
            get { return _problem; }
        }
        
        //when they have made an option customer can move on
        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if(NextButton.IsEnabled) {
                PageFinished(new object(), new EventArgs());
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
