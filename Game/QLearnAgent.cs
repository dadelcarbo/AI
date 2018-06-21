using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class QLearnAgent : AgentBase
    {
        public override string Name => "QLearner";

        private double[] QLeft;
        private double[] QRight;
        private double[] QNop;

        public QLearnAgent(Model model) : base(model)
        {
            QLeft = new double[3];
            QRight = new double[3];
            QNop = new double[3];
        }

        private Random rnd = new Random();

        private Action previousAction = Action.NOP;
        private double reward = 0;

        public override Action Decide()
        {
            if (!Model.IsLearning)
            {
                var line = Model.Environment.Terrain[Environment.Height - 2].Select(b => b == Body.Void ? 0.0 : b == Body.Good ? 1 : -1).ToArray();
                double right = CalculateAction(Model.PlayerPosition, line, QRight);
                double left = CalculateAction(Model.PlayerPosition, line, QLeft);
                double nop = CalculateAction(Model.PlayerPosition, line, QNop);

                if (right > Math.Max(nop, left)) return Action.Right;
                if (left > Math.Max(nop, right)) return Action.Left;
                return Action.NOP;
            }
            else
            {
                int random = rnd.Next(100);
                var action = random > 90 ? Action.Right : random < 10 ? Action.Left : Action.NOP;
                if (action != Action.NOP) return action;

                var line = Model.Environment.Terrain[Environment.Height - 2].Select(b => b == Body.Void ? 0.0 : b == Body.Good ? 1 : -1).ToArray();
                double right = CalculateAction(Model.PlayerPosition, line, QRight);
                double left = CalculateAction(Model.PlayerPosition, line, QLeft);
                double nop = CalculateAction(Model.PlayerPosition, line, QNop);

                if (right > Math.Max(nop, left)) return Action.Right;
                if (left > Math.Max(nop, right)) return Action.Left;
                return Action.NOP;
            }
        }

        private static double CalculateAction(int playerPosition, double[] line, double[] Q)
        {
            double action;
            if (playerPosition < 1)
            {
                action = line[0] * Q[1] + line[1] * Q[2];
            }
            else if (playerPosition >= Environment.Width - 1)
            {

                action = line[Environment.Width - 2] * Q[0] + line[Environment.Width - 1] * Q[1];
            }
            else
            {
                action = line[playerPosition - 1] * Q[0] + line[playerPosition] * Q[1] + line[playerPosition] * Q[2];
            }

            return action;
        }

        public override void OnDead()
        {
            if (!Model.IsLearning) return;

            switch (previousAction)
            {
                case Action.NOP:
                    QNop[1]--;
                    break;
                case Action.Right:
                    QRight[2]--;
                    break;
                case Action.Left:
                    QLeft[2]--;
                    break;
                default:
                    break;
            }
        }
    }
}
