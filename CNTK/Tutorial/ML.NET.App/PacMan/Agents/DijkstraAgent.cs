using ML.NET.App.PacMan.Agents.Graph;
using ML.NET.App.PacMan.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ML.NET.App.PacMan.Agents
{
    public class DijkstraAgent : IAgent
    {
        public string Name => "Dijkstra Agent";

        public Graph<Position> Graph { get; set; }

        public void Activate()
        {
            // Build graph
            Graph = new Graph<Position>();
            var values = World.Instance.Values;
            for (int i = 1; i < World.SIZE - 1; i++)
            {
                for (int j = 1; j < World.SIZE - 1; j++)
                {
                    if (values[i * World.SIZE + j] == 1) continue;

                    var p1 = new Position(j, i);
                    if (values[i * World.SIZE + j] != 1)
                    {
                        var p2 = new Position(j, i + 1);
                        Graph.AddEdge(p1, p2);
                    }
                    if (values[i * World.SIZE + j] != 1)
                    {
                        var p3 = new Position(j + 1, i);
                        Graph.AddEdge(p1, p3);
                    }
                }
            }
        }

        public void Deactivate()
        {
        }

        private Queue<INode<Position>> path = null;

        public PlayAction Decide(World world)
        {
            PlayAction action = PlayAction.NOP;

            // Find closest coin
            if (world.Coins.Count == 0)
                return action;

            if (path == null)
            {

                var coin = world.Coins.Select(c => new { c, Dist = (world.Pacman.Position.X - c.Position.X) * (world.Pacman.Position.X - c.Position.X) + (world.Pacman.Position.Y - c.Position.Y) * (world.Pacman.Position.Y - c.Position.Y) }).OrderBy(c => c.Dist).First().c;

                var dx = coin.Position.X - world.Pacman.Position.X;
                var dy = coin.Position.Y - world.Pacman.Position.Y;

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
                    var nodes = Graph.FindShortestRoute(world.Pacman.Position, coin.Position, SearchAlgorithm.Dijkstra);
                    if (nodes == null)
                    {
                        return RandomAgent.GetRandomAction();
                    }

                    nodes.RemoveAt(0);
                    path = new Queue<INode<Position>>(nodes);
                    var node = path.Dequeue();

                    if (path.Count == 0)
                    {
                        path = null;
                    }
                    else
                    {
                        GameObject.Renderer.DrawPath(path);
                    }

                    if (world.Pacman.Position.X - node.Value.X > 0)
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
            }
            else
            {
                var node = path.Dequeue();

                if (path.Count == 0)
                {
                    path = null;
                }
                else
                {
                    GameObject.Renderer.DrawPath(path);
                }

                if (world.Pacman.Position.X - node.Value.X > 0)
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
