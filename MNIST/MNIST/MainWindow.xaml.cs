using MNIST.ML.NET.DataUtils;
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
            vm.Initialize();
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
