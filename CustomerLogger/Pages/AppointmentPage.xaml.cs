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
    /// Interaction logic for AppointmentPage.xaml.
    /// The appointment page asks a customer whether they are signing in for an already scheduled appointment or not.
    /// </summary>
    public partial class AppointmentPage : Page
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        public event EventHandler PageFinished; //Event handler for when the next button is completed

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppointmentPage()
        {
            InitializeComponent();

            //Disable next button and appointment flag
            SubmitButton.IsEnabled = false;
        }

        /// <summary>
        /// Resets all dynamic fields on the page.
        /// </summary>
        public void Reset()
        {
            CustomerName.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFirstName"></param>
        public void Set_text(string sFirstName)
        {
            CustomerName.Text = sFirstName + "!";
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
        /// Event handler for when the No radio button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cougtech_CustomerLogger.Appointment_Set(false))
                SubmitButton.IsEnabled = true;
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");
        }

        /// <summary>
        /// Event handler for when the submit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //If the submit button is enabled
            if (SubmitButton.IsEnabled)
                PageFinished(new object(), new EventArgs());
        }

        /// <summary>
        /// Event handler for when the Yes radio button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (Cougtech_CustomerLogger.Appointment_Set(true))
                SubmitButton.IsEnabled = true;
            else
                MessageBox.Show("The ticketing system has not been initialized correctly./nPlease re-enter your student ID", "System Error");

        }
    }
}
