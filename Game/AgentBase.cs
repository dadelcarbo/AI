using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class AgentBase : IAgent
    {
        public AgentBase(Model model)
        {
            this.Model = model;
        }
        public abstract string Name { get; }
        public Model Model { get; set; }

        public abstract Action Decide();
        public virtual void OnDead() { }
    }
}
