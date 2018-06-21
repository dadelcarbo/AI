using Game.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game
{
    public class Model : INotifyPropertyChanged
    {
        static Model model;
        static public Model Current
        {
            get
            {
                if (model == null)
                {
                    model = new Model();
                }
                return model;
            }
        }

        public delegate void DeadHandler();

        public event DeadHandler OnDead;

        public Environment Environment { get; set; }

        public int PlayerPosition { get; set; }

        private int score;
        public int Score { get { return score; } set { if (value != score) { score = value; NotifyPropertyChanged(); } } }

        public bool IsStarted { get; set; }

        public bool IsLearning { get; set; }

        public IAgent Agent { get; set; }

        public Model()
        {
            this.Environment = new Environment();
            this.PlayerPosition = Environment.Width / 2;
        }

        //private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Agent")
        //    {
        //        if (this.Agent != null)
        //            this.OnDead -= this.Agent.OnDead;

        //        this.Agent = this.viewModel.Agent;
        //        this.OnDead += this.Agent.OnDead;
        //    }
        //}

        public void Update()
        {
            switch (this.Agent.Decide())
            {
                case Action.Left:
                    MoveLeft();
                    break;
                case Action.Right:
                    MoveRight();
                    break;
            }

            Environment.Update();

            switch (Environment.GetPlayerBody(PlayerPosition))
            {
                case Body.Void:
                    break;
                case Body.Good:
                    Score++;
                    break;
                case Body.Bad:
                    this.Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void MoveRight()
        {
            this.PlayerPosition = Math.Min(Environment.Width - 1, this.PlayerPosition + 1);

            switch (Environment.GetPlayerBody(PlayerPosition))
            {
                case Body.Void:
                    break;
                case Body.Good:
                    Score++;
                    break;
                case Body.Bad:
                    this.Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void MoveLeft()
        {
            this.PlayerPosition = Math.Max(0, this.PlayerPosition - 1);

            switch (Environment.GetPlayerBody(PlayerPosition))
            {
                case Body.Void:
                    break;
                case Body.Good:
                    Score++;
                    break;
                case Body.Bad:
                    this.Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Die()
        {
            this.OnDead?.Invoke();
        }

        public void Start()
        {
            this.Score = 0;
            this.PlayerPosition = Environment.Width / 2;
            this.Environment.Initialize();

            this.IsStarted = true;
        }

        public void Stop()
        {
            this.IsStarted = false;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
