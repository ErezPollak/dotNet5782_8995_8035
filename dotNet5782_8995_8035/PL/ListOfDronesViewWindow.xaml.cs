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

namespace PL
{
    /// <summary>
    /// Interaction logic for ListOfDronesViewWindow.xaml
    /// </summary>
    public partial class ListOfDronesViewWindow : Window
    {
        private IBL.IBL bl;

        public ListOfDronesViewWindow(IBL.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            ListOfDronesView.ItemsSource = bl.GetDrones(t => true);
        }
    }
}