using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML.NET.App.PacMan.Agents.Graph;
using ML.NET.App.PacMan.Model;

namespace GraphUnitTest
{
    [TestClass]
    public class GraphUnitTest
    {
        [TestMethod]
        public void CreateGraphTest()
        {
            int size = 3;
            var graph = new Graph<Position>();

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                {
                    var p1 = new Position(i, j);
                    if (i < size - 2)
                    {
                        var p2 = new Position(i + 1, j);
                        graph.AddEdge(p1, p2);
                    }
                    if (j < size - 2)
                    {
                        var p3 = new Position(i, j + 1);
                        graph.AddEdge(p1, p3);
                    }
                }
            }
            Assert.AreEqual(size * size, graph.Nodes.Count);
        }
    }
}
