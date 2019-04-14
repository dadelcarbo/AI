namespace ML.NET.App.PacMan.Model
{
    public class Wall : GameObject, IGameObject
    {
        public override int ClassId => WallId;
        public Wall(Position position)
        {
            this.Position = position;
            this.Element = ImageParser.ParseWall();
        }
    }
}
