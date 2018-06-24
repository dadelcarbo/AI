using System;
using System.Linq;

namespace NeuralNetwork.Activation
{
    public class Softmax : IActivation
    {
        public void Activate(double[] input, double[] output)
        {
            var e = input.Select(Math.Exp).ToArray();
            var sum = 1.0 / e.Sum();
            for (var i = 0; i < input.Length; i++)
            {
                output[i] = e[i]*sum;
            }
        }

        public double Derivative(double x)
        {
            throw new NotImplementedException();
        }

        public double[] Derivative(double[] x)
        {
            throw new NotImplementedException();
        }
    }
}
