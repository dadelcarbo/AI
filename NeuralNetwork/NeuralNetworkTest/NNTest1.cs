using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using System.Linq;
using NeuralNetwork.Activation;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;

namespace NeuralNetworkTest
{
    [TestClass]
    public class NNTest1
    {
        Random rnd = new Random();
        [TestMethod]
        public void XORTraining()
        {
            //double[][] inputs = new double[][] { new double[] { -1, -1 }, new double[] { -1, 1 }, new double[] { 1, -1 }, new double[] { 1, 1 } };
            //double[] outputs = new double[] { 0, 1, 1, 0 };

            //var nn = new Network(
            //    new IdentityLayer(2),
            //    new DenseLayer(2, 1, new Relu(), new CrossEntropy()));

            //for (int i = 0; i < inputs.Length; i++)
            //{
            //    nn.Evaluate(inputs[i]);

            //    Assert.AreNotEqual(outputs[i], nn.OutputLayer.Output[0]);
            //}

            //// Train network
            //for (int j = 0; j < 100; j++)
            //{
            //    for (int i = 0; i < inputs.Length; i++)
            //    {
            //        double error = nn.OutputLayer.Train(inputs[i], new double[] { outputs[i] }, 0.05);
            //        Console.WriteLine("Error: " + error);
            //    }
            //}
        }

        [TestMethod]
        public void TrainingLayer()
        {
            double[][] inputs = new double[][] { new double[] { 0, 1, 0, 1 } };
            double[][] outputs = new double[][] { new double[] { 0, 1, 0 } };

            var layer = new DenseLayer(inputs[0].Length, outputs[0].Length, new Relu(), new Distance());

            for (int iter = 0; iter < 20; iter++)
            {
                // Generate input data
                for (int i = 0; i < inputs[0].Length; i++)
                {
                    inputs[0][i] = rnd.NextDouble();
                }

                // Train network
                int step = 0;
                double error = double.MaxValue;
                double errorTarget = 0.0001;
                while (step < 1000 && error > errorTarget)
                {
                    error = layer.Train(inputs[0], outputs[0], 0.05);
                    step++;
                }
                Console.WriteLine($"Iteration: {iter} Step: {step} Error: {error}");
            }
        }

        [TestMethod]
        public void SoftMax()
        {
            int nb = 10;

            double[] input = new double[nb];
            double[] output = new double[nb];
            for (int i = 0; i < nb; i++)
            {
                input[i] = Math.PI * rnd.NextDouble();
            }

            var softmax = new Softmax();

            softmax.Activate(input, output);

            Assert.AreEqual(input.ArgMax(), output.ArgMax());

            double actualSum = output.Sum();

            Assert.IsTrue(Math.Abs(1 - actualSum) <= 0.00001);
        }

        [TestMethod]
        public void CrossEntropyTest()
        {
            var crossEntropyOH = new CrossEntropyOneHot();
            var crossEntropy = new CrossEntropy();

            int nb = 10;

            double[] input = new double[nb];
            double[] output = new double[nb];
            for (int i = 0; i < nb; i++)
            {
                input[i] = Math.PI * rnd.NextDouble();
            }

            var softmax = new Softmax();

            softmax.Activate(input, output);

            double[] expectedOutput = new double[nb];
            expectedOutput[output.ArgMax()] = 1;

            double[] errors = new double[nb];
            double entropyOH = crossEntropyOH.Evaluate(output, expectedOutput, errors);
            double entropy = crossEntropy.Evaluate(output, expectedOutput, errors);

            Assert.AreEqual(entropyOH, entropy);
        }

        
    }
}
