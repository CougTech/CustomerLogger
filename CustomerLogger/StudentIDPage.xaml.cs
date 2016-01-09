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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for SudentIDPage.xaml
    /// </summary>
    public partial class StudentIDPage:Page {

        MainWindow _main_window;

        private string _student_id;

        public StudentIDPage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
            //this.Width = _main_window.Width;
            //this.Height = _main_window.Height;

            SubmitButton.IsEnabled = false;
            StudentNumberTextBox.Focus();
        }

        public string StudentID {
            get { return _student_id; }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e) {

            if(SubmitButton.IsEnabled) {
                _student_id = StudentNumberTextBox.Text;
                _main_window.changePage(_main_window.DevicePage);
            }

        }

        private void StudentNumberTextBox_GotFocus(object sender, RoutedEventArgs e) {
            StudentNumberTextBox.SelectAll();
        }

        private void StudentNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {

            bool correct_length;
            bool is_num;

            if(StudentNumberTextBox.Text.Length == 8 || StudentNumberTextBox.Text.Length == 9) {

                correct_length = true;
            } else {

                correct_length = false;
            }

            int n;
            is_num = int.TryParse(StudentNumberTextBox.Text, out n);

            if(is_num && correct_length) {
                SubmitButton.IsEnabled = true;
            } else {
                SubmitButton.IsEnabled = false;
            }
        }

        private void StudentNumberTextBox_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }
    }
}
