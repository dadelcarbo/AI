using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model model;
        private ViewModel vm;

        public MainWindow()
        {
            model = Model.Current;
            model.OnDead += Model_OnDead;

            vm = new ViewModel();

            this.DataContext = vm;

            InitializeComponent();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += Timer_Tick;
        }

        private void Model_OnDead()
        {
            model.IsStarted = false;
            timer.Stop();

            MessageBox.Show("Game Over");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (model.IsStarted)
            {
                model.Update();
            }
        }

        private readonly DispatcherTimer timer;

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!model.IsStarted)
            {
                model.Start();
                timer.Start();
            }
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (model.IsStarted)
            {
                model.Stop();
                timer.Stop();
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (model.IsStarted && vm.Agent.Name == "Manual")
            {
                switch (e.Key)
                {
                    case Key.Right:
                        model.MoveRight();
                        e.Handled = true;
                        break;
                    case Key.Left:
                        model.MoveLeft();
                        e.Handled = true;
                        break;
                }
            }
        }
    }
}
