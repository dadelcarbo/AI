using System;
using System.Linq;

namespace NeuralNetwork.Activation
{
    public class Tanh : IActivation
    {
        public string Name => "Tanh";
        public void Activate(double[] input, double[] output)
        {
            for (var i = 0; i < input.Length; i++)
            {
                output[i] = Math.Tanh(input[i]);
            }
        }

        public double Derivative(double x)
        {
            double tanh = Math.Tanh(x);
            return 1 - tanh * tanh;
        }

        public double[] Derivative(double[] X)
        {
            return X.Select(x => Derivative(x)).ToArray();
        }
    }
}
