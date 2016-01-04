﻿using System;
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

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow:Window {
        public MessageWindow(string message) {
            InitializeComponent();

            MessageTextBox.Text = message;


        }

        private void OKButton_Click(object sender, RoutedEventArgs e) {
            Task.Delay(TimeSpan.FromSeconds(3)).ContinueWith((x) => close_window());
            //this.Close();
        }

        private void close_window() { 
        
            this.Close();
        }
    }
}
