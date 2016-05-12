//        CCCCCCCCCCCCC     OOOOOOOOO      UUUUUUUU     UUUUUUUU       GGGGGGGGGGGGG       
//     CCC::::::::::::C   OO:::::::::OO    U::::::U     U::::::U    GGG::::::::::::G       
//   CC:::::::::::::::C OO:::::::::::::OO  U::::::U     U::::::U  GG:::::::::::::::G       
//  C:::::CCCCCCCC::::C O:::::::OOO:::::::O UU:::::U     U:::::UU G:::::GGGGGGGG::::G       
// C:::::C       CCCCCC O::::::O   O::::::O  U:::::U     U:::::U  G:::::G       GGGGGG       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G                     
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G                     
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    GGGGGGGGGG       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    G::::::::G       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G    GGGGG::::G       
//C:::::C               O:::::O     O:::::O  U:::::D     D:::::U G:::::G        G::::G       
// C:::::C       CCCCCC O::::::O   O::::::O  U::::::U   U::::::U G:::::G       G::::G       
//  C:::::CCCCCCCC::::C O:::::::OOO:::::::O  U:::::::UUU:::::::U  G:::::GGGGGGGG::::G       
//   CC:::::::::::::::C  OO:::::::::::::OO    UU:::::::::::::UU    GG:::::::::::::::G       
//     CCC::::::::::::C    OO:::::::::OO        UU:::::::::UU        GGG::::::GGG:::G       
//        CCCCCCCCCCCCC      OOOOOOOOO            UUUUUUUUU             GGGGGG   GGGG
//
//TTTTTTTTTTTTTTTTTTTTTTT EEEEEEEEEEEEEEEEEEEEEE       CCCCCCCCCCCCC HHHHHHHHH     HHHHHHHHH
//T:::::::::::::::::::::T E::::::::::::::::::::E    CCC::::::::::::C H:::::::H     H:::::::H
//T:::::::::::::::::::::T E::::::::::::::::::::E  CC:::::::::::::::C H:::::::H     H:::::::H
//T:::::TT:::::::TT:::::T EE::::::EEEEEEEEE::::E C:::::CCCCCCCC::::C HH::::::H     H::::::HH
//TTTTTT  T:::::T  TTTTTT   E:::::E       EEEEEE C:::::C       CCCCCC   H:::::H     H:::::H  
//        T:::::T           E:::::E             C:::::C                 H:::::H     H:::::H  
//        T:::::T           E::::::EEEEEEEEEE   C:::::C                 H::::::HHHHH::::::H  
//        T:::::T           E:::::::::::::::E   C:::::C                 H:::::::::::::::::H  
//        T:::::T           E:::::::::::::::E   C:::::C                 H:::::::::::::::::H  
//        T:::::T           E::::::EEEEEEEEEE   C:::::C                 H::::::HHHHH::::::H  
//        T:::::T           E:::::E             C:::::C                 H:::::H     H:::::H  
//        T:::::T           E:::::E       EEEEEE C:::::C       CCCCCC   H:::::H     H:::::H  
//      TT:::::::TT       EE::::::EEEEEEEE:::::E  C:::::CCCCCCCC::::C HH::::::H     H::::::HH
//      T:::::::::T       E::::::::::::::::::::E   CC:::::::::::::::C H:::::::H     H:::::::H
//      T:::::::::T       E::::::::::::::::::::E     CCC::::::::::::C H:::::::H     H:::::::H
//      TTTTTTTTTTT       EEEEEEEEEEEEEEEEEEEEEE        CCCCCCCCCCCCC HHHHHHHHH     HHHHHHHHH


// CougTech Customer Logger
// V1.0
// Developed at Washington State University by
// Ryan Huard, Adam Rodriguez, and Mitchell Weholt
// Copyright 2016


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

        public AppointmentProblemPage()
        {
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

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            _problem = ((RadioButton)sender).Content.ToString(); // set problem to text of radio button
            NextButton.IsEnabled = true;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (NextButton.IsEnabled == true)
            {
                PageFinished(new object(), new EventArgs());
            }
        }

    }
}
