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

namespace CustomerLogger.Pages
{
    /// <summary>
    /// Interaction logic for AppointmentProblemPage.xaml.
    /// The appointment problem page queries the customer for the type of appointment they are signing in to.
    /// </summary>
    public partial class AppointmentProblemPage : Page
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        public event EventHandler PageFinished; //Event handler for when the Next button is pressed

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppointmentProblemPage()
        {
            InitializeComponent();

            //Disable next button
            SubmitButton.IsEnabled = false;

            //Add radio button event handlers
            FormatReinstallBackup_Button.Click += RadioButton_Click;
            FormatReinstall_Button.Click += RadioButton_Click;
            Bootcamp_Button.Click += RadioButton_Click;
            Software_Button.Click += RadioButton_Click;
            Software_Button.Click += RadioButton_Click;
            Troubleshoot_Button.Click += RadioButton_Click;
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

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
        /// Event handler for when a radio button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            //Set problem to text of radio button
            if (Cougtech_CustomerLogger.Problem_SelectionChanged(((RadioButton)sender).Content.ToString()))
                SubmitButton.IsEnabled = true;
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");
        }

        /// <summary>
        /// Event handler for when the next button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //If the submit button is enabled
            if (SubmitButton.IsEnabled)
                PageFinished(new object(), new EventArgs()); //Leave this page
        }
    }
}
