using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Activation;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;
using System;

namespace NeuralNetworkTest
{
    [TestClass]
    public class DenseLayerTest
    {
        [TestMethod]
        public void EvaluateTest()
        {
            var layer = new DenseLayer(6, 3, new IdentityActivation(), new Distance());
            layer.Initialize();

            var input = NNArray.Random(6);

            layer.Evaluate(input);
        }

        [TestMethod]
        public void BackPropagateNoBiasTest()
        {
            double expectedWeight = 3;
            double actualWeight = 2;

            double inputData = 2;

            double expectedOutput = inputData * expectedWeight;
            double actualOutput = inputData * actualWeight;

            var layer = new DenseLayerNoBias(1, 1, new IdentityActivation(), new Distance());

            layer.Initialize();
            layer.Weights[0, 0] = actualWeight;
            var input = new double[] { inputData };

            layer.Evaluate(input);

            Assert.AreEqual(actualOutput, layer.Output[0]);

            var inputError = new NNArray(1);

            layer.BackPropagate(input, new double[] { expectedOutput }, 1, inputError);

            double expectedErrorInput = (expectedOutput - actualOutput) * actualWeight;
            Assert.AreEqual(expectedErrorInput, inputError[0]);

            layer.Evaluate(input);
            Assert.AreEqual(expectedOutput, layer.Output[0]);
        }
        Random rnd = new Random(1);
        [TestMethod]
        public void LinearRegressionTest()
        {
            // y = ax + b
            double a = 1, b = -2;
            int count = 20;
            double[] input = new double[count];
            double[] expectedOutput = new double[count];
            for (int i = 1; i < count; i++)
            {
                input[i] = i;
                expectedOutput[i] = a * i + b;
            }

            var layer = new DenseLayer(1, 1, new IdentityActivation(), new Distance());
            layer.Initialize();
            layer.Biases[0] = 0;
            layer.Weights[0, 0] = 2;

            int epoc = 0;
            double error = 100;
            while (++epoc < 10000 && error > 0.01)
            {
                error = layer.Train(new double[] { input[1] }, new double[] { expectedOutput[1] }, 0.01);
                error = layer.Train(new double[] { input[2] }, new double[] { expectedOutput[2] }, 0.01);
                error = layer.Train(new double[] { input[3] }, new double[] { expectedOutput[3] }, 0.01);
            }

            for (int n = 0; n < 20; n++)
            {
                for (int i = 1; i < 20; i++)
                {
                }
            }

            double bias = layer.Biases[0];
            double coef = layer.Weights[0, 0];
        }

        [TestMethod]
        public void LinearRegressionNoBiasTest()
        {
            // y = ax + b
            double a = 5.2, b = 0;
            int count = 6;
            double[] input = new double[count];
            double[] expectedOutput = new double[count];
            for (int i = 1; i < count; i++)
            {
                input[i] = i;
                expectedOutput[i] = a * i + b;
            }

            var layer = new DenseLayerNoBias(1, 1, new IdentityActivation(), new SquaredDistance());
            layer.Initialize();

            int epoc = 0;
            double error = 100;
            while (++epoc < 1000 && error > 0.001)
            {
                for (int i = 1; i < count; i++)
                {
                    error = layer.Train(new double[] { input[i] }, new double[] { expectedOutput[i] }, 0.01);
                }
            }
            Assert.IsTrue(epoc < 1000);
            Assert.IsTrue(Math.Abs(layer.Weights[0, 0] - a) < 0.01);
        }
        [TestMethod]
        public void LinearRegressionNeuronBiasTest()
        {
            // y = ax + b
            double a = 2, b = 3;
            int count = 20;
            double[] input = new double[count];
            double[] expectedOutput = new double[count];
            for (int i = 1; i < count; i++)
            {
                input[i] = i;
                expectedOutput[i] = a * i + b;
            }

            var layer = new DenseLayerBias(1, 1, new IdentityActivation(), new SquaredDistance());
            layer.Initialize();
            layer.Weights[0, 0] = 2;

            int epoc = 0;
            double error = 100;
            while (++epoc < 10000 && error > 0.01)
            {
                for (int i = 1; i < count; i++)
                {
                    error = layer.Train(new double[] { input[i] }, new double[] { expectedOutput[i] }, 0.1);
                }
            }

            double coef = layer.Weights[0, 0];
        }

    }
}
