using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    interface ILossFunction
    {
        double Evaluate(double[] actual, double[] expected);
    }
}
