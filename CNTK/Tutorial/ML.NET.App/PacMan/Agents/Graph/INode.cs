using System.Collections.Generic;

namespace ML.NET.App.PacMan.Agents.Graph
{
    public interface INode<T>
    {
        List<INode<T>> Nodes { get; }

        bool IsVisited { get; set; }

        T Value { get; }
    }
}