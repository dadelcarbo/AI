using NeuralNetwork.Activation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NeuralNetwork;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using System.Collections;
using System.Data;

namespace WpfApp1
{
    public class NetworkViewModel : INotifyPropertyChanged
    {
        Random rnd = new Random();

        public Network Network { get; set; }
        private ILayer Layer1 { get; set; }
        private ILayer Layer2 { get; set; }

        public NetworkViewModel()
        {
            ILayer layer = new DenseLayer(5, 4, new IdentityActivation(), new CrossEntropy());

            this.Network = new Network(new IdentityLayer(5), new DenseLayer(4, 2, new IdentityActivation(), new CrossEntropy()));
            this.Network.AddLayer(layer);

            this.Layer1 = layer;
            this.Layer2 = this.Network.OutputLayer;



            this.Input = (new double[this.Network.InputLayer.NbInput]).Select(x => rnd.NextDouble()).ToArray();
            this.ExpectedOutput = Utils.OneHot(this.Layer2.NbOutput, rnd.Next(0, this.Layer2.NbOutput - 1));

            this.errors = new double[this.Layer2.NbOutput];

            this.Calculate();

            this.targetError = 0.01;
            this.learnRate = 0.01;
        }

        static NetworkViewModel current;
        public static NetworkViewModel Current => current ?? (current = new NetworkViewModel());

        double[] input;
        public double[] Input { get { return input; } set { input = value; NotifyPropertyChanged("Input"); } }

        public DataTable Weights1 => this.Layer1.Weights.ToDataTable();
        public DataTable Weights2 => this.Layer2.Weights.ToDataTable();

        double[] actualOutput;
        public double[] ActualOutput { get { return actualOutput; } set { actualOutput = value; NotifyPropertyChanged("ActualOutput"); } }

        double[] expectedOutput;
        public double[] ExpectedOutput { get { return expectedOutput; } set { expectedOutput = value; NotifyPropertyChanged("ExpectedOutput"); } }

        private double[] errors;
        public double[] Errors { get { return errors; } set { errors = value; NotifyPropertyChanged("Errors"); } }

        static private List<IActivation> activations;
        static public List<IActivation> Activations => activations ?? (activations = new List<IActivation>()
        {
            new IdentityActivation(),
            new Relu(),
            new Sigmoid(),
            new Softmax()
        });

        public IActivation Activation
        {
            get { return this.Layer2.Activation; }
            set { if (this.Layer2.Activation != value) { this.Layer2.Activation = value; NotifyPropertyChanged("Activation"); } }
        }

        static private List<ILossFunction> lossFunctions;
        static public List<ILossFunction> LossFunctions => lossFunctions ?? (lossFunctions = new List<ILossFunction>()
        {
            new Distance(),
            new CrossEntropy(),
            new CrossEntropyOneHot()
        });

        public ILossFunction LossFunction
        {
            get { return this.Layer2.LossFunction; }
            set { if (this.Layer2.LossFunction != value) { this.Layer2.LossFunction = value; NotifyPropertyChanged("LossFunction"); } }
        }

        private double learnRate;
        public double LearnRate { get { return learnRate; } set { if (value != learnRate) { learnRate = value; NotifyPropertyChanged("LearnRate"); } } }

        private int step;
        public int Step { get { return step; } set { if (value != step) { step = value; NotifyPropertyChanged("Step"); } } }

        private double error;
        public double Error { get { return error; } set { if (value != error) { error = value; NotifyPropertyChanged("Error"); } } }

        private double targetError;
        public double TargetError { get { return targetError; } set { if (value != targetError) { targetError = value; NotifyPropertyChanged("TargetError"); } } }

        public void Calculate()
        {
            this.Network.Evaluate(this.Input);

            this.ActualOutput = null;
            this.ActualOutput = this.Network.OutputLayer.Output;

            double[] err = this.errors;
            this.Errors = null;

            this.Layer2.LossFunction.Evaluate(this.Layer2.Output, this.ExpectedOutput, err);
            this.Errors = err;
        }
        public void Train()
        {
            //this.Step = 0;
            this.Error = 0;
            //do
            //{
            this.Error = this.Network.Train(this.Input, this.ExpectedOutput, this.LearnRate, this.Error, 1000);
            ++this.Step;
            //} while (this.error > this.targetError && this.Step < 1000);


            this.NotifyPropertyChanged("Weights1");
            this.NotifyPropertyChanged("Weights2");

            this.Calculate();
        }
        public void Initialize()
        {
            this.Network.Initialize();
            this.Input = (new double[this.Network.InputLayer.NbInput]).Select(x => rnd.NextDouble()).ToArray();
            this.ExpectedOutput = Utils.OneHot(this.Layer2.NbOutput, rnd.Next(0, this.Layer2.NbOutput - 1));

            NotifyPropertyChanged("Weights1");
            NotifyPropertyChanged("Weights2");

            this.Calculate();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
