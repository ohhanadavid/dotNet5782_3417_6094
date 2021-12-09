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
using IBL.BO;
using IBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL.BL bL;
       
        public MainWindow()
        {
            bL = new IBL.BL();
            InitializeComponent();
        }

        private void DroneMainButton_Click(object sender, RoutedEventArgs e)
        {
            
            new DronesListWindow(bL).ShowDialog();
            
        }
    }
}
