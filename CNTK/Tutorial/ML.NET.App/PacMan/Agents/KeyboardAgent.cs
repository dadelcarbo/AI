using System.Collections.Generic;
using ML.NET.App.PacMan.Model;

namespace ML.NET.App.PacMan.Agents
{
    public class KeyboardAgent : IAgent
    {
        public string Name => "Keyboard Agent";

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        Queue<PlayAction> actions = new Queue<PlayAction>();

        public void AddMove(PlayAction action)
        {
            if (actions.Count == 0 || actions.Peek() != action)
                actions.Enqueue(action);
        }

        public PlayAction Decide(World world)
        {
            if (actions.Count == 0) return PlayAction.NOP;

            return actions.Dequeue();
        }
    }
}
