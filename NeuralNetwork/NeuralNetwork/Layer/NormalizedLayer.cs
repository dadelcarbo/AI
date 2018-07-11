using System;
using System.Collections.Generic;
using NeuralNetwork.Activation;

namespace NeuralNetwork.Layer
{
    public class NormalizedLayer : LayerBase
    {
        double Max { get; set; }

        public NormalizedLayer(int nbInput, double max) : base(nbInput, nbInput, new IdentityActivation(), null)
        {
            this.Max = max;
        }

        protected override void EvaluateNonActivated(double[] input)
        {
            this.Input = input;
            for (var i = 0; i < this.NbInput; i++)
            {
                this.NonActivatedOutput[i] = this.Input[i] / Max;
            }
        }

        public override double Train(double[] input, double[] output, double learningRate, bool calcInputError = false)
        {
            return 0.0;
        }

        public override double Train(List<double[]> inputBatch, List<double[]> expectedOutputBatch, double learningRate)
        {
            return 0.0;
        }

        public override void Initialize()
        {
        }
    }
}
