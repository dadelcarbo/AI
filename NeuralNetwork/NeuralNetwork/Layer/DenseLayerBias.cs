using System;
using System.Collections.Generic;
using NeuralNetwork.Activation;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;

namespace NeuralNetwork.Layer
{
    public class DenseLayerBias : LayerBase
    {
        private double[,] weightsError;

        public DenseLayerBias(int nbInput, int nbOutput, IActivation activation, ILossFunction lossFunction) : base(nbInput + 1, nbOutput, activation, lossFunction)
        {
            errors = new double[nbOutput];
            derivatives = new double[nbOutput];
            weightsError = new double[nbInput + 1, nbOutput];

            this.Input = new double[nbInput + 1];
            this.Input[nbInput] = 1;
        }

        protected override void EvaluateNonActivated(double[] input)
        {
            input.CopyTo(this.Input, 0);

            this.NonActivatedOutput = (this.Weights * this.Input);
        }

        private double[] errors;
        private double[] derivatives;
        //public override double Train(double[] input, double[] expectedOutput, double learningRate)
        //{
        //    this.Evaluate(input);

        //    // Calculate Error
        //    double error = this.LossFunction.Evaluate(this.Output, expectedOutput, errors);
        //    for (var j = 0; j < this.NbOutput; j++)
        //    {
        //        errors[j] *= learningRate;
        //    }

        //    // Update weights
        //    derivatives = this.Activation.Derivative(this.Output);

        //    for (var j = 0; j < this.NbOutput; j++)
        //    {
        //        for (var i = 0; i < this.NbInput; i++)
        //        {
        //            this.Weights[i, j] += errors[j] * derivatives[j] * this.Input[i];
        //        }
        //    }
        //    return error;
        //}
        public override double Train(double[] input, double[] expectedOutput, double learningRate, bool calcInputError = false)
        {
            this.Evaluate(input);

            // Calculate Error
            double error = this.LossFunction.Evaluate(this.Output, this.NonActivatedOutput, expectedOutput, errors, this.Activation);

            // Update weights
            for (var j = 0; j < this.NbOutput; j++)
            {
                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] += errors[j] * this.Input[i] * learningRate;
                }
            }
            return error;
        }

        public override double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate)
        {
            // Reset to zero
            for (var j = 0; j < this.NbOutput; j++)
            {
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
                    for (var i = 0; i < this.NbInput; i++)
                    {
                        this.weightsError[i, j] += errors[j] * derivatives[j];
                    }
                }
            }

            // Apply weight changes
            for (var j = 0; j < this.NbOutput; j++)
            {
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


        public override double BackPropagate(double[] OutputError, double learningRate, double[] weightedError)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            for (var j = 0; j < this.NbOutput; j++)
            {
                for (var i = 0; i < this.NbInput; i++)
                {
                    this.Weights[i, j] = rnd.NextDouble() * 2.0 - 1.0;
                }
            }
        }
    }
}
