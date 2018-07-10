using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.MathTools
{
    public class NNArray
    {
        public double[] Values { get; set; }
        public int Length => Values.Length;

        public NNArray(int size) {this.Values = new double[size]; }
        public NNArray(double[] values) { this.Values = values; }

        /// <summary>Generate matrix with random elements</summary>
        /// <param name="n">   Number of colums.</param>
        /// <returns>     An array with uniformly distributed random elements.
        /// </returns>
        public static NNArray Random(int n)
        {
            System.Random random = new System.Random();

            double[] res = new double[n];
            for (int i = 0; i < n; i++)
            {
                res[i] = random.NextDouble();
            }
            return res;
        }
        /// <summary>Generate matrix with random elements</summary>
         /// <param name="n">   Number of colums.</param>
         /// <returns>     An array with uniformly distributed random elements.
         /// </returns>
        public NNArray Copy()
        {
            double[] res = new double[this.Length];
            for (int i = 0; i < this.Length; i++)
            {
                res[i] = this[i];
            }
            return (NNArray)res;
        }

        #region Implicit cast operators
        // User-defined conversion from Digit to double
        public static implicit operator double[](NNArray d)
        {
            return d.Values;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator NNArray(double[] d)
        {
            return new NNArray(d);
        }
        #endregion
        #region Operators
        public double this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }
        
        public static NNArray operator +(NNArray a, NNArray b)
        {
            if (a.Length != b.Length) throw new ArgumentException("NNArrays do not have same length");
            var res = new NNArray(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] + b[i];
            }
            return res;
        }
        public void Add(NNArray b)
        {
            if (this.Length != b.Length) throw new ArgumentException("NNArrays do not have same length");
            var res = new NNArray(this.Length);
            for (int i = 0; i < this.Length; i++)
            {
                this.Values[i] += b[i];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="res">   an array to store the result</param>
        public void Add(NNArray b, NNArray res)
        {
            if (this.Length != b.Length || this.Length != res.Length) throw new ArgumentException("NNArrays do not have same length");
            for (int i = 0; i < this.Length; i++)
            {
                this.Values[i] += b[i];
            }
        }

        public static NNArray operator -(NNArray a, NNArray b)
        {
            if (a.Length != b.Length) throw new ArgumentException("NNArrays do not have same length");
            var res = new NNArray(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }
        public void Subst(NNArray b)
        {
            if (this.Length != b.Length) throw new ArgumentException("NNArrays do not have same length");
            var res = new NNArray(this.Length);
            for (int i = 0; i < this.Length; i++)
            {
                this.Values[i] -= b[i];
            }
        }

        public static NNArray operator *(NNArray a, double b)
        {
            var res = new NNArray(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                res[i] = a[i] * b;
            }
            return res;
        }

        public void Mult(double b)
        {
            var res = new NNArray(this.Length);
            for (int i = 0; i < this.Length; i++)
            {
                this.Values[i] *= b;
            }
        }

        /// <summary>
        /// Scalar product
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double operator *(NNArray a, NNArray b)
        {
            if (a.Length != b.Length) throw new ArgumentException("NNArrays do not have same length");
            double res = 0;
            for (int i = 0; i < a.Length; i++)
            {
                res += a[i] * b[i];
            }
            return res;
        }

        #endregion


    }
}
