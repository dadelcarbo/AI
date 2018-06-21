using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class DirectAgent : AgentBase
    {
        public DirectAgent(Model model) : base(model)
        {

        }
        public override string Name => "Direct";

        private Random rnd = new Random();
        public override Action Decide()
        {
            int viewIndex = Environment.Height - 2;
            if (Model.PlayerPosition < Environment.Width / 2)
            {
                if (Model.Environment.Terrain[viewIndex][Model.PlayerPosition] == Body.Bad)
                {
                    return Action.Right;
                }
            }
            else
            {
                if (Model.Environment.Terrain[viewIndex][Model.PlayerPosition] == Body.Bad)
                {
                    return Action.Left;
                }
            }
            if (Model.PlayerPosition > 0 && Model.PlayerPosition < Environment.Width - 1)
            {
                if (Model.Environment.Terrain[viewIndex][Model.PlayerPosition - 1] == Body.Good)
                {
                    return Action.Left;
                }
                if (Model.Environment.Terrain[viewIndex][Model.PlayerPosition + 1] == Body.Good)
                {
                    return Action.Right;
                }
            }
            if (Model.PlayerPosition > 1 && Model.PlayerPosition < Environment.Width - 2)
            {
                if (Model.Environment.Terrain[viewIndex - 1][Model.PlayerPosition - 2] == Body.Good)
                {
                    return Action.Left;
                }
                if (Model.Environment.Terrain[viewIndex - 1][Model.PlayerPosition + 2] == Body.Good)
                {
                    return Action.Right;
                }
            }

            return Action.NOP;
        }
    }
}
