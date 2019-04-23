namespace ML.NET.App.PacMan.Model
{
    public class Void : GameObject, IGameObject
    {
        public override int ClassId => VoidId;
        public Void(Position position)
        {
            this.Position = position;
        }
    }
}
