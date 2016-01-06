using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for SummayPage.xaml
    /// </summary>
    public partial class SummaryPage : Page {
        MainWindow _main_window;
        public SummaryPage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
        }

        public void setText() {

            StudentID.Text = _main_window.StudentIDPage.StudentID;
            Device.Text = _main_window.DevicePage.Device;
            Problem.Text = _main_window.ProblemPage.Problem;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            _main_window.addToCurrent(StudentID.Text);
            _main_window.addToCurrent(Device.Text);
            _main_window.addToCurrent(Problem.Text);
            _main_window.writeLine();
            _main_window.Reset();
            SubmitButton.IsEnabled = false;
        }

    }
}
