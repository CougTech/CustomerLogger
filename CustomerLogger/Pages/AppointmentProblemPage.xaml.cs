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
    /// Interaction logic for AppointmentProblemPage.xaml
    /// </summary>
    public partial class AppointmentProblemPage : Page
    {
        public event EventHandler PageFinished;
        private string _problem;

        public AppointmentProblemPage() {
            InitializeComponent();
            NextButton.IsEnabled = false;

            FormatReinstallBackup_Button.Click += RadioButton_Click;
            FormatReinstall_Button.Click += RadioButton_Click;
            Bootcamp_Button.Click += RadioButton_Click;
            Software_Button.Click += RadioButton_Click;
            Software_Button.Click += RadioButton_Click;
            Troubleshoot_Button.Click += RadioButton_Click;
        }

        public string Problem
        {
            get { return _problem; }
            set { _problem = value; }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {
            _problem = ((RadioButton)sender).Content.ToString(); // set problem to text of radio button
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) { 
            if (NextButton.IsEnabled == false) {
                PageFinished(new object(), new EventArgs());
            }
        }

    }
}
