using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MNIST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = (ViewModel)this.Resources["ViewModel"];
            this.DataContext = vm;
            int i = 0;
            foreach (var item in MnistReader.Read(MnistReader.TrainImages, MnistReader.TrainLabels))
            {
                vm.Images.Add(item);
                if (i++ > 200) break;
            }

            this.DataContext = this;

            vm.Image = vm.Images.First();


        }

        private void EvaluateButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Evaluate();
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Train();
        }
    }
    public static class MnistReader
    {
        public const string TrainImages = "mnist/train-images.idx3-ubyte";
        public const string TrainLabels = "mnist/train-labels.idx1-ubyte";
        public const string TestImages = "mnist/t10k-images.idx3-ubyte";
        public const string TestLabels = "mnist/t10k-labels.idx1-ubyte";

        public static IEnumerable<Image> ReadTrainingData()
        {
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<Image> ReadTestData()
        {
            foreach (var item in Read(TestImages, TestLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<Image> Read(string imagesPath, string labelsPath)
        {
            Console.WriteLine(imagesPath);
            BinaryReader labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            BinaryReader images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

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

                yield return new Image()
                {
                    Data = arr,
                    Label = labels.ReadByte(),
                    Bitmap = Image.FromArray(bytes.Select(b => (byte)(255 - b)).ToArray())
                };
            }
        }
    }

}
