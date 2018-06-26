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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Current.Calculate();
        }

        private void TrainButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Current.Train();
        }

        private void InitializeButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Current.Initialize();
        }
    }
}
