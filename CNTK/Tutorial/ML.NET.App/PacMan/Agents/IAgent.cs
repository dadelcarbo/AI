using ML.NET.App.PacMan.Model;

namespace ML.NET.App.PacMan.Agents
{
    public interface IAgent
    {
        string Name { get; }

        PlayAction Decide(World world);

        void Activate();
        void Deactivate();
    }
}
