using ML.NET.App.PacMan.View;
using System.Windows;

namespace ML.NET.App.PacMan.Model
{
    public abstract class GameObject : IGameObject
    {
        public const int VoidId = 0;
        public const int WallId = 1;
        public const int CoinId = 2;
        public const int EnnemyId = 3;

        public static World World { get; set; }
        public static GameRenderer Renderer { get; set; }
        public Position Position { get; set; }
        public UIElement Element { get; protected set; }
        public abstract int ClassId { get; }
    }
}
