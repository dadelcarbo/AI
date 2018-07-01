using System;
using System.Collections.Generic;
using NeuralNetwork.Activation;

namespace NeuralNetwork.Layer
{
    public class IdentityLayer : LayerBase
    {
        private static readonly Random rnd = new Random();

        public IdentityLayer(int nbInput) : base(nbInput, nbInput, new IdentityActivation(), null)
        {
        }

        protected override void EvaluateNonActivated(double[] input)
        {
            this.Input = input;
            for (var i = 0; i < this.NbInput; i++)
            {
                this.NonActivatedOutput[i] = this.Input[i];
            }
        }

        public override double Train(double[] input, double[] output, double learningRate)
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
