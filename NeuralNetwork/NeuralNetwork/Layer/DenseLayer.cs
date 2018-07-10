using System;
using System.Collections.Generic;
using NeuralNetwork.Activation;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;

namespace NeuralNetwork.Layer
{
    public class DenseLayer : LayerBase
    {
        private static readonly Random rnd = new Random();

        private double[] biasesError;
        private double[,] weightsError;

        public DenseLayer(int nbInput, int nbOutput, IActivation activation, ILossFunction lossFunction) : base(nbInput, nbOutput, activation, lossFunction)
        {
            this.Biases = new double[nbOutput];

            errors = new double[nbOutput];
            derivatives = new double[nbOutput];

            biasesError = new double[nbOutput];
            weightsError = new double[nbInput, nbOutput];
        }

        public NNArray Biases { get; private set; }

        protected override void EvaluateNonActivated(double[] input)
        {
            this.Input = input;

            this.NonActivatedOutput = (this.Weights * input) + this.Biases;
        }

        private double[] errors;
        private double[] derivatives;
        public override double Train(double[] input, double[] expectedOutput, double learningRate)
        {
            this.Evaluate(input);

            // Calculate Error
            double error = this.LossFunction.Evaluate(this.Output, expectedOutput, errors);
            for (var j = 0; j < this.NbOutput; j++)
            {
                errors[j] *= learningRate;
            }

            // Update weights
            derivatives = this.Activation.Derivative(this.Output);
            for (var j = 0; j < this.NbOutput; j++)
            {
                this.Biases[j] += errors[j];

                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] += errors[j] * derivatives[j];
                }
            }
            return error;
        }

        public override double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate)
        {
            // Reset to zero
            for (var j = 0; j < this.NbOutput; j++)
            {
                this.biasesError[j] = 0;

                for (var i = 0; i < this.NbInput; i++)
                {
                    this.weightsError[i, j] = 0;
                }
            }

            // Calculate weight update for all items in minibatch
            double error = 0;
            int k = 0;
            foreach (var input in inputBatch)
            {
                this.Evaluate(input);

                // Calculate Error
                double[] expectedOutput = expectedOutputBatch[k];
                double err = this.LossFunction.Evaluate(this.Output, expectedOutput, errors);
                error += err * err;
                for (var j = 0; j < this.NbOutput; j++)
                {
                    errors[j] *= learningRate;
                }

                // Update weights
                derivatives = this.Activation.Derivative(this.Output);
                for (var j = 0; j < this.NbOutput; j++)
                {
                    biasesError[j] += errors[j];

                    for (var i = 0; i < this.NbInput; i++)
                    {
                        this.weightsError[i, j] += errors[j] * derivatives[j];
                    }
                }
            }

            // Apply weight changes
            for (var j = 0; j < this.NbOutput; j++)
            {
                this.Biases[j] += this.biasesError[j];

                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] += this.weightsError[i, j];
                }
            }

            return Math.Sqrt(error);
        }

        public double BackPropagate(double[] input, double[] expectedOutput, double learningRate, double[] inputError)
        {
            this.Evaluate(input);

            // Calculate Error
            var derivative = this.Activation.Derivative(this.NonActivatedOutput);
            for (var j = 0; j < this.NbOutput; j++)
            {
                errors[j] = derivative[j] * (expectedOutput[j] - this.Output[j]) / this.NbInput;
            }

            // Calculate InputError
            this.Weights.Transpose().Multiply(errors, inputError);

            return 0.0;
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
