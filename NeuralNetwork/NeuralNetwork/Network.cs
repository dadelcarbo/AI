using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Layer;

namespace NeuralNetwork
{
    public class Network
    {
        public ILayer InputLayer { get; set; }

        public ILayer OutputLayer { get; set; }

        private List<ILayer> HiddenLayers { get; set; }

        public Network(ILayer inputLayer, ILayer outputLayer)
        {
            this.InputLayer = inputLayer;
            this.OutputLayer = outputLayer;

            this.HiddenLayers = new List<ILayer>();
        }

        private bool isInitialized = false;

        public void AddLayer(ILayer layer)
        {
            this.HiddenLayers.Add(layer);
        }

        public void Evaluate(double[] inputData)
        {
            if (!isInitialized)
                this.Initialize();

            this.InputLayer.Evaluate(inputData);
            double[] output = this.InputLayer.Output;
            foreach (var hiddenLayer in this.HiddenLayers)
            {
                hiddenLayer.Evaluate(output);
                output = hiddenLayer.Output;
            }

            this.OutputLayer.Evaluate(output);
        }

        public void Initialize()
        {
            this.InputLayer.Initialize();
            foreach (var hiddenLayer in this.HiddenLayers)
            {
                hiddenLayer.Initialize();
            }

            this.OutputLayer.Initialize();

            this.isInitialized = true;
        }

        public double Train(double[] input, double[] expectedOutput, double learningRate, double error, int maxSteps)
        {
            return 0;
        }
        public double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate, double error, int maxSteps)
        {
            return 0;
        }
        
    }
}
