using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataUtils
{
    public static class MnistReader
    {
        public static string RootPath = @"C:\AI\MNIST";
        public const string TrainImages = "train-images.idx3-ubyte";
        public const string TrainLabels = "train-labels.idx1-ubyte";
        public const string TestImages = "t10k-images.idx3-ubyte";
        public const string TestLabels = "t10k-labels.idx1-ubyte";

        public static IEnumerable<MNISTImage> ReadTrainingData()
        {
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<MNISTImage> ReadTestData()
        {
            foreach (var item in Read(TestImages, TestLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<MNISTImage> Read(string imagesPath, string labelsPath)
        {
            Console.WriteLine(Path.Combine(RootPath, imagesPath));
            BinaryReader labels = new BinaryReader(new FileStream(Path.Combine(RootPath, labelsPath), FileMode.Open));
            BinaryReader images = new BinaryReader(new FileStream(Path.Combine(RootPath, imagesPath), FileMode.Open));

            int magicImages = images.ReadBigInt32();
            int numberOfImages = images.ReadBigInt32();
            int width = images.ReadBigInt32();
            int height = images.ReadBigInt32();

            int magicLabel = labels.ReadBigInt32();
            int numberOfLabels = labels.ReadBigInt32();

            for (int i = 0; i < numberOfImages; i++)
            {
                var bytes = images.ReadBytes(width * height);
                var arr = new byte[height, width];

                arr.ForEach((j, k) => arr[j, k] = bytes[j * height + k]);

                yield return new MNISTImage()
                {
                    Data = arr,
                    Label = labels.ReadByte(),
                    Bitmap = MNISTImage.FromArray(bytes.Select(b => (byte)(255 - b)).ToArray())
                };
            }
        }
    }

}
