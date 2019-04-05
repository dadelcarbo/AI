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

        public static Image ParseCoin()
        {
            var coinImage = new Image();
            var uri = new Uri(coinPath, UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            coinImage.Source = bitmap;
            return coinImage;
        }
        public static Image ParsePacman()
        {
            var pacmanImage = new Image();
            var uri = new Uri(pacmanPath, UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            pacmanImage.Source = bitmap;
            return pacmanImage;
        }
        public static Image ParseWall()
        {
            var wallImage = new Image();
            var uri = new Uri(wallPath, UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            wallImage.Source = bitmap;
            return wallImage;
        }

    }
}
