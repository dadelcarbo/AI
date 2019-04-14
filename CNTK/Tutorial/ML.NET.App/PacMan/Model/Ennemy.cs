namespace ML.NET.App.PacMan.Model
{
    public class Ennemy : GameObject, IGameObject
    {
        public override int ClassId => EnnemyId;

        public Ennemy(Position position)
        {
            this.Position = position;
            this.Element = ImageParser.ParseEnnemy();
        }
    }
}
