using System;
using NeuralNetwork.Activation;

namespace NeuralNetwork.Loss
{
    public class SquaredDistance : LossBase, ILossFunction
    {
        public override string Name => "Distance";

        public double Ratio { get; set; }

        public SquaredDistance()
        {
            this.Ratio = 0.5;
        }
        
        public override double Evaluate(double[] actual, double[] expected, double[] errors)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");

            double sum = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                var diff = this.Ratio * (expected[i] - actual[i]);
                diff *= diff;
                errors[i] = diff;
                sum += diff;
            }

            return Math.Sqrt(sum);
        }

        public override double Derivative(double actual, double expected)
        {
            return expected - actual;
        }
        public override void Derivative(double[] actual, double[] expected, double[] derivatives)
        {
            for (int i = 0; i < actual.Length; i++)
            {
                derivatives[i] = expected[i] - actual[i];
            }
        }
    }
}