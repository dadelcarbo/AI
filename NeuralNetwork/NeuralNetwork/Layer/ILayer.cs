using NeuralNetwork.Activation;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;
using System.Collections.Generic;

namespace NeuralNetwork.Layer
{
    public interface ILayer
    {
        int NbInput { get; }
        double[] Input { get; }
        int NbOutput { get; }
        NNMatrix Weights { get; }
        double[] Output { get; }

        void Evaluate(double[] input);

        double Train(double[] input, double[] expectedOutput, double learningRate);
        double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate);

        void Initialize();

        IActivation Activation { get; set; }
        ILossFunction LossFunction { get; set; }
    }
}
