namespace ML.NET.App.PacMan.Model
{
    public class PacManGame
    {
        public World World { get; set; }

        public PacManGame()
        {
            this.World = new World();
        }
    }
}
