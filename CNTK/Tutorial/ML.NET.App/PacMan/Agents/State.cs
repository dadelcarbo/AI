using CNTK;
using ML.NET.App.PacMan.Model;
using System.Diagnostics;

namespace ML.NET.App.PacMan.Agents
{
    [DebuggerDisplay("Reward={Reward} Action={Action}")]
    class State
    {
        public float[] Value;
        public float Reward;
        public PlayAction Action;
        public float[] ExpectedActions;
    }
}
