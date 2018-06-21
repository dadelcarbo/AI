using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public interface IActivation
    {
        double[] Activate(double[] input);

        double Derivative(double x);
        double[] Derivative(double[] x);
    }
}
