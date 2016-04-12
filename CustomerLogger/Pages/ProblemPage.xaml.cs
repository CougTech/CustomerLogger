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
        private string _description;

        public ProblemPage() {
            InitializeComponent();
            NextButton.IsEnabled = false;
            requiredLabel.Visibility = Visibility.Hidden;
            descriptionTextBox.Visibility = Visibility.Hidden;
            descriptionTextBox.MaxLength = 120; // limit character limit in problem description
            _description = " ";

            // have radiobuttons subscribe to one event
            HardwareButton.Click += RadioButton_Click;
            SoftwareButton.Click += RadioButton_Click;
            WirelessButton.Click += RadioButton_Click;
            EmailButton.Click += RadioButton_Click;
            PasswordButton.Click += RadioButton_Click;
            Other.Click += RadioButton_Click;
            RentalButton.Click += RadioButton_Click;

        }

        public string Problem {
            get { return _problem; }
        }
        
        public string Description {
            get { return _description; }
        }

        //when they have made an option customer can move on
        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if(NextButton.IsEnabled) {
                if (descriptionTextBox.Text != "Shortly describe the problem you are having ...") { 
                    _description = descriptionTextBox.Text; // add description if its not the default text
                    _description = _description.Replace(",", string.Empty); // remove any commas in description because we save to a CSV
                }
                PageFinished(new object(), new EventArgs());
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {
            _problem = ((RadioButton)sender).Content.ToString(); // set problem to text of radio button
            descriptionTextBox.Visibility = Visibility.Visible;
            requiredLabel.Visibility = Visibility.Visible;
            descriptionTextBox.Focus();
            descriptionTextBox.SelectAll();
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                NextButton_Click(sender, e);
            }
        }

        private void descriptionTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (descriptionTextBox.Text.Length == 0)
            {
                NextButton.IsEnabled = false; // disable 'Next' button if no text or text is just whitespace
                descriptionTextBox.Text = "Shortly describe the problem you are having ...";
                descriptionTextBox.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                NextButton.IsEnabled = false;
                return;
            }

            if (descriptionTextBox.Text != "Shortly describe the problem you are having ...") {
                NextButton.IsEnabled = true; // enable button once text is not the default text
            }
        }

        private void descriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (descriptionTextBox.Text == "Shortly describe the problem you are having ...")
            {
                descriptionTextBox.Text = ""; // remove default text if user starts typing before deleting it
            }
        }
    }
}
