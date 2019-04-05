
using System.Windows;

namespace ML.NET.App.PacMan.Model
{
    public interface IGameObject
    {
        Position Position { get; }
        UIElement Element { get; }
        int ClassId { get; }
    }
}
