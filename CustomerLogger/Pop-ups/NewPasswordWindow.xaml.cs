using System.Windows;
using System.Windows.Input;

namespace CustomerLogger.Popup
{
    /// <summary>
    /// Interaction logic for NewPasswordWindow.xaml
    /// </summary>
    public partial class NewPasswordWindow : Window
    {
        //  Constructor ///////////////////////////////////////////////////////////////////////////

        public NewPasswordWindow()
        {
            InitializeComponent();
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(NewPassword_PswdBox.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string sHashedPassword = System.Text.Encoding.ASCII.GetString(data);

            Cougtech_CustomerLogger.Admin_Password_Hashed = sHashedPassword;

            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmPassword_PswdBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ConfirmPassword_PswdBox.Password = "";
            NewPassword_Unconfirmed();
        }

        private void ConfirmPassword_PswdBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (ConfirmPassword_PswdBox.Password == NewPassword_PswdBox.Password)
                NewPassword_Confirmed();
            else
                NewPassword_Unconfirmed();
        }

        private void NewPassword_Confirmed()
        {
            ConfirmPassword_PswdBox.Background = System.Windows.Media.Brushes.LightGreen;
            ConfirmPassword_PswdBox.BorderBrush = System.Windows.Media.Brushes.DarkGreen;

            Ok_Button.IsEnabled = true;
            Ok_Button.Focus();
        }

        private void NewPassword_Unconfirmed()
        {
            ConfirmPassword_PswdBox.Background = System.Windows.Media.Brushes.Red;
            ConfirmPassword_PswdBox.BorderBrush = System.Windows.Media.Brushes.DarkRed;

            Ok_Button.IsEnabled = false;
        }
    }
}
