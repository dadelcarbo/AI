using System;
using NeuralNetwork.Activation;

namespace NeuralNetwork.Layer
{
    public class IdentityLayer : LayerBase
    {
        private static readonly Random rnd = new Random();

        public IdentityLayer(int nbInput) : base(nbInput, nbInput, new IdentityActivation())
        {
        }

        public override void Evaluate(double[] input)
        {
            if (input.Length != this.NbInput) throw new ArgumentException($"Expected input of length {this.NbInput}, but input was of length {input.Length}");

            this.Input = input;
            for (var i = 0; i < this.NbInput; i++)
            {
                this.Output[i] = this.Input[i];
            }
        }

        public override double Train(double[] input, double[] output, double errorRate)
        {
            return 0.0;
        }

        public override void Initialize()
        {
        }
    }
}
