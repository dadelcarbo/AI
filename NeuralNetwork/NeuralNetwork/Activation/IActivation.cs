namespace NeuralNetwork.Activation
{
    public interface IActivation
    {
        void Activate(double[] input, double [] output);

        double Derivative(double x);
        double[] Derivative(double[] x);
    }
}
