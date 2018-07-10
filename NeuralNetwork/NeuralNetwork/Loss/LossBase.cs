using System;
using NeuralNetwork.Activation;

namespace NeuralNetwork.Loss
{
    public abstract class LossBase : ILossFunction
    {
        public abstract string Name { get; }
        public abstract double Evaluate(double[] actual, double[] expected, double[] errors);

        public abstract double Derivative(double actual, double expected);

        public abstract void Derivative(double[] actual, double[] expected, double[] derivatives);

        public double Evaluate(double[] actual, double[] actualNonActivated, double[] expected, double[] errors, IActivation activation)
        {
            double[] activationDerivative = activation.Derivative(actualNonActivated);
            this.Derivative(actual, expected, errors);

            double res = 0;
            for (int i = 0; i < actual.Length; i++)
            {
                var err = activationDerivative[i]*errors[i];
                errors[i] =err;
                res += err*err;
            }
            return res;
        }
    }
}