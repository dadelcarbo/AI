using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MNIST.ML.NET.DataUtils
{
    public class MNISTImage
    {
        private static int count = 0;
        public MNISTImage()
        {
            Id = count++;
        }

        public int Id { get; set; }

        public string Name { get { return Id + "-" + Label; } }
        public byte Label { get; set; }
        public byte[,] Data { get; set; }

        double[] values;

        public double[] Values
        {
            get
            {
                if (values == null)
                {
                    values = new double[Data.GetLength(0) * Data.GetLength(1)];
                    for (int i = 0; i < Data.GetLength(0); i++)
                    {
                        for (int j = 0; j < Data.GetLength(1); j++)
                        {
                            values[i * Data.GetLength(0) + j] = Data[i, j];
                        }
                    }
                }
                return values;
            }
        }
        public BitmapSource Bitmap { get; set; }

        public static BitmapSource FromArray(byte[] data, int w = 28, int h = 28)
        {
            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, PixelFormats.Gray8, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, w, 0);

            return wbm;
        }

        private MNISTData mnistData;
        public MNISTData MNISTData
        {
            get
            {
                if (mnistData == null)
                {
                    mnistData = new MNISTData
                    {
                        Feature = Label,
                        Values = Values
                    };
                }
                return mnistData;
            }
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
