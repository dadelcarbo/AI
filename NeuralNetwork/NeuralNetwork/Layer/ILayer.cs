using NeuralNetwork.Activation;
using NeuralNetwork.Loss;

namespace NeuralNetwork.Layer
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

        IActivation Activation { get; set; }
        ILossFunction LossFunction { get; set; }
    }
}
