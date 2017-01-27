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

namespace CustomerLogger
{
    /// <summary>
    /// Interaction logic for SummayPage.xaml
    /// </summary>
    
    //Page that will display the information customer has signed in with
    //displayed as last step of sign in
    public partial class SummaryPage:Page
    {

        public event EventHandler PageFinished; // triggered when submit is clicked
        private System.Windows.Threading.DispatcherTimer submitTimer; // timer to automatically click submit after 10 seconds

        public SummaryPage()
        {
            InitializeComponent();
            SubmitButton.Focus();
        }

        private void SubmitTimer_Tick(object sender, EventArgs e)
        {
            SubmitButton_Click(sender, new RoutedEventArgs());
        }

        public void SetText(string id, string prob, string descr)
        { 
            StudentID.Text = id;
            Problem.Text = prob;

            descText.Text = descr;
            descText.Visibility = Visibility.Visible;
            descriptionLabel.Visibility = Visibility.Visible;
           
        }

        public void StartTimer()
        {
            submitTimer = new System.Windows.Threading.DispatcherTimer();
            submitTimer.Interval = TimeSpan.FromSeconds(10);
            submitTimer.IsEnabled = true;
            submitTimer.Tick += SubmitTimer_Tick;
        }

        public void StopTimer()
        {
            if (submitTimer != null) {
                submitTimer.IsEnabled = false; // stop timer
                submitTimer = null;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            submitTimer.IsEnabled = false;
            PageFinished(new object(), new EventArgs());
        }

        //allow customer to hit enter or click submit button
        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }

    }
}
