using NeuralNetwork.Activation;

namespace NeuralNetwork.Loss
{
    public interface ILossFunction
    {
        string Name { get; }
        double Evaluate(double[] actual, double[] expected, double[] errors);
        double Evaluate(double[] actual, double[] actualNonActivated, double[] expected, double[] errors, IActivation activation);
        double Derivative(double actual, double expected);
        void Derivative(double[] actual, double[] expected, double[] derivatives);
    }
}
