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
        string _defaultText = "Briefly describe your problem.";

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
            virusRadioButton.Click += RadioButton_Click;

        }

        public string Problem {
            get { return _problem; }
            set { _problem = value; }
        }
        
        public string Description {
            get { return _description; }
        }

        //when they have made an option customer can move on
        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if(NextButton.IsEnabled) {
                if (descriptionTextBox.Text != _defaultText) { 
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
            if (descriptionTextBox.Text.Length == 0 || string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                NextButton.IsEnabled = false; // disable 'Next' button if no text or text is just whitespace
                return;
            }

            if (descriptionTextBox.Text != _defaultText) {
                NextButton.IsEnabled = true; // enable button once text is not the default text
            }
        }

        private void descriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (descriptionTextBox.Text == _defaultText)
            {
                descriptionTextBox.Text = ""; // remove default text if user starts typing before deleting it
            }

        }

        private void descriptionTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // KeyDown event does not handle space or backspace key
            // this does
            if (e.Key == Key.Space || e.Key == Key.Back)
            {
                descriptionTextBox_KeyDown(sender, e);
            }
        }
    }
}
