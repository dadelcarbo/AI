using Game.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Game
{
    public enum Body
    {
        Void = 0,
        Good,
        Bad
    }
    public class Environment : INotifyPropertyChanged
    {
        public delegate void TerrainUpdatedHandler();
        public event TerrainUpdatedHandler OnTerrainUpdated;

        public Body[][] Terrain { get; set; }

        public static int Width => 10;
        public static int Height => 10;

        private Random rnd = new Random();

        public Environment()
        {
            this.Initialize();
        }

        private static byte ratio = 5;
        private Body[] CreateLine()
        {
            byte[] numbers = new byte[Width];
            rnd.NextBytes(numbers);

            return numbers.Select(n => n < ratio ? Body.Bad : n > (255 - ratio) ? Body.Good : Body.Void).ToArray();
        }

        public override string ToString()
        {
            var text = Terrain.Select(
                line => line.Select(a => a == Body.Bad ? "X" : a == Body.Good ? "O" : " ").Aggregate((a, b) => a + b)).Aggregate((a, b) => a + System.Environment.NewLine + b);
            
            return text;
        }

        public void Update()
        {
            for (int i = Height-1; i > 0; i--)
            {
                Terrain[i] = Terrain[i - 1];
            }
            Terrain[0] = CreateLine();

            this.OnTerrainUpdated?.Invoke();
        }

        public Body GetPlayerBody(int playerPosition)
        {
            return Terrain.Last()[playerPosition];
        }

        public void Initialize()
        {
            Terrain = new Body[Height][];
            for (var i = 0; i < Height; i++)
            {
                Terrain[i] = new Body[Width];
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
