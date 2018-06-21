using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class RandomAgent : AgentBase
    {
        public RandomAgent(Model model):base(model)
        {

        }
        public override string Name => "Random";

        private Random rnd = new Random();
        public override Action Decide()
        {
            int random = rnd.Next(100);
            return random > 90 ? Action.Right : random < 10 ? Action.Left : Action.NOP;
        }
    }
}
