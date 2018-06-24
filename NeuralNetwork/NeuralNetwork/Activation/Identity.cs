using System.Linq;

namespace NeuralNetwork.Activation
{
    public class IdentityActivation : IActivation
    {
        public void Activate(double[] input, double[] output)
        {
            for (var i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }
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
