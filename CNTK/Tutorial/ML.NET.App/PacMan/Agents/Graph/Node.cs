using System.Collections.Generic;
using System.Diagnostics;

namespace ML.NET.App.PacMan.Agents.Graph
{

    [DebuggerDisplay("Value={Value} IsVisited={IsVisited}")]
    public class Node<T> : INode<T>
    {
        public List<INode<T>> Nodes { get; private set;}
        public bool IsVisited { get; set; }
        public T Value { get; private set; }

        public Node(T value)
        {
            this.Nodes = new List<INode<T>>();
            this.Value = value;
        }
    }
}
