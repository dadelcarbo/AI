namespace ML.NET.App.PacMan.Model
{
    public class Void : GameObject, IGameObject
    {
        public override int ClassId => 0;
        public Void(Position position)
        {
            this.Position = position;
        }
    }
}
