using NeuralNetwork.DataUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MNIST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = (ViewModel)this.Resources["ViewModel"];
            this.DataContext = vm;
            int i = 0;
            foreach (var item in MnistReader.Read(MnistReader.TrainImages, MnistReader.TrainLabels))
            {
                vm.Images.Add(item);
                if (i++ > 200) break;
            }

            this.DataContext = this;

            vm.Image = vm.Images.First();


        }

        private void EvaluateButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Evaluate();
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Train();
        }
    }

}
