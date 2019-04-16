using ML.NET.App.PacMan.Model;
using ML.NET.App.PacMan.Agents.Graph;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;

namespace ML.NET.App.PacMan.View
{
    public class GameRenderer
    {
        private Canvas canvas;

        public const int SPRITE_SIZE = 27;

        public GameRenderer(Canvas canvas)
        {
            this.canvas = canvas;
        }
        double width = 11;
        internal void DrawGraph(Graph<Position> graph)
        {
            foreach (var wall in World.Instance.Walls)
            {
                this.Draw(wall);
            }
            double offset = (-width + SPRITE_SIZE) / 2.0;
            foreach (var n in graph.Nodes)
            {
                n.IsVisited = false;
                var ellipse = new Ellipse() { Width = width, Height = width, Fill = Brushes.White };
                Canvas.SetLeft(ellipse, n.Value.X * SPRITE_SIZE + offset);
                Canvas.SetTop(ellipse, n.Value.Y * SPRITE_SIZE + offset);
                this.canvas.Children.Add(ellipse);
            }
            DrawEdges(graph.Nodes.First());
        }
        internal void DrawEdges(INode<Position> node)
        {
            if (node.IsVisited)
                return;
            node.IsVisited = true;
            double x1 = node.Value.X * SPRITE_SIZE + SPRITE_SIZE / 2, y1 = node.Value.Y * SPRITE_SIZE + SPRITE_SIZE / 2;
            foreach (var n in node.Nodes)
            {
                double x2 = n.Value.X * SPRITE_SIZE + SPRITE_SIZE / 2, y2 = n.Value.Y * SPRITE_SIZE + SPRITE_SIZE / 2;
                var line = new Line() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.White, StrokeThickness = 2 };
                this.canvas.Children.Add(line);
                if (!n.IsVisited)
                    DrawEdges(n);
            }
        }
        List<Line> path = new List<Line>();
        internal void DrawPath(IEnumerable<INode<Position>> nodes)
        {
            // Clear path
            foreach(var e in path)
            {
                this.canvas.Children.Remove(e);
            }
            path.Clear();

            var node1 = nodes.ElementAt(0);
            for (int i = 1; i < nodes.Count(); i++)
            {
                var node2 = nodes.ElementAt(i);

                double x1 = node1.Value.X * SPRITE_SIZE + SPRITE_SIZE / 2, y1 = node1.Value.Y * SPRITE_SIZE + SPRITE_SIZE / 2;
                double x2 = node2.Value.X * SPRITE_SIZE + SPRITE_SIZE / 2, y2 = node2.Value.Y * SPRITE_SIZE + SPRITE_SIZE / 2;

                var line = new Line() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.White, StrokeThickness = 2};
                this.canvas.Children.Add(line);
                path.Add(line);

                node1 = node2;
            }
        }

        internal void DrawWorld(World world)
        {
            foreach (var wall in world.Walls)
            {
                this.Draw(wall);
            }
            foreach (var coin in world.Coins)
            {
                this.Draw(coin);
            }
            foreach (var ennemy in world.Ennemies)
            {
                this.Draw(ennemy);
            }
            this.Draw(world.Pacman);
        }
        public void Clear()
        {
            this.canvas.Children.Clear();
        }
        public void Draw(IGameObject go)
        {
            Canvas.SetLeft(go.Element, go.Position.X * SPRITE_SIZE);
            Canvas.SetTop(go.Element, go.Position.Y * SPRITE_SIZE);
            this.canvas.Children.Add(go.Element);
        }
        public void Move(IGameObject go)
        {
            Canvas.SetLeft(go.Element, go.Position.X * SPRITE_SIZE);
            Canvas.SetTop(go.Element, go.Position.Y * SPRITE_SIZE);
        }
        internal void Remove(IGameObject gameObject)
        {
            this.canvas.Children.Remove(gameObject.Element);
        }
    }
}
