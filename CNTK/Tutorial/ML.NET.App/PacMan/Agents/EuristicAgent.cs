using System;
using System.Linq;
using ML.NET.App.PacMan.Model;

namespace ML.NET.App.PacMan.Agents
{
    public class EuristicAgent : IAgent
    {
        public string Name => "Euristic Agent";

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public PlayAction Decide(World world)
        {
            // Find closest coin
            if (world.Coins.Count == 0)
                return PlayAction.NOP;

            var coin = world.Coins.Select(c => new { c, Dist = (world.Pacman.Position.X - c.Position.X) * (world.Pacman.Position.X - c.Position.X) + (world.Pacman.Position.Y - c.Position.Y) * (world.Pacman.Position.Y - c.Position.Y) }).OrderBy(c => c.Dist).First().c;

            var dx = coin.Position.X - world.Pacman.Position.X;
            var dy = coin.Position.Y - world.Pacman.Position.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                return dx > 0 ? PlayAction.Right : PlayAction.Left;
            }
            else
            {
                return dy > 0 ? PlayAction.Down : PlayAction.Up;
            }
        }
    }
}
