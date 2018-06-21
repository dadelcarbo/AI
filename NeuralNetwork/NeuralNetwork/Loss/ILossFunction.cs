namespace NeuralNetwork.Loss
{
    public interface ILossFunction
    {
        double Evaluate(double[] actual, double[] expected);
    }
}
