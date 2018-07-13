using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Layer;
using NeuralNetwork.MathTools;

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

        /// <summary>
        /// Add hidden layer to the end
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(ILayer layer)
        {
            // Validation
            if (this.HiddenLayers.Count==0)
            {
                if (this.InputLayer.NbOutput != layer.NbInput) throw new ArgumentException("Layer nb input doesn't match previous layer output");
            }
            else
            {
                if (this.HiddenLayers.Last().NbOutput != layer.NbInput) throw new ArgumentException("Layer nb input doesn't match previous layer output");
            }
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

        public double Train(double[] input, NNArray expectedOutput, double learningRate)
        {
            this.Evaluate(input);

            double[] outputError = new double[this.OutputLayer.NbOutput];

            var error = this.OutputLayer.LossFunction.Evaluate(this.OutputLayer.Output, expectedOutput, outputError);

            double[] weightedError = HiddenLayers.Any() ? new double[this.OutputLayer.NbInput] : null;

            this.OutputLayer.BackPropagate(outputError, learningRate, weightedError);
            for (int n = this.HiddenLayers.Count - 1; n >= 0; n--)
            {
                var layer = this.HiddenLayers[n];

                // Apply derivative to weightedErrors to calculate next layer output error
                outputError = new double[layer.NbOutput];
                var derivative = layer.Activation.Derivative(layer.NonActivatedOutput);
                for(int i =0; i< outputError.Length; i++)
                {
                    outputError[i] = weightedError[i] * derivative[i]; 
                }

                weightedError = n>0 ? new double[layer.NbInput] : null;
                this.HiddenLayers[n].BackPropagate(outputError, learningRate, weightedError);
            }

            return error;
        }

        public double Train(double[] input, double[] expectedOutput, double learningRate, double error, int maxSteps)
        {
            return 0;
        }

        public double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate,
            double error, int maxSteps)
        {
            return 0;
        }
    }
}
