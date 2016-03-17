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
    /// Interaction logic for AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        public event EventHandler PageFinished;
        private bool _hasAppointment;

        public bool HasAppointment
        {
            get { return _hasAppointment; }
        }

        public AppointmentPage()
        {
            InitializeComponent();
            nextButton.IsEnabled = false;
            _hasAppointment = false;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            PageFinished(new object(), new EventArgs());
        }

        private void yesRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _hasAppointment = true;
            nextButton.IsEnabled = true;
        }

        private void noRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _hasAppointment = false;
            nextButton.IsEnabled = true;
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitButton_Click(sender, e);
            }
        }
    }
}
