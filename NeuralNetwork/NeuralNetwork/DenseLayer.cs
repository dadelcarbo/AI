using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class DenseLayer : LayerBase
    {
        private static readonly Random rnd = new Random();

        public DenseLayer(int nbInput, int nbOutput, IActivation activation) : base(nbInput, nbOutput, activation)
        {
            this.Weights = new double[nbInput, nbOutput];
            this.Biases = new double[nbOutput];

            errors = new double[nbOutput];
        }


        public double[,] Weights { get; private set; }
        public double[] Biases { get; private set; }

        public override void Evaluate(double[] input)
        {
            if (input.Length != this.NbInput) throw new ArgumentException($"Expected input of length {this.NbInput}, but input was of length {input.Length}");

            this.Input = input;

            for (var j = 0; j < this.NbOutput; j++)
            {
                this.Output[j] = this.Biases[j];
                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Output[j] += this.Weights[i, j] * this.Input[i];
                }
            }
        }

        private double[] errors;
        public override double Train(double[] input, double[] expectedOutput, double learningRate)
        {
            this.Evaluate(input);

            // Calculate Error
            double error = 0;
            for (int i = 0; i < Output.Length; i++)
            {
                errors[i] = expectedOutput[i] - this.Output[i];

                error += errors[i] * errors[i];
            }

            // Update weights
            for (var j = 0; j < this.NbOutput; j++)
            {
                //this.Biases[j] -= errors[j] * learningRate;

                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] -= errors[j] * learningRate;
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
