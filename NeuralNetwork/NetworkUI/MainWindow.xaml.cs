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

        private void LayerCalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            LayerViewModel.Current.Calculate();
        }

        private void LayerTrainButton_OnClick(object sender, RoutedEventArgs e)
        {
            LayerViewModel.Current.Train();
        }

        private void LayerInitializeButton_OnClick(object sender, RoutedEventArgs e)
        {
            LayerViewModel.Current.Initialize();
        }

        private void NetworkCalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            NetworkViewModel.Current.Calculate();
        }

        private void NetworkTrainButton_OnClick(object sender, RoutedEventArgs e)
        {
            NetworkViewModel.Current.Train();
        }

        private void NetworkInitializeButton_OnClick(object sender, RoutedEventArgs e)
        {
            NetworkViewModel.Current.Initialize();
        }
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:F3}";
        }
    }
}
