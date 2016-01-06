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

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for AdminPassword.xaml
    /// </summary>
    public partial class AdminPassword : Window
    {
        private bool _correct;
        //private System.Security.SecureString _pwd;

        public AdminPassword()
        {
            InitializeComponent();
            _correct = false;
            passwordBox.Focus();
        }

        public bool IsCorrect
        {
            get { return _correct; }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if(passwordBox.Password == "couglolz") { // hardcoded password for now...
                _correct = true;
            }

            this.Close();
        }

        private void passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                okButton_Click(sender, e);
            }
        }
    }
}
