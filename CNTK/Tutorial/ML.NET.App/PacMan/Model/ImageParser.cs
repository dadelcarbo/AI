using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ML.NET.App.PacMan.Model
{
    public static class ImageParser
    {
        public static Image Parse(string imagePath)
        {
            var image = new Image();
            var uri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            image.Source = bitmap;
            return image;
        }

        private static string coinPath = @"PacMan\Images\Coin.png";
        private static string pacmanPath = @"PacMan\Images\PacMan.png";
        private static string wallPath = @"PacMan\Images\Wall.png";
        private static string ennemyPath = @"PacMan\Images\Ennemy.png";
        public static Image ParseEnnemy()
        {
            return Parse(ennemyPath);
        }

        public static Image ParseCoin()
        {
            return Parse(coinPath);
        }
        public static Image ParsePacman()
        {
            return Parse(pacmanPath);
        }
        public static Image ParseWall()
        {
            return Parse(wallPath);
        }

    }
}
