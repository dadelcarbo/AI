using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using NeuralNetwork.Activation;
using NeuralNetwork.Layer;
using NeuralNetwork.Loss;
using NeuralNetwork.MathTools;

namespace NeuralNetworkTest
{
    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void BackPropagateTest()
        {
            IActivation activation = new Relu();
            TrainNetwork(activation);
            activation = new IdentityActivation();
            TrainNetwork(activation);
            activation = new Sigmoid();
            TrainNetwork(activation);
            activation = new Tanh();
            TrainNetwork(activation);
        }

        private static void TrainNetwork(IActivation activation)
        {
            int nbInput = 6;
            var network = new Network(
                 new IdentityLayer(nbInput),
                 new DenseLayerNoBias(3, 2, new IdentityActivation(), new SquaredDistance()));

            network.AddLayer(new DenseLayerNoBias(nbInput, 5, activation, new SquaredDistance()));
            network.AddLayer(new DenseLayerNoBias(5, 4, activation, new SquaredDistance()));
            network.AddLayer(new DenseLayerNoBias(4, 3, activation, new SquaredDistance()));

            network.Initialize();

            var input = NNArray.Random(nbInput);
            var output = NNArray.Random(2);

            network.Evaluate(input);

            DateTime start;
            DateTime end;

            int epoc = 0, maxEpoc = 5000;
            double error = double.MaxValue;


            start = DateTime.Now;
            while (++epoc < maxEpoc && error > 0.01)
            {
                error = network.Train(input, output, 0.01);
            }
            end = DateTime.Now;

            var duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine($"Duration for activation {activation.Name}: {duration} + epoc: {epoc}");

            Assert.IsTrue(epoc < maxEpoc);
        }
    }
}
