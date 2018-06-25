using System;
using System.Runtime.Remoting;
using NeuralNetwork.Activation;
using NeuralNetwork.Loss;

namespace NeuralNetwork.Layer
{
    public abstract class LayerBase : ILayer
    {
        public double[] Input { get; set; }

        public int NbInput { get; private set; }

        public int NbOutput { get; private set; }

        public double[] Output { get; private set; }

        protected double[] NonActivatedOutput { get; private set; }

        public IActivation Activation { get; set; }

        public ILossFunction LossFunction { get; set; }
        
        public LayerBase(int nbInput, int nbOutput, IActivation activation, ILossFunction lossFunction)
        {
            this.NbOutput = nbOutput;
            this.NbInput = nbInput;
            this.Output = new double[nbOutput];
            this.NonActivatedOutput = new double[nbOutput];
            this.Activation = activation;
            this.LossFunction = lossFunction;
        }

        public void Evaluate(double[] input)
        {
            if (input.Length != this.NbInput) throw new ArgumentException($"Expected input of length {this.NbInput}, but input was of length {input.Length}");

            this.EvaluateNonActivated(input);
            this.Activate();
        }

        protected abstract void EvaluateNonActivated(double[] input);

        protected virtual void Activate()
        {
            if (this.Activation != null)
            {
                this.Activation.Activate(this.NonActivatedOutput, this.Output);
            }
            else
            {
                throw new InvalidOperationException("Activation is null");
            }
        }

        public abstract double Train(double[] input, double[] output, double errorRate);

        public abstract void Initialize();
    }
}
