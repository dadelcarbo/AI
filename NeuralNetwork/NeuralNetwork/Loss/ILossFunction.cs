namespace NeuralNetwork.Loss
{
    public interface ILossFunction
    {
        string Name { get; }
        double Evaluate(double[] actual, double[] expected, double[] errors);
    }
}
