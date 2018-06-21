using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using System.Linq;

namespace NeuralNetworkTest
{
    [TestClass]
    public class NNTest1
    {
        Random rnd = new Random();
        [TestMethod]
        public void XORTraining()
        {
            double[][] inputs = new double[][] { new double[] { -1, -1 }, new double[] { -1, 1 }, new double[] { 1, -1 }, new double[] { 1, 1 } };
            double[] outputs = new double[] { 0, 1, 1, 0 };

            var nn = new Network(
                new IdentityLayer(2),
                new DenseLayer(2, 1, new Relu()));

            for (int i = 0; i < inputs.Length; i++)
            {
                nn.Evaluate(inputs[i]);

                Assert.AreNotEqual(outputs[i], nn.OutputLayer.Output[0]);
            }


            // Train network
            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    double error = nn.OutputLayer.Train(inputs[i], new double[] { outputs[i] }, 0.05);
                    Console.WriteLine("Error: " + error);
                    Assert.AreNotEqual(outputs[i], nn.OutputLayer.Output[0]);
                }
            }
        }

        [TestMethod]
        public void SoftMax()
        {
            int nb = 10;

            double[] input = new double[nb];
            for (int i = 0; i < nb; i++)
            {
                input[i] = Math.PI * rnd.NextDouble();
            }

            var softmax = new Softmax();

            double[] actual = softmax.Activate(input);

            Assert.AreEqual(input.ArgMax(), actual.ArgMax());

            double actualSum = actual.Sum();

            Assert.IsTrue(Math.Abs(1 - actualSum) <= 0.00001);
        }
        [TestMethod]
        public void CrossEntropyTest()
        {
            var crossEntropyOH = new CrossEntropyOneHot();
            var crossEntropy = new CrossEntropy();

            int nb = 10;

            double[] input = new double[nb];
            for (int i = 0; i < nb; i++)
            {
                input[i] = Math.PI * rnd.NextDouble();
            }

            var softmax = new Softmax();

            double[] actualOutput = softmax.Activate(input);

            double[] expectedOutput = new double[nb];
            expectedOutput[actualOutput.ArgMax()] = 1;

            double entropyOH = crossEntropyOH.Evaluate(actualOutput, expectedOutput);
            double entropy = crossEntropy.Evaluate(actualOutput, expectedOutput);

            double noEntropy = crossEntropy.Evaluate(actualOutput, actualOutput);

            Assert.AreEqual(entropyOH, entropy);
        }
    }
}
