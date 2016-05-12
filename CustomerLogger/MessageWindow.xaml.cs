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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    
    //Displays an arbitrary message
    //used for errors or feedback or lamas, or whatever
    //just keep it classy
    public partial class MessageWindow:Window {

        private System.Windows.Threading.DispatcherTimer openTimer;

        //message is the text you want to display
        //If you put a new line in the message string then the message
        //should be displayed on the two lines.
        //interval is the seconds you want it to desplay for
        public MessageWindow(string message, double interval) {
            InitializeComponent();
    
            MessageTextBox.Text = message;

            // Display this window for interval seconds
            openTimer = new System.Windows.Threading.DispatcherTimer();
            openTimer.Interval = TimeSpan.FromSeconds(interval);
            openTimer.IsEnabled = true;
            openTimer.Tick += new EventHandler(this.close_window);

            OKButton.Focus(); // focus on OK button so user can press enter to close popup
        }

        //when you are done click ok
        private void OKButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        //auto closed after time is done
        private void close_window(Object sender, EventArgs args) {
            openTimer.IsEnabled = false;
            this.Close();
        }
    }
}
