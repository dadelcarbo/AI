using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using NeuralNetwork.Activation;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;

namespace NeuralNetwork.Layer
{
    public abstract class LayerBase : ILayer
    {
        protected static readonly Random rnd = new Random();
        public double[] Input { get; set; }
        public double[] InputError { get; set; }

        public int NbInput { get; private set; }

        public int NbOutput { get; private set; }

        public double[] Output { get; private set; }

        public NNMatrix Weights { get; private set; }

        public double[] NonActivatedOutput { get; protected set; }

        public IActivation Activation { get; set; }

        public ILossFunction LossFunction { get; set; }
        
        public LayerBase(int nbInput, int nbOutput, IActivation activation, ILossFunction lossFunction)
        {
            this.InputError = new double[nbInput];
            this.NbOutput = nbOutput;
            this.NbInput = nbInput;
            this.Output = new double[nbOutput];
            this.Weights = new NNMatrix(nbInput, nbOutput);
            this.NonActivatedOutput = new double[nbOutput];
            this.Activation = activation;
            this.LossFunction = lossFunction;
        }

        public void Evaluate(double[] input)
        {
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

        public abstract double Train(double[] input, double[] expectedOutput, double learningRate, bool calcInputError = false);
        public abstract double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate);
        public abstract void Initialize();
        public abstract double BackPropagate(double[] OutputError, double learningRate, double[] weightedError);
    }
}
