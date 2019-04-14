using ML.NET.App.PacMan.Agents.Graph;
using ML.NET.App.PacMan.Model;
using System;
using System.Linq;

namespace ML.NET.App.PacMan.Agents
{
    public class DijkstraAgent : IAgent
    {
        public string Name => "Dijkstra Agent";

        private Graph<Position> graph;

        public void Activate()
        {
            // Build graph
            graph = new Graph<Position>();
            var values = World.Instance.Values;
            for (int i = 1; i < World.SIZE - 1; i++)
            {
                for (int j = 1; j < World.SIZE - 1; j++)
                {
                    if (values[i, j] == 1) continue;

                    var p1 = new Position(j, i);
                    if (values[i + 1, j] != 1)
                    {
                        var p2 = new Position(j, i + 1);
                        graph.AddEdge(p1, p2);
                    }
                    if (values[i, j + 1] != 1)
                    {
                        var p3 = new Position(j + 1, i);
                        graph.AddEdge(p1, p3);
                    }
                }
            }
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

            PlayAction action = PlayAction.NOP;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                action = dx > 0 ? PlayAction.Right : PlayAction.Left;
            }
            else
            {
                action = dy > 0 ? PlayAction.Down : PlayAction.Up;
            }

            if (world.CanGo(action))
            {
                return action;
            }
            else
            {
                // Find best route using graph
                var nodes = graph.FindShortestRoute(world.Pacman.Position, coin.Position);
                if (nodes==null)
                {
                    return RandomAgent.GetRandomAction();
                }
                var node = nodes.ElementAt(1);
                if(world.Pacman.Position.X - node.Value.X > 0)
                {
                    return PlayAction.Left;
                }
                if (world.Pacman.Position.X - node.Value.X < 0)
                {
                    return PlayAction.Right;
                }
                if (world.Pacman.Position.Y - node.Value.Y > 0)
                {
                    return PlayAction.Up;
                }
                if (world.Pacman.Position.Y - node.Value.Y < 0)
                {
                    return PlayAction.Down;
                }
            }
            return action;
        }
    }
}
