using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using NeuralNetwork.Activation;
using NeuralNetwork.DataUtils;
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
            int nbInput = 6;
            var input = NNArray.Random(nbInput);
            var output = new double[] { 0, 1 };

            IActivation activation;
            activation = new IdentityActivation();
            TrainNetwork(activation, input, output);
            activation = new Sigmoid();
            TrainNetwork(activation, input, output);
            activation = new Tanh();
            TrainNetwork(activation, input, output);
            activation = new Relu();
            TrainNetwork(activation, input, output);
        }
        [TestMethod]
        public void SoftmaxPropagateTest()
        {
            int nbInput = 10;
            NNArray input = Utils.OneHot(nbInput, 5);
            NNArray output = Utils.OneHot(nbInput, 3);

            IActivation activation;
            activation = new Softmax();

            var network = new Network(
                 new IdentityLayer(nbInput),
                 new DenseLayerNoBias(nbInput, nbInput, activation, new SquaredDistance()));

            network.Initialize();

            DateTime start;
            DateTime end;

            int epoc = 0, maxEpoc = 10000;
            double error = double.MaxValue;

            start = DateTime.Now;
            while (++epoc < maxEpoc && error > 0.05)
            {
                error = network.Train(input, output, 0.01);
            }
            end = DateTime.Now;

            var duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine($"Duration for activation {activation.Name}: {duration} \t epoc: {epoc}\terror: {error}");

            Assert.IsTrue(epoc < maxEpoc);
        }

        private static void TrainNetwork(IActivation activation, NNArray input, NNArray output)
        {
            var nbInput = input.Length;
            var network = new Network(
                 new IdentityLayer(nbInput),
                 new DenseLayerNoBias(3, 2, activation, new SquaredDistance()));

            network.AddLayer(new DenseLayerNoBias(nbInput, 3, activation, new SquaredDistance()));

            network.Initialize();

            DateTime start;
            DateTime end;

            int epoc = 0, maxEpoc = 10000;
            double error = double.MaxValue;

            start = DateTime.Now;
            while (++epoc < maxEpoc && error > 0.05)
            {
                error = network.Train(input, output, 0.01);
            }
            end = DateTime.Now;

            var duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine($"Duration for activation {activation.Name}: {duration} \t epoc: {epoc}\terror: {error}");

            Assert.IsTrue(epoc < maxEpoc);
        }

        [TestMethod]
        public void MNISTPropagateTest()
        {
            MnistReader.RootPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\MNIST"));

            var images = MnistReader.ReadTestData().ToList();

            Assert.IsTrue(images.Count() > 0);

            var image = images.ElementAt(0);
            NNArray input = image.Values;
            NNArray output = Utils.OneHot(10, image.Label);

            var nbInput = input.Length;

            IActivation activation;
            activation = new Softmax();

            var network = new Network(
                 new NormalizedLayer(nbInput, 1),
                 new DenseLayerNoBias(nbInput, 10, activation, new CrossEntropyOneHot()));

            // network.AddLayer(new DenseLayerNoBias(nbInput, 28, activation, new SquaredDistance()));

            network.Initialize();

            DateTime start;
            DateTime end;

            int epoc = 0, maxEpoc = 10000;
            double error = double.MaxValue;

            start = DateTime.Now;
            while (++epoc < maxEpoc && error > 0.01)
            {
                error = network.Train(input, output, 0.01);
            }
            end = DateTime.Now;

            var duration = (end - start).TotalMilliseconds / 1000;
            Console.WriteLine($"Duration for activation {activation.Name}: {duration} \t epoc: {epoc}\terror: {error}");

            Assert.AreEqual(image.Label, network.OutputLayer.Output.ArgMax());
            Assert.IsTrue(epoc < maxEpoc);

            foreach (var img in images.Where(i => i.Label == image.Label))
            {
                network.Evaluate(img.Values);
                Console.WriteLine($"{network.OutputLayer.Output.ArgMax()}");
            }
        }

    }
}
