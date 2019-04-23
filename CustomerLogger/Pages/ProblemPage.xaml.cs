using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for ProblemPage.xaml.
    /// The problem page provides a way for the customer to describe their issue in more detail.
    /// </summary>
    public partial class ProblemPage : Page
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        public event EventHandler PageFinished;

        private const string m_sDefaultTextBox = "Briefly describe your problem.";

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProblemPage()
        {
            InitializeComponent();

            //Set initial page data
            SubmitButton.IsEnabled = false;                     //Disable next button
            requiredLabel.Visibility = Visibility.Hidden;       //Hide the red asterisk
            DescriptionTextBox.Visibility = Visibility.Hidden;  //Hide the text box
            DescriptionTextBox.MaxLength = 120;                 //Set the text box's max length to 120 characters

            //Add radio button event handlers
            HardwareButton.Click += RadioButton_Click_TextboxFocus;
            SoftwareButton.Click += RadioButton_Click_TextboxFocus;
            WirelessButton.Click += RadioButton_Click_TextboxFocus;
            EmailButton.Click += RadioButton_Click_TextboxFocus;
            PasswordButton.Click += RadioButton_Click_TextboxFocus;
            virusRadioButton.Click += RadioButton_Click_TextboxFocus;
            HealthCheckButton.Click += RadioButton_Click_NoTextboxFocus;
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Event handler for a keypress within the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (DescriptionTextBox.Text == m_sDefaultTextBox)   //If the text in the description box is the default text
                DescriptionTextBox.Text = "";                       //Remove it once the user begins typing
        }

        /// <summary>
        /// Event handler for a keypress from a preview key.
        /// </summary>
        /// <remarks>
        /// A preview key is not a standard alphanumeric character key such as "Backspace" and "Space".
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Space) || (e.Key == Key.Back))    //If the key pressed is "Enter" or "Backspace"
                DescriptionTextBox_KeyDown(sender, e);              //Call the keydown event handler
        }

        /// <summary>
        /// Event handler for text changing within the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Set the ticket description
            if (Cougtech_CustomerLogger.Description_TextChanged(DescriptionTextBox.Text))
            {
                //If successful
                if ((DescriptionTextBox.Text == m_sDefaultTextBox) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text))   //If the default description is present, or there is no content within the text box
                    SubmitButton.IsEnabled = false;                                                                               //Disable the submit button
                else                                                                                                        //Else
                    SubmitButton.IsEnabled = true;                                                                                //Enable the submit button
            }
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");
        }

        /// <summary>
        /// Event handler for a keypress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            //If the user has pressed "Enter", click the submit button
            if (e.Key == Key.Enter)
                SubmitButton_Click(sender, e);
        }

        /// <summary>
        /// Event handler for when the "Next" button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //If the submit button is enabled
            if (SubmitButton.IsEnabled)
            {
                //Leave this page
                PageFinished(new object(), new EventArgs());
            }
        }

        /// <summary>
        /// Event handler for when a problem radio button has been checked and the problem does not require a description.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click_NoTextboxFocus(object sender, RoutedEventArgs e)
        {
            //Set the problem to the text of the radio buttton
            if (Cougtech_CustomerLogger.Problem_SelectionChanged(((RadioButton)sender).Content.ToString()))
            {
                //If successfull
                SubmitButton.IsEnabled = true; //Enable the "Next" button

                requiredLabel.Visibility = Visibility.Hidden;       //Hide the red asterisk
                DescriptionTextBox.Visibility = Visibility.Hidden;  //Hide the textbox
            }
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");
        }

        /// <summary>
        /// Event handler for when a problem radio button has been checked and the problem does require a description.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click_TextboxFocus(object sender, RoutedEventArgs e)
        { 
            //Set the problem to the text of the radio buttton
            if(Cougtech_CustomerLogger.Problem_SelectionChanged(((RadioButton) sender).Content.ToString())) 
            {
                //If successfull
                SubmitButton.IsEnabled = false; //Disable the "Next" button

                DescriptionTextBox.Visibility = Visibility.Visible; //Make the red asterisk visible
                requiredLabel.Visibility = Visibility.Visible;      //Make the textbox visible

                DescriptionTextBox.Focus();                         //Focus on the textbox
                DescriptionTextBox.SelectAll();                     //Select the text within the textbox
            }
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");
        }
    }
}
