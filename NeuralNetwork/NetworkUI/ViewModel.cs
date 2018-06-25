using NeuralNetwork.Activation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using NeuralNetwork;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;

namespace WpfApp1
{
    public class ViewModel : INotifyPropertyChanged
    {
       // public Network Network { get; set; }
        public ILayer Layer { get; set; }

        public ViewModel()
        {
         //   this.Network = new Network(new IdentityLayer(5), new DenseLayer(5, 2, new IdentityActivation(), new CrossEntropy()));

            this.Layer = new DenseLayer(5, 7, Activations.First(), LossFunctions.First());

            this.Input = new double[this.Layer.NbInput];
            this.ActualOutput = this.Layer.Output;
            this.ExpectedOutput = new double[this.Layer.NbOutput];
        }

        static ViewModel current;
        public static ViewModel Current => current ?? (current = new ViewModel());

        double[] input;
        public double[] Input { get { return input; } set { input = value; NotifyPropertyChanged("Input"); } }

        double[] weights;
        public double[] Weights { get { return weights; } set { weights = value; NotifyPropertyChanged("Weights"); } }

        double[] actualOutput;
        public double[] ActualOutput { get { return actualOutput; } set { actualOutput = value; NotifyPropertyChanged("ActualOutput"); } }

        double[] expectedOutput;
        public double[] ExpectedOutput { get { return expectedOutput; } set { expectedOutput = value; NotifyPropertyChanged("ExpectedOutput"); } }


        static private List<IActivation> activations;
        static public List<IActivation> Activations => activations ?? (activations = new List<IActivation>()
        {
            new IdentityActivation(),
            new Relu(),
            new Sigmoid(),
            new Softmax()
        });

        private IActivation activation;
        public IActivation Activation
        {
            get { return this.Layer.Activation; }
            set { if (this.Layer.Activation != value) { this.Layer.Activation = value; NotifyPropertyChanged("Activation"); } }
        }

        static private List<ILossFunction> lossFunctions;
        static public List<ILossFunction> LossFunctions => lossFunctions ?? (lossFunctions = new List<ILossFunction>()
        {
            new Distance(),
            new CrossEntropy(),
            new CrossEntropyOneHot()
        });

        private ILossFunction lossFunction;
        public ILossFunction LossFunction
        {
            get { return this.Layer.LossFunction; }
            set { if (this.Layer.LossFunction != value) { this.Layer.LossFunction = value; NotifyPropertyChanged("LossFunction"); } }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public void Calculate()
        {
            this.Layer.Evaluate(this.Input);
            this.ActualOutput = this.Layer.Output;
        }

        public void Train()
        {
            this.Layer.Train(this.Input, this.ExpectedOutput, 0.01);
        }
    }
}
