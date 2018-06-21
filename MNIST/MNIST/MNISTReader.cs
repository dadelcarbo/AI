using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MNIST
{
    public class Image
    {
        private static int count = 0;
        public Image()
        {
            Id = count++;
        }

        public int Id { get; set; }

        public string Name { get { return Id + "-" + Label; } }
        public byte Label { get; set; }
        public byte[,] Data { get; set; }
        public BitmapSource Bitmap { get; set; }

        public static BitmapSource FromArray(byte[] data, int w = 28, int h = 28)
        {
            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, PixelFormats.Gray8, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, w, 0);

            return wbm;
        }
    }

    public static class Extensions
    {
       

        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (int w = 0; w < source.GetLength(0); w++)
            {
                for (int h = 0; h < source.GetLength(1); h++)
                {
                    action(w, h);
                }
            }
        }
    }
}
