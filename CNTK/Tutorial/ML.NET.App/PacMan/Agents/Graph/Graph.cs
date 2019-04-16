using System;
using System.Collections.Generic;
using System.Linq;

namespace ML.NET.App.PacMan.Agents.Graph
{
    public enum SearchAlgorithm
    {
        Dijkstra,
        Greedy
    }
    public class Graph<T>
    {
        public List<INode<T>> Nodes { get; set; }

        public Graph()
        {
            this.Nodes = new List<INode<T>>();
        }

        public void AddEdge(T value1, T value2)
        {
            var node1 = this.Nodes.FirstOrDefault(n => n.Value.Equals(value1));
            if (node1 == null)
            {
                node1 = new Node<T>(value1);
                this.Nodes.Add(node1);
            }

            var node2 = this.Nodes.FirstOrDefault(n => n.Value.Equals(value2));
            if (node2 == null)
            {
                node2 = new Node<T>(value2);
                this.Nodes.Add(node2);
            }

            node1.Nodes.Add(node2);
            node2.Nodes.Add(node1);
        }

        public List<INode<T>> FindShortestRoute(T start, T end, SearchAlgorithm algo)
        {
            // Reset Visited 
            foreach (var n in this.Nodes)
            {
                n.IsVisited = false;
            }

            var startNode = this.Nodes.FirstOrDefault(n => n.Value.Equals(start));
            if (startNode == null)
            {
                throw new ArgumentException("Start node not found !");
            }
            var endNode = this.Nodes.FirstOrDefault(n => n.Value.Equals(end));
            if (endNode == null)
            {
                throw new ArgumentException("Start node not found !");
            }
            switch (algo)
            {
                case SearchAlgorithm.Dijkstra:
                    return FindShortestRoute_Dijkstra(startNode, endNode);
                case SearchAlgorithm.Greedy:

                    var route = new Stack<INode<T>>();
                    route.Push(startNode);

                    bool found = FindShortestRoute_Greedy(endNode, route);

                    return found ? shortestRoute.ToList() : null;
                default:
                    throw new NotImplementedException($"Algorithm {algo} is not implemented");
            }

        }

        public List<INode<T>> FindShortestRoute_Dijkstra(INode<T> startNode, INode<T> endNode)
        {
            startNode.IsVisited = true;
            var routes = new List<List<INode<T>>>() { new List<INode<T>>() { startNode } };
            var newRoutes = new List<List<INode<T>>>();
            bool found = false, possible = true;
            while (!found && possible)
            {
                newRoutes.Clear();
                possible = false;
                foreach (var route in routes)
                {
                    var node = route.Last();
                    foreach (var n in node.Nodes)
                    {
                        if (!n.IsVisited)
                        {
                            var newRoute = new List<INode<T>>(route);
                            newRoute.Add(n);
                            if (n == endNode)
                            {
                                return newRoute;
                            }
                            possible = true;
                            node.IsVisited = true;
                            newRoutes.Add(newRoute);
                        }
                    }
                }
                routes.Clear();
                routes.AddRange(newRoutes);
            }
            return null;
        }

        List<INode<T>> shortestRoute = null;

        public bool FindShortestRoute_Greedy(INode<T> endNode, Stack<INode<T>> route)
        {
            bool found = false;

            INode<T> startNode = route.Peek();
            found = false;
            if (startNode.Nodes.Contains(endNode))
            {
                found = true;
                route.Push(endNode);
                return true;
            }
            foreach (var n in startNode.Nodes)
            {
                if (!route.Contains(n) && (shortestRoute == null || shortestRoute.Count > route.Count))
                {
                    route.Push(n);
                    if (FindShortestRoute_Greedy(endNode, route))
                    {
                        if (shortestRoute == null || shortestRoute.Count > route.Count)
                        {
                            shortestRoute = route.ToList();
                            found = true;
                        }
                    }
                    route.Pop();
                }
            }
            return found;
        }

        public List<INode<T>> FindShortestRoute2(INode<T> startNode, INode<T> endNode)
        {
            startNode.IsVisited = true;
            bool allVisited = true;
            foreach (var n in startNode.Nodes.Where(n => !n.IsVisited))
            {
                if (n == endNode)
                {
                    return new List<INode<T>>() { startNode, endNode };
                }
                allVisited = false;
            }

            if (allVisited)
                return null;

            List<INode<T>> shortestPath = null;
            foreach (var n in startNode.Nodes)
            {
                if (n.IsVisited)
                    continue;
                var path = FindShortestRoute2(n, endNode);
                if (path == null)
                    continue;
                if (shortestPath == null || shortestPath.Count > path.Count)
                {
                    shortestPath = path;
                }
                if (shortestPath.Count == 2) // Cannot be shorter stop
                    break;
            }
            if (shortestPath == null)
                return null;
            shortestPath.Insert(0, startNode);
            return shortestPath;
        }
    }
}