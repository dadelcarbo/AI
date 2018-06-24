using System.Linq;

namespace NeuralNetwork.Activation
{
    public class Relu : IActivation
    {
        public void Activate(double[] input, double[] output)
        {
            for (var i = 0; i < input.Length; i++)
            {
                output[i] = input[i] > 0 ? input[i] : 0;
            }
        }

        public double Derivative(double x)
        {
            return x > 0 ? 1 : 0;
        }

        public double[] Derivative(double[] X)
        {
            return X.Select(x => x > 0 ? 1.0 : 0.0).ToArray();
        }
    }
}
