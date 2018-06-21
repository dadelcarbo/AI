using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public static class Extensions
    {
        public static int ArgMax(this double[] array)
        {
            int index = 0;
            double max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i]> max)
                {
                    index = i;
                    max = array[i];
                }
            }
            return index;
        }

        public static int ArgMin(this double[] array)
        {
            int index = 0;
            double min = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                {
                    index = i;
                    min = array[i];
                }
            }
            return index;
        }
    }
}
