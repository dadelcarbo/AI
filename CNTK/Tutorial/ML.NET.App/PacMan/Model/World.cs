using ML.NET.App.PacMan.Agents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        public Pacman Pacman = new Pacman(new Position(1, 1));
        private World()
        {
            this.currentLevel = 0;
            this.isStopped = true;
        }

        private void Initialize()
        {
            // Create agents
            this.Agents = new List<IAgent>();
            this.Agents.Add(new KeyboardAgent());
            this.Agents.Add(new RandomAgent());
            this.Agents.Add(new EuristicAgent());
            this.Agents.Add(new DijkstraAgent());
            this.Agents.Add(new GreedyAgent());
            //this.Agents.Add(new CNNAgent());
            this.Agents.Add(new SupervisedAgent());
            this.Agents.Add(this.CurrentAgent = new SupervisedAgent2());
            this.Agents.Add(new MLPAgent());

            InitLevel();
        }

        public event EventHandler GameCompleted;
        public event MovePerformedHandler MovePerformed;

        DateTime startTime;

        public void Start()
        {
            this.IsStopped = false;
            this.Score = 200;
            this.Duration = 0;
            InitLevel();

            this.startTime = DateTime.Now;
        }

        private void InitLevel()
        {
            if (currentLevel == 0)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        this.Values[i * SIZE + j] = 0;
                    }
                }
                // Create borders
                for (int i = 0; i < SIZE; i++)
                {
                    this.Values[i] = 1;
                    this.Values[(SIZE - 1) * SIZE + i] = 1;
                    this.Values[i * SIZE] = 1;
                    this.Values[(SIZE - 1) * SIZE + i] = 1;
                    this.Values[i * SIZE + (SIZE - 1)] = 1;
                }

                // Create Coin
                int x = SIZE - 2, y = 1;
                this.Values[y * SIZE + x] = 2;

                // Create Pacman
                x = 1; y = SIZE - 2;
                this.Values[y * SIZE + x] = 9;


                //for (int i = 0; i < NB_COINS;)
                //{
                //    int x = rnd.Next(1, SIZE - 2);
                //    int y = rnd.Next(1, SIZE - 2);
                //    if (this.Values[x, y] != 0) continue;

                //    this.Values[x, y] = 2;
                //    i++;
                //}
                //// Create Ennemies
                //for (int i = 0; i < NB_ENNEMIES;)
                //{
                //    int x = rnd.Next(1, SIZE - 2);
                //    int y = rnd.Next(1, SIZE - 2);
                //    if (this.Values[x, y] == 3) continue;

                //    this.Values[x, y] = 3;
                //    i++;
                //}
                //bool ok = false;
                //do
                //{
                //    int i = rnd.Next(1, SIZE - 2);
                //    int j = rnd.Next(1, SIZE - 2);
                //    if (this.Values[i, j] == 0)
                //    {
                //        ok = true;
                //        this.Pacman.Position = new Position(j, i);
                //    }
                //}
                //while (!ok);
            }
            else
            {
                var filePath = $"PacMan\\Level\\Level{currentLevel}.txt";

                // Read a text file line by line.  
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < SIZE; i++)
                {
                    var items = lines[i].Split(',');
                    for (int j = 0; j < SIZE; j++)
                    {
                        this.Values[i * SIZE + j] = int.Parse(items[j]);
                    }
                }
            }

            this.Walls.Clear();
            this.Coins.Clear();
            this.Ennemies.Clear();
            // Create data array
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    switch (this.Values[i * SIZE + j])
                    {
                        case 1: // Wall
                            this.Walls.Add(new Wall(new Position(j, i)));
                            break;
                        case 2: // Coin
                            this.Coins.Add(new Coin(new Position(j, i)));
                            break;
                        case 3: // Ennemy
                            this.Ennemies.Add(new Ennemy(new Position(j, i)));
                            break;
                        case 9: // Pacman
                            this.Pacman.Position = new Position(j, i);
                            this.Values[i * SIZE + j] = 0; // Pacman is not listed in Values
                            break;
                    }
                }
            }
        }

        private void CreateCoin(int nbCoins)
        {
            // Create Coins
            for (int n = 0; n < nbCoins;)
            {
                int j = rnd.Next(1, SIZE - 1);
                int i = rnd.Next(1, SIZE - 1);

                if (this.Values[i * World.SIZE + j] != 0) continue;
                if (this.Pacman.Position.X == j && this.Pacman.Position.Y == i) continue;

                this.Values[i * World.SIZE + j] = 2;
                Coin coin = new Coin(new Position(j, i));
                Coin.Renderer.Draw(coin);
                this.Coins.Add(coin);
                n++;
            }
        }

        public void Stop()
        {
            this.Pacman.GoTo(new Position(SIZE / 2, SIZE / 2));

            this.IsStopped = true;
            this.GameCompleted?.Invoke(this, null);
        }
        public const int SIZE = 7;
        public const int PLAY_ACTION_COUNT = 5;
        const int NB_COINS = 8;
        const int NB_ENNEMIES = 0;

        public int[] Values = new int[SIZE * SIZE];

        public List<Coin> Coins = new List<Coin>();
        public List<Wall> Walls = new List<Wall>();
        public List<Ennemy> Ennemies = new List<Ennemy>();

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

        private int epsilon;
        public int Epsilon
        {
            get => epsilon;
            set
            {
                if (epsilon != value)
                {
                    epsilon = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Epsilon"));
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

        private static List<int> levels = new List<int> { 0, 1, 2, 3, 4, 5 };
        public List<int> Levels => levels;

        int currentLevel = 0;
        public int CurrentLevel
        {
            get => currentLevel;
            set
            {
                if (currentLevel != value)
                {
                    currentLevel = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentLevel"));
                }
            }
        }

        public void GoTo(Position p)
        {
            switch (this.Values[p.Y * SIZE + p.X])
            {
                case 0:
                    this.Pacman.GoTo(p);
                    break;
                case 1:
                    break;
                case 2:

                    int j = rnd.Next(1, SIZE - 1);
                    int i = rnd.Next(1, SIZE - 1);

                    this.Pacman.GoTo(new Position(j, i));
                    this.EatCoin(p);
                    break;
                default:
                    break;
            }
        }

        private void EatCoin(Position p)
        {
            // Remove from world map
            this.Values[p.Y * SIZE + p.X] = 0;

            // Remove from coin list
            var coin = this.Coins.First(c => c.Position.X == p.X && c.Position.Y == p.Y);
            this.Coins.Remove(coin);

            // Remove from render
            GameObject.Renderer.Remove(coin);

            this.Score += 200;

            if (this.Coins.Count < 10)
            {
                CreateCoin(1);
            }

            if (this.Coins.Count == 0)
            {
                this.Stop();
            }
        }

        private void EatEnnemy(Position p)
        {
            // Remove from world map
            this.Values[p.Y * SIZE + p.X] = 0;

            // Remove from coin list
            var ennemy = this.Ennemies.First(c => c.Position.X == p.X && c.Position.Y == p.Y);
            this.Ennemies.Remove(ennemy);

            // Remove from render
            GameObject.Renderer.Remove(ennemy);

            this.Score += 20;

            if (this.Coins.Count == 0)
            {
                this.Stop();
            }
        }

        internal bool CanGoTo(Position p)
        {
            return this.Values[p.Y * SIZE + p.X] != 1;
        }
        internal bool CanGo(PlayAction action)
        {
            Position p = new Position(Pacman.Position);
            switch (action)
            {
                case PlayAction.Up:
                    p.Y -= 1;
                    break;
                case PlayAction.Down:
                    p.Y += 1;
                    break;
                case PlayAction.Left:
                    p.X -= 1;
                    break;
                case PlayAction.Right:
                    p.X += 1;
                    break;
            }
            return this.Values[p.Y * SIZE + p.X] != 1;
        }
    };
}
