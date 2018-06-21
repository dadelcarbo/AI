using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class IdentityActivation : IActivation
    {
        public double[] Activate(double[] input)
        {
            return input.Select(x=>x).ToArray();
        }

        public double Derivative(double x)
        {
            return 1;
        }

        public double[] Derivative(double[] X)
        {
            return X.Select(x => 1.0).ToArray();
        }
    }
}
