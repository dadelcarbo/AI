﻿using ML.NET.App.PacMan.Model;
using System.Windows.Controls;

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
            this.Draw(world.Pacman);
        }

        public void Clear()
        {
            this.canvas.Children.Clear();
        }

        public void Draw(GameObject go)
        {
            Canvas.SetLeft(go.Element, go.Position.X * SPRITE_SIZE);
            Canvas.SetTop(go.Element, go.Position.Y * SPRITE_SIZE);
            this.canvas.Children.Add(go.Element);
        }
        public void Move(GameObject go)
        {
            Canvas.SetLeft(go.Element, go.Position.X * SPRITE_SIZE);
            Canvas.SetTop(go.Element, go.Position.Y * SPRITE_SIZE);
        }

        internal void Remove(Coin coin)
        {
            this.canvas.Children.Remove(coin.Element);
        }
    }
}
