using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.NET.App.PacMan.Agents.Graph
{
    public class Link<T>
    {
        INode<T> Start { get; set; }
        INode<T> End { get; set; }
        double Distance { get; set; }
    }
}
