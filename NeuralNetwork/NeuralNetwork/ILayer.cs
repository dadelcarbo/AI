using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public interface ILayer
    {
        int NbInput { get; }
        double[] Input { get; }
        int NbOutput { get; }
        double[] Output { get; }

        void Evaluate(double[] input);

        double Train(double[] input, double[] output, double errorRate);

        void Initialize();
    }
}
