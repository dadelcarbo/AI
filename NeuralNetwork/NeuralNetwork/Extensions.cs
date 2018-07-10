using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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
                if (array[i] > max)
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

        public static DataTable ToDataTable(this double[,] data)
        {
            int len1d = data.GetLength(0);
            int len2d = data.GetLength(1);

            DataTable dt = new DataTable();
            for (int i = 0; i < len2d; i++)
            {
                dt.Columns.Add("W"+i, typeof(double));
            }

            for (int row = 0; row < len1d; row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < len2d; col++)
                {
                    dr[col] = data[row, col];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static double[] Add(this double[] a, double[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Array do not have same length");
            var res = new double[a.Length];
            for(int i = 0; i< a.Length;i++)
            {
                res[i] = a[i] + b[i];
            }
            return res;
        }

        public static double[] Subst(this double[] a, double[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Array do not have same length");
            var res = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }
    }
}
