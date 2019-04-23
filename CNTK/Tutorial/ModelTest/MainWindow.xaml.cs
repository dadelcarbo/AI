using System;
using System.Collections.Generic;
using System.Windows;

namespace CNTKSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model model = new Model();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = model;

        }

        private void Test1Btn_Click(object sender, RoutedEventArgs e)
        {
            model.MLPTest();
        }
        private void Test2Btn_Click(object sender, RoutedEventArgs e)
        {
            model.CNNTest();
        }
        private void Test3Btn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Test4Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
