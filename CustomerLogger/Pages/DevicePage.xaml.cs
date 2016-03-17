using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for DevicePage.xaml
    /// </summary>

    //gives the customer a list of radial buttons so they can choose their device
    public partial class DevicePage:Page {

        public event EventHandler PageFinished; // this event will trigger once user clicks submit

        private string _device;

        public DevicePage() {
            InitializeComponent();
            SubmitButton.IsEnabled = false;
        }

        public string Device {
            get { return _device; }
        }

        //make sure the submit button is enabled first them move on
        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            if(SubmitButton.IsEnabled) {
                PageFinished(new object(), new EventArgs());
            }
        }

        //these radial button events are all the same,
        //set the device string to the option they selected and enable submitbutton
        //so they can continue
        private void ComputerButton_Click(object sender, RoutedEventArgs e) {
            _device = "Computer";
            SubmitButton.IsEnabled = true;
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e) {
            _device = "Phone";
            SubmitButton.IsEnabled = true;
        }

        private void TabletButton_Click(object sender, RoutedEventArgs e) {
            _device = "Tablet";
            SubmitButton.IsEnabled = true;
        }

        private void Other_Click(object sender, RoutedEventArgs e) {
            _device = "Other";
            SubmitButton.IsEnabled = true;
        }

        private void RentalButton_Click(object sender, RoutedEventArgs e) {
            _device = "Rental";
            SubmitButton.IsEnabled = true;
        }

        //make sure enter key works
        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }


    }
}
