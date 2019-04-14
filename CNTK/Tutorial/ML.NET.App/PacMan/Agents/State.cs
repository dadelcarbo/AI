using CNTK;
using ML.NET.App.PacMan.Model;
using System.Diagnostics;

namespace ML.NET.App.PacMan.Agents
{
    [DebuggerDisplay("Reward={Reward} Action={Action}")]
    struct State
    {
        public Value Value;
        public int Reward;
        public PlayAction Action;
    }
}
