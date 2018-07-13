using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Activation
{
    public static class ActivationManager
    {
        static IActivation GetActivation(string name)
        {
            switch (name)
            {
                case "":
                    return null;
                default:
                    throw new ArgumentException($"Activation of type {name} not found !");
            }
        }
    }
}
