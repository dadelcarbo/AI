using System;

namespace NeuralNetwork.Loss
{
    public class Distance : ILossFunction
    {
        public string Name => "Distance";
        public double Evaluate(double[] actual, double[] expected, double[] errors)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");
            
            double sum = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                var diff = expected[i] - actual[i];
                errors[i] = diff;
                sum += diff*diff;
            }

            return Math.Sqrt(sum);
        }
    }
}