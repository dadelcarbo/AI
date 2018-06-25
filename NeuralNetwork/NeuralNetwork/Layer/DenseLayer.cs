using System;
using NeuralNetwork.Activation;
using NeuralNetwork.Loss;

namespace NeuralNetwork.Layer
{
    public class DenseLayer : LayerBase
    {
        private static readonly Random rnd = new Random();

        public DenseLayer(int nbInput, int nbOutput, IActivation activation, ILossFunction lossFunction) : base(nbInput, nbOutput, activation, lossFunction)
        {
            this.Weights = new double[nbInput, nbOutput];
            this.Biases = new double[nbOutput];

            errors = new double[nbOutput];
            derivatives = new double[nbOutput];
        }

        public double[,] Weights { get; private set; }
        public double[] Biases { get; private set; }

        protected override void EvaluateNonActivated(double[] input)
        {
            this.Input = input;

            for (var j = 0; j < this.NbOutput; j++)
            {
                this.NonActivatedOutput[j] = this.Biases[j];
                for (var i = 0; i < this.NbInput; i++)
                {
                    this.NonActivatedOutput[j] += this.Weights[i, j] * this.Input[i];
                }
            }
        }

        private double[] errors;
        private double[] derivatives;
        public override double Train(double[] input, double[] expectedOutput, double learningRate)
        {
            this.Evaluate(input);

            // Calculate Error
            double error = 0;
            for (int i = 0; i < Output.Length; i++)
            {
                errors[i] = this.Output[i] - expectedOutput[i];

                error += errors[i] * errors[i];
            }

            // Update weights
            derivatives = this.Activation.Derivative(this.Output);
            for (var j = 0; j < this.NbOutput; j++)
            {
                this.Biases[j] -= errors[j] * learningRate;

                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] -= errors[j] * learningRate * derivatives[j];
                }
            }
            return error;
        }

        public override void Initialize()
        {
            for (var j = 0; j < this.NbOutput; j++)
            {
                this.Biases[j] = rnd.NextDouble() * 2.0 - 1.0;
                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] = rnd.NextDouble() * 2.0 - 1.0;
                }
            }
        }
    }
}
