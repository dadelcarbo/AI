using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public static class Utils
    {
        static public double[] OneHot(int size, int value)
        {
            var res = new double[size];
            OneHot(res, value);
            return res;
        }
        static public void OneHot(double[] data, int value)
        {
            if (value>data.Length-1) throw new ArgumentException("Value exceeds the data size, One Hot cannot be applied");
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i == value ? 1 : 0;
            }
        }
    }
}
