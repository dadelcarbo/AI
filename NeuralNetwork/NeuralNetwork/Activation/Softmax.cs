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
                output[i] = e[i] * sum;
            }
        }

        public void Derivative(double[] input, double[] output)
        {
            double[] activated = new double[input.Length];
            this.Activate(input, activated);
            for (int i = 0; i < input.Length; i++)
            {
                double y = activated[i];
                output[i] = y * (1 - y);
            }
        }

        public double[] Derivative(double[] input)
        {
            double[] activated = new double[input.Length];
            this.Activate(input, activated);
            return activated.Select(y => y * (1 - y)).ToArray();
        }
    }
}
