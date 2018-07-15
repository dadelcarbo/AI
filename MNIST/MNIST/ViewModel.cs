using NeuralNetwork;
using NeuralNetwork.Activation;
using NeuralNetwork.DataUtils;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using System.Collections.Generic;
using System.ComponentModel;

namespace MNIST
{
    public class ViewModel : INotifyPropertyChanged
    {
        Network network;
        SortedDictionary<byte, double[]> oneHotLabel = new SortedDictionary<byte, double[]>();
        public ViewModel()
        {
            network = new Network(new NormalizedLayer(28 * 28, 255), new DenseLayer(28 * 28, 10, new Softmax(), new CrossEntropyOneHot()));
            this.output = new double[10];

            for (byte i = 0; i < 10; i++) {
                oneHotLabel.Add(i, Utils.OneHot(10, i));
                           }
        }
        public List<MNISTImage> Images { get; set; } = new List<MNISTImage>();

        public event PropertyChangedEventHandler PropertyChanged;
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

        public void Evaluate()
        {
            this.Output = null;

            network.Evaluate(Image.Values);

            this.Output = network.OutputLayer.Output;
        }

        public void Train()
        {
            network.Train(Image.Values,  oneHotLabel[Image.Label], 0.01, 0.01, 1000);
        }
    }

}