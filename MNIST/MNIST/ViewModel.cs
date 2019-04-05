using MNIST.ML.NET.DataUtils;
using MNIST.ML.NET.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MNIST
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            this.output = new double[10];
        }
        public List<MNISTImage> Images { get; set; } = new List<MNISTImage>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            int i = 0;
            foreach (var item in MnistReader.Read(MnistReader.TrainImages, MnistReader.TrainLabels))
            {
                Images.Add(item);
                if (i++ > 200) break;
            }
            Image = Images.First();
        }

        private MNISTImage image;
        public MNISTImage Image
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

        private double[] output;
        public double[] Output
        {
            get { return output; }
            set
            {
                if (output != value)
                {
                    output = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Output"));
                }
            }
        }

        MNISTModel model = new MNISTModel();

        public void Evaluate()
        {
        }


        public void Train()
        {
            model.Train(Images.Select(i => i.MNISTData));
        }
    }

}