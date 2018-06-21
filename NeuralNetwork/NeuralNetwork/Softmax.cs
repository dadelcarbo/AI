using System;
using System.Linq;

namespace NeuralNetwork
{
    public class Softmax : IActivation
    {
        public double[] Activate(double[] input)
        {
            var e = input.Select(Math.Exp).ToArray();
            var sum = 1.0 / e.Sum();
            for (var i = 0; i < input.Length; i++)
            {
                e[i] *= sum;
            }
            return e;
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
