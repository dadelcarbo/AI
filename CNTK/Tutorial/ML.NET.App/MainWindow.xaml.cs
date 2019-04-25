using ML.NET.App.PacMan.Agents;
using ML.NET.App.PacMan.Model;
using ML.NET.App.PacMan.View;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ML.NET.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World world;

        public static MainWindow Instance;
        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            world = World.Instance;
            world.GameCompleted += World_GameCompleted;

            var renderer = new GameRenderer(this.GameCanvas);

            this.GameCanvas.Height = this.GameCanvas.Width = World.SIZE * GameRenderer.SPRITE_SIZE;

            renderer.DrawWorld(world);

            GameObject.World = world;
            GameObject.Renderer = renderer;

            this.DataContext = world;
            world.PropertyChanged += World_PropertyChanged;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += world.GameLoop;

            this.KeyDown += MainWindow_KeyDown;

            //var agent = new Agent2D.Agent2D();
            //for (int i = 0; i < 15; i++)
            //{
            //    agent.Test();
            //    agent.Train();
            //}
            //agent.Test();
            //Environment.Exit(0);
        }

        DispatcherTimer timer = new DispatcherTimer();

        private void World_GameCompleted(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (world.CurrentAgent.Name == "Keyboard Agent")
            {
                var agent = (KeyboardAgent)world.CurrentAgent;
                switch (e.Key)
                {
                    case Key.Up:
                        agent.AddMove(PlayAction.Up);
                        break;
                    case Key.Down:
                        agent.AddMove(PlayAction.Down);
                        break;
                    case Key.Left:
                        agent.AddMove(PlayAction.Left);
                        break;
                    case Key.Right:
                        agent.AddMove(PlayAction.Right);
                        break;
                }
            }
        }

        private void World_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentAgent")
            {
                if (world.CurrentAgent.Name == "Keyboard Agent")
                {
                    this.Focus();
                }
            }
        }

        private void Start_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.world.Start();
            GameObject.Renderer.Clear();
            GameObject.Renderer.DrawWorld(world);

            this.timer.Start();
        }
        private void Stop_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.world.Stop();
        }

        private void ShowGraph_Button_Click(object sender, RoutedEventArgs e)
        {
            world.IsStopped = true;
            timer.Stop();

            var agent = new DijkstraAgent();
            agent.Activate();

            GameObject.Renderer.Clear();
            GameObject.Renderer.DrawGraph(agent.Graph);

        }
    }
}
