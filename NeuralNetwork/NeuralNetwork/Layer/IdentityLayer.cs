using System;
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

        public override double Train(double[] input, double[] output, double errorRate)
        {
            return 0.0;
        }

        public override void Initialize()
        {
        }
    }
}
