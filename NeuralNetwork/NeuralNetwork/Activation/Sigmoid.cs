using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Activation
{
    /// <summary>
    /// Sigmoid activation function.
    /// </summary>
    ///
    /// <remarks><para>The class represents sigmoid activation function with
    /// the next expression:
    /// <code lang="none">
    ///                1
    /// f(x) = ------------------
    ///        1 + exp(-alpha * x)
    ///
    ///           alpha * exp(-alpha * x )
    /// f'(x) = ---------------------------- = alpha * f(x) * (1 - f(x))
    ///           (1 + exp(-alpha * x))^2
    /// </code>
    /// </para>
    ///
    /// <para>Output range of the function: <b>[0, 1]</b>.</para>
    /// 
    /// <para>Functions graph:</para>
    /// <img src="img/neuro/sigmoid.bmp" width="242" height="172" />
    /// </remarks>
    /// 
    public class Sigmoid : IActivation
    {
        public string Name => "Sigmoid";
        // sigmoid's alpha value
        private double alpha = 2;

        /// <summary>
        /// Sigmoid's alpha value.
        /// </summary>
        /// 
        /// <remarks><para>The value determines steepness of the function. Increasing value of
        /// this property changes sigmoid to look more like a threshold function. Decreasing
        /// value of this property makes sigmoid to be very smooth (slowly growing from its
        /// minimum value to its maximum value).</para>
        ///
        /// <para>Default value is set to <b>2</b>.</para>
        /// </remarks>
        /// 
        public double Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sigmoid"/> class.
        /// </summary>
        public Sigmoid() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sigmoid"/> class.
        /// </summary>
        /// 
        /// <param name="alpha">Sigmoid's alpha value.</param>
        /// 
        public Sigmoid(double alpha)
        {
            this.alpha = alpha;
        }


        /// <summary>
        /// Calculates function value.
        /// </summary>
        ///
        /// <param name="x">Function input value.</param>
        /// 
        /// <returns>Function output value, <i>f(x)</i>.</returns>
        ///
        /// <remarks>The method calculates function value at point <paramref name="x"/>.</remarks>
        ///
        public double Activate(double x)
        {
            return (1 / (1 + Math.Exp(-alpha * x)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void Activate(double[] input, double[] output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = this.Activate(input[i]);
            }
        }

        /// <summary>
        /// Calculates function derivative.
        /// </summary>
        /// 
        /// <param name="x">Function input value.</param>
        /// 
        /// <returns>Function derivative, <i>f'(x)</i>.</returns>
        /// 
        /// <remarks>The method calculates function derivative at point <paramref name="x"/>.</remarks>
        ///
        public double Derivative(double x)
        {
            double y = Activate(x);

            return (alpha * y * (1 - y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double[] Derivative(double[] x)
        {
            return x.Select(v => this.Activate(v)).ToArray();
        }
    }
}