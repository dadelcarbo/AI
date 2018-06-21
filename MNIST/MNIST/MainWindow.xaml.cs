using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TensorFlow;

namespace MNIST
{
    public class ViewModel : INotifyPropertyChanged
    {

        public List<Image> Images { get; set; } = new List<Image>();

        private Image image;

        public event PropertyChangedEventHandler PropertyChanged;

        public Image Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
                }
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel vm = (ViewModel)this.Resources["ViewModel"];
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
                    Bitmap = Image.FromArray(bytes.Select(b=>(byte)(255-b)).ToArray())
                };
            }
        }
    }

}
