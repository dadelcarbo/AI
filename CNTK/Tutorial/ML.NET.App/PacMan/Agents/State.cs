using CNTK;
using ML.NET.App.PacMan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.NET.App.PacMan.Agents
{
    struct State
    {
        public Value Value;
        public int Reward;
        public PlayAction Action;
    }
}
