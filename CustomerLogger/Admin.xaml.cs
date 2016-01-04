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
using System.Windows.Shapes;
using System.Windows.Forms;

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>

    public partial class AdminWindow:Window {

        MainWindow _main_window;
        System.Windows.Forms.FolderBrowserDialog _folder_dialog;

        public AdminWindow(MainWindow main_window) {
            InitializeComponent();
            _main_window = main_window;
            _folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
        }

        private void StartDayButton_Click(object sender, RoutedEventArgs e) {
            _main_window.StartLog(DateTime.Now.ToString("MM-dd-yyyy"));
            MessageWindow mw = new MessageWindow("Start New day \n" + DateTime.Now.ToString("MM-dd-yyyy"));
            mw.Show();
        }

        private void EndDayButton_Click(object sender, RoutedEventArgs e) {
            _main_window.EndLog();
            MessageWindow mw = new MessageWindow("End day \n" + DateTime.Now.ToString("MM-dd-yyyy"));
            mw.Show();
        }

        private void LogDirectoryButton_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.DialogResult result = _folder_dialog.ShowDialog();
            if(System.Windows.Forms.DialogResult.OK == result) {
                _main_window.LogPath = _folder_dialog.SelectedPath.ToString();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            _main_window.Close();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
