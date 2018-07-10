using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using NeuralNetwork.Activation;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void BackPropagateTest()
        {
            var layer = new DenseLayer(2, 1, new IdentityActivation(), new Distance());
            layer.Initialize();
            layer.Weights[0, 0] = 1;
            layer.Weights[1, 0] = 1;
            layer.Biases[0] = 0;

            var input = new double[] { 1, 1 };

            layer.Evaluate(input);

            Assert.AreEqual(2, layer.Output[0]);

            var expectedOutput = new double[] { 1 };

            var inputError = new NNArray(2);
            layer.BackPropagate(input, expectedOutput, 0.01, inputError);
            Assert.AreEqual(-0.5, inputError[0]);
            Assert.AreEqual(-0.5, inputError[1]);

            layer.Train(input, expectedOutput, 1);

            layer.Evaluate(input);
            Assert.AreEqual(expectedOutput[0], layer.Output[0]);
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
            for (int i = 1; i <count; i++)
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
    }
}
