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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerLogger {
    /// <summary>
    /// Interaction logic for DevicePage.xaml
    /// </summary>
    
    //gives the customer a list of radial buttons so they can choose their device
    public partial class DevicePage:Page {

        MainWindow _main_window;

        private string _device;

        public DevicePage(MainWindow mw) {
            InitializeComponent();
            _main_window = mw;
            SubmitButton.IsEnabled = false;
        }

        public string Device {
            get { return _device; }
        }

        //make sure the submit button is enabled first them move on
        private void SubmitButton_Click(object sender, RoutedEventArgs e) {
            if(SubmitButton.IsEnabled) {
                _main_window.changePage(_main_window.ProblemPage);
            }
        }

        //these radial button events are all the same,
        //set the device string to the option they selected and enable submitbutton
        //so they can continue
        private void ComputerButton_Click(object sender, RoutedEventArgs e) {
            _device = "Computer";
            SubmitButton.IsEnabled = true;
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e) {
            _device = "Phone";
            SubmitButton.IsEnabled = true;
        }

        private void TabletButton_Click(object sender, RoutedEventArgs e) {
            _device = "Tablet";
            SubmitButton.IsEnabled = true;
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e) {
            _device = "Account";
            SubmitButton.IsEnabled = true;
        }

        private void Other_Click(object sender, RoutedEventArgs e) {
            _device = "Other";
            SubmitButton.IsEnabled = true;
        }

        private void RentalButton_Click(object sender, RoutedEventArgs e) {
            _device = "Rental";
            SubmitButton.IsEnabled = true;
        }

        //make sure enter key works
        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                SubmitButton_Click(sender, e);
            }
        }


    }
}
