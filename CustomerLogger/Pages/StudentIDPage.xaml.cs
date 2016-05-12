using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for SudentIDPage.xaml
    /// </summary>

    //inital page the customer sees
    //gets their student ID number
    //will auto place a 0 in front if it is not automatically there
    //also we can have the card swiper attached to the comptuer so they
    //can swipe the cougarcard and it will work as if they typed it in
    //pretty nifty, because the card swiper just puts the number on the card
    //as if it can from stdin

    //because it is set to use the enter key to move on as well as clicking the next button,
    //if they swipe the card it should automatically save their number and move onto the next page
    //with out the user needing to click.
    public partial class StudentIDPage:Page {

        private string _student_id;
        private bool _isTest = false;
        public event EventHandler PageFinished;
    
        public StudentIDPage() {
            InitializeComponent();

            SubmitButton.IsEnabled = false;
            StudentNumberTextBox.Focus();
        }

        public string StudentID {
            get { return _student_id; }
        }

        public bool isTest
        {
            get { return _isTest; }
        }

        //when clicked we move on to the next page
        private void SubmitButton_Click(object sender, RoutedEventArgs e) {

            if(SubmitButton.IsEnabled) {
                _student_id = StudentNumberTextBox.Text;
                // add 0 to id number if not present
                if (_student_id.Length == 8)
                {
                    _student_id = "0" + _student_id;
                }
                PageFinished(new object(), new EventArgs()); // fire event to let main window know that submit was clicked
            }

        }

        //grabs all text in the text box
        //incase customer thinks they did it wrong and then
        //wants to go back, it will highlight all of the text
        //woot woot for usability features
        private void StudentNumberTextBox_GotFocus(object sender, RoutedEventArgs e) {
            StudentNumberTextBox.SelectAll();
        }

        //when they type in the box this event will fire
        private void StudentNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {

            bool correct_length;
            bool is_num;

            // Change maxLength to 9 if the first digit is 0
            // Must check if the box is empty or the program will crash if backspaced
            if (StudentNumberTextBox.Text != "" && StudentNumberTextBox.Text[0] == '0')
            {
                StudentNumberTextBox.MaxLength = 9;
            }

            //make sure we have a valid length, either 8 without a leading 0 or nine with a leading 0
            if (StudentNumberTextBox.Text.Length == 8 || (StudentNumberTextBox.Text.Length == 9 && StudentNumberTextBox.Text[0] == '0')) { // length of 9 is only correct if first digit is 0

                correct_length = true;
            } else {

                correct_length = false;
            }

            //make sure we actually have an integer as the student id number
            //no decimals
            //woot woot for input sanitization
            int n;
            is_num = int.TryParse(StudentNumberTextBox.Text, out n);

            //if we have a valid id number, IE correct length and its an int
            //then we can allow the customer to go on
            if(is_num && correct_length) {
                SubmitButton.IsEnabled = true;
            } else {
                SubmitButton.IsEnabled = false;
            }

            // Check to see if this is a test ticket
            if (StudentNumberTextBox.Text == "00000000") {
                SubmitButton.Background = System.Windows.Media.Brushes.Crimson;
                SubmitButton.Content = "TEST";
                _isTest = true;
            }
            else {
                SubmitButton.Background = System.Windows.Media.Brushes.White;
                SubmitButton.Content = "Next";
                _isTest = false;
            }

        }

        //allows for enter key to be used as a click for the submit button
        private void StudentNumberTextBox_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }

        }
    }
}
