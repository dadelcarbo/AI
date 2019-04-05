﻿using ML.NET.App.PacMan.View;
using System.Windows;

namespace ML.NET.App.PacMan.Model
{
    public abstract class GameObject : IGameObject
    {
        public static World World { get; set; }
        public static GameRenderer Renderer { get; set; }
        public Position Position { get; protected set; }
        public UIElement Element { get; protected set; }
        public abstract int ClassId { get; }
    }
}