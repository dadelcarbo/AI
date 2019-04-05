using ML.NET.App.PacMan.Agents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ML.NET.App.PacMan.Model
{

    public delegate void MovePerformedHandler(World world, PlayAction action);
    public class World : INotifyPropertyChanged
    {
        private static World instance;
        public static World Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new World();
                    instance.Initialize();
                }
                return instance;
            }
        }

        public Pacman Pacman = new Pacman(new Position(SIZE / 2, SIZE / 2));
        private World()
        {
        }

        private void Initialize()
        {
            // Create agents
            this.Agents = new List<IAgent>();
            this.Agents.Add(new KeyboardAgent());
            this.Agents.Add(new RandomAgent());
            this.Agents.Add(new EuristicAgent());
            this.Agents.Add(this.CurrentAgent = new MLAgent());

            this.CurrentAgent.Activate();
            this.Start();
        }

        public event EventHandler GameCompleted;
        public event MovePerformedHandler MovePerformed;

        DateTime startTime;

        public void Start()
        {
            this.IsStopped = false;
            this.Score = 0;
            this.Duration = 0;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    this.Values[i, j] = 0;
                }
            }
            // Create borders
            for (int i = 0; i < SIZE; i++)
            {
                this.Values[i, 0] = 1;
                this.Values[0, i] = 1;
                this.Values[i, SIZE - 1] = 1;
                this.Values[SIZE - 1, i] = 1;
            }

            // Create Coins
            for (int i = 0; i < NB_COINS;)
            {
                int x = rnd.Next(1, SIZE - 2);
                int y = rnd.Next(1, SIZE - 2);
                if (this.Values[x, y] == 2) continue;

                this.Values[x, y] = 2;
                i++;
            }

            this.Walls.Clear();
            this.Coins.Clear();
            // Create data array
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    switch (this.Values[i, j])
                    {
                        case 1: // Wall
                            this.Walls.Add(new Wall(new Position(j, i)));
                            break;
                        case 2: // Coin
                            this.Coins.Add(new Coin(new Position(j, i)));
                            break;

                    }
                }
            }

            this.startTime = DateTime.Now;
        }

        private void Stop()
        {
            this.Pacman.GoTo(new Position(SIZE / 2, SIZE / 2));

            this.IsStopped = true;
            this.GameCompleted?.Invoke(this, null);
        }
        public const int SIZE = 14;
        const int NB_COINS = 16;

        public int[,] Values = new int[SIZE, SIZE];

        public List<Coin> Coins = new List<Coin>();
        public List<Wall> Walls = new List<Wall>();

        public event PropertyChangedEventHandler PropertyChanged;

        private Random rnd = new Random();

        private int score;
        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        public string DurationString => this.Duration.ToString("F2");

        private double duration;
        public double Duration
        {
            get => duration;
            set
            {
                if (duration != value)
                {
                    duration = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Duration"));
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DurationString"));
                }
            }
        }
        private bool isStopped = false;
        public bool IsStopped
        {
            get => isStopped;
            set
            {
                if (isStopped != value)
                {
                    isStopped = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsStopped"));
                }
            }
        }

        public List<IAgent> Agents { get; private set; }

        public void GameLoop(object sender, EventArgs e)
        {
            var action = this.CurrentAgent.Decide(this);
            this.Pacman.PerformAction(action);
            this.Duration = (DateTime.Now - startTime).TotalSeconds;

            this.MovePerformed?.Invoke(this, action);
        }
        IAgent currentAgent;
        public IAgent CurrentAgent
        {
            get => currentAgent;
            set
            {
                if (currentAgent != value)
                {
                    if (currentAgent != null)
                        currentAgent.Deactivate();

                    currentAgent = value;
                    currentAgent.Activate();
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentAgent"));
                }
            }
        }

        public void GoTo(Position p)
        {
            switch (this.Values[p.Y, p.X])
            {
                case 0:
                    this.Pacman.GoTo(p);
                    break;
                case 1: break;
                case 2:
                    this.EatCoin(p);
                    this.Pacman.GoTo(p);
                    break;
                default:
                    break;
            }
        }

        private void EatCoin(Position p)
        {
            // Remove from world map
            this.Values[p.Y, p.X] = 0;

            // Remove from coin list
            var coin = this.Coins.First(c => c.Position.X == p.X && c.Position.Y == p.Y);
            this.Coins.Remove(coin);

            // Remove from render
            GameObject.Renderer.Remove(coin);

            this.Score += 20;

            if (this.Coins.Count == 0)
            {
                this.Stop();
            }
        }

        internal bool CanGoTo(Position p)
        {
            return this.Values[p.Y, p.X] != 1;
        }
    };
}
