﻿namespace ML.NET.App.PacMan.Model
{
    public class Coin : GameObject, IGameObject
    {
        public override int ClassId => CoinId;

        public Coin(Position position)
        {
            this.Position = position;
            this.Element = ImageParser.ParseCoin();
        }
    }
}
