﻿using System;

namespace NeuralNetwork.Loss
{
    public class Distance : LossBase, ILossFunction
    {
        public override string Name => "Distance";
        public override double Evaluate(double[] actual, double[] expected, double[] errors)
        {
            if (actual.Length != expected.Length) throw new ArgumentException("Input arrays have different size");

            double sum = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                var diff = Math.Abs(expected[i] - actual[i]);
                errors[i] = diff;
                sum += diff * diff;
            }

            return Math.Sqrt(sum);
        }

        public override double Derivative(double actual, double expected)
        {
            return (expected > actual) ? 1 : -1;
        }
        public override void Derivative(double[] actual, double[] expected, double[] derivatives)
        {
            for (int i = 0; i < actual.Length; i++)
            {
                derivatives[i] = (expected[i] > actual[i]) ? 1 : -1;
            }
        }
    }
}