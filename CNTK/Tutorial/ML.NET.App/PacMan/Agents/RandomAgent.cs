using System;
using System.Linq;
using ML.NET.App.PacMan.Model;

namespace ML.NET.App.PacMan.Agents
{
    public class RandomAgent : IAgent
    {
        static Random rnd = new Random();
        public string Name => "Random Agent";

        static PlayAction[] actions = Enum.GetValues(typeof(PlayAction)).Cast<PlayAction>().ToArray();

        public PlayAction Decide(World world)
        {
            return GetRandomAction();
        }

        static public PlayAction GetRandomAction()
        {
            return actions[rnd.Next(actions.Length)];
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}
