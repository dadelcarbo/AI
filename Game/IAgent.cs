namespace Game
{
    public interface IAgent
    {
        string Name { get; }
        Model Model { get; set; }

        Action Decide();

        void OnDead(); 
    }
}