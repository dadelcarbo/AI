﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Relu : IActivation
    {
        public double[] Activate(double[] input)
        {
            return input.Select(x=>x>0 ? x : 0).ToArray();
        }

        public double Derivative(double x)
        {
            return x > 0 ? 1 : 0;
        }

        public double[] Derivative(double[] X)
        {
            return X.Select(x => x > 0 ? 1.0 : 0.0).ToArray();
        }
    }
}