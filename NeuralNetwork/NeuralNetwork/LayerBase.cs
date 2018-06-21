using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public abstract class LayerBase : ILayer
    {
        public double[] Input { get; set; }

        public int NbInput { get; private set; }

        public int NbOutput { get; private set; }

        public double[] Output { get; private set; }

        public IActivation Activation { get; private set; }

        public LayerBase(int nbInput, int nbOutput, IActivation activation)
        {
            this.NbOutput = nbOutput;
            this.NbInput = nbInput;
            this.Output = new double[nbOutput];
            this.Activation = activation;
        }

        public abstract void Evaluate(double[] input);

        public abstract double Train(double[] input, double[] output, double errorRate);

        public abstract void Initialize();
    }
}
