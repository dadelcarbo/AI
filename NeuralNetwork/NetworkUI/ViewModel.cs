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
    public class ViewModel : INotifyPropertyChanged
    {
        Random rnd = new Random();

        // public Network Network { get; set; }
        public ILayer Layer { get; set; }

        public ViewModel()
        {
            //   this.Network = new Network(new IdentityLayer(5), new DenseLayer(5, 2, new IdentityActivation(), new CrossEntropy()));

            this.Layer = new DenseLayer(4, 3, Activations.First(), LossFunctions.First());
            this.Layer.Initialize();

            this.Input = (new double[this.Layer.NbInput]).Select(x => rnd.NextDouble()).ToArray();
            this.ExpectedOutput = Utils.OneHot(this.Layer.NbOutput, rnd.Next(0, this.Layer.NbOutput - 1));
            this.Weights = this.Layer.Weights.ToDataTable();
            this.errors = new double[this.Layer.NbOutput];

            this.Calculate();

            this.targetError = 0.01;
            this.learnRate = 0.01;
        }

        static ViewModel current;
        public static ViewModel Current => current ?? (current = new ViewModel());

        double[] input;
        public double[] Input { get { return input; } set { input = value; NotifyPropertyChanged("Input"); } }

        DataTable weights;
        public DataTable Weights { get { return weights; } set { weights = value; NotifyPropertyChanged("Weights"); } }

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

        public ILossFunction LossFunction
        {
            get { return this.Layer.LossFunction; }
            set { if (this.Layer.LossFunction != value) { this.Layer.LossFunction = value; NotifyPropertyChanged("LossFunction"); } }
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
            this.Layer.Evaluate(this.Input);

            this.ActualOutput = null;
            this.ActualOutput = this.Layer.Output;

            double[] err = this.errors;
            this.Errors = null;
            this.Layer.LossFunction.Evaluate(this.Layer.Output, this.ExpectedOutput, err);
            this.Errors = err;
        }
        public void Train()
        {
            //this.Step = 0;
            this.Error = 0;
            //do
            //{
            this.Error = this.Layer.Train(this.Input, this.ExpectedOutput, this.LearnRate);
            ++this.Step;
            //} while (this.error > this.targetError && this.Step < 1000);
            
            this.Weights = null;
            this.Weights = this.Layer.Weights.ToDataTable();

            this.Calculate();
        }
        public void Initialize()
        {
            this.Layer.Initialize();
            this.Input = (new double[this.Layer.NbInput]).Select(x => rnd.NextDouble()).ToArray();
            this.ExpectedOutput = Utils.OneHot(this.Layer.NbOutput, rnd.Next(0, this.Layer.NbOutput - 1));
            this.Weights = null;
            this.Weights = this.Layer.Weights.ToDataTable();
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
