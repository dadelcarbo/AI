namespace NeuralNetwork.Activation
{
    public interface IActivation
    {
        double[] Activate(double[] input);

        double Derivative(double x);
        double[] Derivative(double[] x);
    }
}
