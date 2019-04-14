using System;
using System.Collections.Generic;
using System.Linq;

namespace ML.NET.App.PacMan.Agents.Graph
{
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

        public List<INode<T>> FindShortestRoute(T start, T end)
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

            return FindShortestRoute(startNode, endNode);
        }

        public List<INode<T>> FindShortestRoute(INode<T> startNode, INode<T> endNode)
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
                var path = FindShortestRoute(n, endNode);
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