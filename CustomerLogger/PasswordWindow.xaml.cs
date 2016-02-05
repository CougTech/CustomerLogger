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
    
    //when the customer wants to go to the admin window they have to get through this first
    //pretty simple just gets the password from the user and saves it to check when going to admin window
    public partial class PasswordWindow : Window
    {
        private string _pwd;

        public PasswordWindow()
        {
            InitializeComponent();
            _pwd = "";
            passwordBox.Focus();
        }

        public string Password
        {
            get { return _pwd; }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            _pwd = passwordBox.Password;

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
