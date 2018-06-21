using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Game.Annotations;

namespace Game
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model model;
        public ViewModel()
        {
            this.model = Model.Current;
            this.model.PropertyChanged += Current_PropertyChanged;
            this.model.Environment.OnTerrainUpdated += Environment_OnTerrainUpdated;

            this.Environment_OnTerrainUpdated();

            this.Agent = this.Agents.First();
        }

        private void Environment_OnTerrainUpdated()
        {
            var text = this.model.Environment.ToString();

            StringBuilder sb = new StringBuilder(text);
            sb[text.Length - Environment.Width + this.model.PlayerPosition] = 'M';

            this.Terrain = sb.ToString();
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName=="Score")
            {
                this.NotifyPropertyChanged(e.PropertyName);
            }
        }
        
        private string terrain;
        public string Terrain { get { return terrain; } set { if (value != terrain) { terrain = value; NotifyPropertyChanged(); } } }

        public int Score { get { return model.Score; }}

        private List<IAgent> agents;
        public List<IAgent> Agents
        {
            get
            {
                if (agents == null)
                {
                    agents = new List<IAgent>();
                    agents.Add(new ManualAgent(this.model));
                    agents.Add(new RandomAgent(this.model));
                    agents.Add(new DirectAgent(this.model));
                    agents.Add(new QLearnAgent(this.model));
                }
                return agents;
            }
        }

        public IAgent Agent { get { return model.Agent; } set { if (value != model.Agent) { model.Agent = value; NotifyPropertyChanged(); } } }

        public bool IsLearning { get { return model.IsLearning; } set { if (value != model.IsLearning) { model.IsLearning = value; NotifyPropertyChanged(); } } }

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
