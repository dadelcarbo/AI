﻿namespace ML.NET.App.PacMan.Model
{
    public class Pacman : GameObject, IGameObject
    {
        public override int ClassId => PacmanId;
        public Pacman(Position position)
        {
            this.Position = position;
            this.Element = ImageParser.ParsePacman();
        }

        public void PerformAction(PlayAction action)
        {
            Position newPosition = new Position(this.Position);
            switch (action)
            {
                case PlayAction.Up:
                    newPosition.Y -= 1;
                    break;
                case PlayAction.Down:
                    newPosition.Y += 1;
                    break;
                case PlayAction.Left:
                    newPosition.X -= 1;
                    break;
                case PlayAction.Right:
                    newPosition.X += 1;
                    break;
                case PlayAction.NOP:
                    World.Instance.Score -= 1;
                    break;
            }
            if (World.CanGoTo(newPosition))
            {
                World.GoTo(newPosition);
            }
            else
            {
                World.Instance.Score-=1;
            }
        }

        internal void GoTo(Position p)
        {
            this.Position = p;
            Renderer.Move(this);
        }
    }
}
