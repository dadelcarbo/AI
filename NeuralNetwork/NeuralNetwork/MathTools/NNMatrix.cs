using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.MathTools
{
    /// <summary>.NET GeneralMatrix class.
    /// 
    /// The .NET GeneralMatrix Class provides the fundamental operations of numerical
    /// linear algebra.  Various constructors create Matrices from two dimensional
    /// arrays of double precision floating point numbers.  Various "gets" and
    /// "sets" provide access to submatrices and matrix elements.  Several methods 
    /// implement basic matrix arithmetic, including matrix addition and
    /// multiplication, matrix norms, and element-by-element array operations.
    /// Methods for reading and printing matrices are also included.  All the
    /// operations in this version of the GeneralMatrix Class involve real matrices.
    /// Complex matrices may be handled in a future version.
    /// 
    /// Five fundamental matrix decompositions, which consist of pairs or triples
    /// of matrices, permutation vectors, and the like, produce results in five
    /// decomposition classes.  These decompositions are accessed by the GeneralMatrix
    /// class to compute solutions of simultaneous linear equations, determinants,
    /// inverses and other matrix functions.  The five decompositions are:
    /// <P><UL>
    /// <LI>Cholesky Decomposition of symmetric, positive definite matrices.
    /// <LI>LU Decomposition of rectangular matrices.
    /// <LI>QR Decomposition of rectangular matrices.
    /// <LI>Singular Value Decomposition of rectangular matrices.
    /// <LI>Eigenvalue Decomposition of both symmetric and nonsymmetric square matrices.
    /// </UL>
    /// <DL>
    /// <DT><B>Example of use:</B></DT>
    /// <P>
    /// <DD>Solve a linear system A x = b and compute the residual norm, ||b - A x||.
    /// <P><PRE>
    /// double[][] vals = {{1.,2.,3},{4.,5.,6.},{7.,8.,10.}};
    /// GeneralMatrix A = new GeneralMatrix(vals);
    /// GeneralMatrix b = GeneralMatrix.Random(3,1);
    /// GeneralMatrix x = A.Solve(b);
    /// GeneralMatrix r = A.Multiply(x).Subtract(b);
    /// double rnorm = r.NormInf();
    /// </PRE></DD>
    /// </DL>
    /// </summary>
    /// <author>  
    /// The MathWorks, Inc. and the National Institute of Standards and Technology.
    /// </author>
    /// <version>  5 August 1998
    /// </version>

    [Serializable]
    public class NNMatrix : System.ICloneable, System.Runtime.Serialization.ISerializable, System.IDisposable
    {
        #region Class variables

        /// <summary>Array for internal storage of elements.
        /// @serial internal array storage.
        /// </summary>
        private double[][] A;

        /// <summary>Row and column dimensions.
        /// @serial row dimension.
        /// @serial column dimension.
        /// </summary>
        private int n, m;

        #endregion //  Class variables

        #region Constructors

        /// <summary>Construct an m-by-n matrix of zeros. </summary>
        /// <param name="n">   Number of rows.
        /// </param>
        /// <param name="m">   Number of colums.
        /// </param>

        public NNMatrix(int n, int m)
        {
            this.m = m;
            this.n = n;
            A = new double[n][];
            for (int i = 0; i < n; i++)
            {
                A[i] = new double[m];
            }
        }


        /// <summary>Construct a matrix from a 2-D array.</summary>
        /// <param name="A">   Two-dimensional array of doubles.
        /// </param>
        /// <exception cref="System.ArgumentException">   All rows must have the same length
        /// </exception>
        /// <seealso cref="Create">
        /// </seealso>

        public NNMatrix(double[][] A)
        {
            n = A.Length;
            m = A[0].Length;
            for (int i = 0; i < n; i++)
            {
                if (A[i].Length != m)
                {
                    throw new System.ArgumentException("All rows must have the same length.");
                }
            }
            this.A = A;
        }
        #endregion //  Constructors


        #region Public Properties
        /// <summary>Access the internal two-dimensional array.</summary>
        /// <returns>     Pointer to the two-dimensional array of matrix elements.
        /// </returns>
        virtual public double[][] Array
        {
            get
            {
                return A;
            }
        }
        /// <summary>Copy the internal two-dimensional array.</summary>
        /// <returns>     Two-dimensional array copy of matrix elements.
        /// </returns>
        virtual public double[][] ArrayCopy
        {
            get
            {
                double[][] C = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    C[i] = new double[m];
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        C[i][j] = A[i][j];
                    }
                }
                return C;
            }
        }

        /// <summary>Get col dimension.</summary>
        /// <returns>     m, the number of columns.
        /// </returns>
        virtual public int NbCols
        {
            get
            {
                return m;
            }
        }

        /// <summary>Get row dimension.</summary>
        /// <returns>     n, the number of rows.
        /// </returns>
        virtual public int NbRows
        {
            get
            {
                return n;
            }
        }
        #endregion   // Public Properties

        #region	 Public Methods

        public double this[int i, int j]
        {
            get { return A[i][j]; }
            set { A[i][j] = value; }
        }

        /// <summary>Construct a matrix from a copy of a 2-D array.</summary>
        /// <param name="A">   Two-dimensional array of doubles.
        /// </param>
        /// <exception cref="System.ArgumentException">   All rows must have the same length
        /// </exception>
        public static NNMatrix Create(double[][] A)
        {
            int n = A.Length;
            int m = A[0].Length;
            NNMatrix X = new NNMatrix(n, m);
            double[][] C = X.Array;
            for (int i = 0; i < n; i++)
            {
                if (A[i].Length != m)
                {
                    throw new System.ArgumentException("All rows must have the same length.");
                }
                for (int j = 0; j < m; j++)
                {
                    C[i][j] = A[i][j];
                }
            }
            return X;
        }

        /// <summary>Make a deep copy of a matrix</summary>

        public virtual NNMatrix Copy()
        {
            NNMatrix X = new NNMatrix(n, m);
            double[][] C = X.Array;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    C[i][j] = A[i][j];
                }
            }
            return X;
        }

        /// <summary>Unary minus</summary>
        /// <returns>    -A
        /// </returns>
        public virtual NNMatrix UnaryMinus()
        {
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    C[i][j] = -A[i][j];
                }
            }
            return X;
        }

        /// <summary>C = A + B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A + B
        /// </returns>

        public virtual NNMatrix Add(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = A[i][j] + B.A[i][j];
                }
            }
            return X;
        }

        /// <summary>A = A + B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A + B
        /// </returns>

        public virtual NNMatrix AddEquals(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = A[i][j] + B.A[i][j];
                }
            }
            return this;
        }

        /// <summary>C = A - B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A - B
        /// </returns>

        public virtual NNMatrix Subtract(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = A[i][j] - B.A[i][j];
                }
            }
            return X;
        }

        /// <summary>A = A - B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A - B
        /// </returns>

        public virtual NNMatrix SubtractEquals(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = A[i][j] - B.A[i][j];
                }
            }
            return this;
        }

        /// <summary>Element-by-element multiplication, C = A.*B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A.*B
        /// </returns>
        public virtual NNMatrix ArrayMultiply(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = A[i][j] * B.A[i][j];
                }
            }
            return X;
        }

        /// <summary>Element-by-element multiplication in place, A = A.*B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A.*B
        /// </returns>

        public virtual NNMatrix ArrayMultiplyEquals(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = A[i][j] * B.A[i][j];
                }
            }
            return this;
        }

        /// <summary>Element-by-element right division, C = A./B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A./B
        /// </returns>

        public virtual NNMatrix ArrayRightDivide(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = A[i][j] / B.A[i][j];
                }
            }
            return X;
        }

        /// <summary>Element-by-element right division in place, A = A./B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A./B
        /// </returns>

        public virtual NNMatrix ArrayRightDivideEquals(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = A[i][j] / B.A[i][j];
                }
            }
            return this;
        }

        /// <summary>Element-by-element left division, C = A.\B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A.\B
        /// </returns>

        public virtual NNMatrix ArrayLeftDivide(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = B.A[i][j] / A[i][j];
                }
            }
            return X;
        }

        /// <summary>Element-by-element left division in place, A = A.\B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     A.\B
        /// </returns>

        public virtual NNMatrix ArrayLeftDivideEquals(NNMatrix B)
        {
            CheckMatrixDimensions(B);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = B.A[i][j] / A[i][j];
                }
            }
            return this;
        }

        /// <summary>Multiply a matrix by a scalar, C = s*A</summary>
        /// <param name="s">   scalar
        /// </param>
        /// <returns>     s*A
        /// </returns>

        public virtual NNMatrix Multiply(double s)
        {
            NNMatrix X = new NNMatrix(m, n);
            double[][] C = X.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i][j] = s * A[i][j];
                }
            }
            return X;
        }

        /// <summary>Multiply a matrix by a scalar in place, A = s*A</summary>
        /// <param name="s">   scalar
        /// </param>
        /// <returns>     replace A by s*A
        /// </returns>

        public virtual NNMatrix MultiplyEquals(double s)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i][j] = s * A[i][j];
                }
            }
            return this;
        }

        /// <summary>Linear algebraic matrix multiplication, A * B</summary>
        /// <param name="B">   another matrix
        /// </param>
        /// <returns>     Matrix product, A * B
        /// </returns>
        /// <exception cref="System.ArgumentException">  Matrix inner dimensions must agree.
        /// </exception>
        public virtual NNMatrix Multiply(NNMatrix B)
        {
            if (B.n != m)
            {
                throw new System.ArgumentException("GeneralMatrix inner dimensions must agree.");
            }
            NNMatrix X = new NNMatrix(this.n, B.m);
            double[][] C = X.Array;
            double[] Bcolj = new double[m];
            for (int j = 0; j < B.m; j++)
            {
                for (int k = 0; k < m; k++)
                {
                    Bcolj[k] = B.A[k][j];
                }
                for (int i = 0; i < n; i++)
                {
                    double[] Arowi = A[i];
                    double s = 0;
                    for (int k = 0; k < m; k++)
                    {
                        s += Arowi[k] * Bcolj[k];
                    }
                    C[i][j] = s;
                }
            }
            return X;
        }


        /// <summary>Linear algebraic matrix multiplication, A * B</summary>
        /// <param name="B">   an array
        /// </param>
        /// <returns>     Matrix product, A * B
        /// </returns>
        /// <exception cref="System.ArgumentException">  Matrix inner dimensions must agree.
        /// </exception>
        public virtual NNArray Multiply(NNArray B)
        {
            if (B.Length != n)
            {
                throw new System.ArgumentException("GeneralMatrix inner dimensions must agree.");
            }
            NNArray X = new NNArray(this.NbCols);
            for (int j = 0; j < m; j++)
            {
                double val = 0;
                for (int i = 0; i < n; i++)
                {
                    val += this[i, j] * B[i];
                }
                X.Values[j] = val;
            }
            return X;
        }

        /// <summary>Linear algebraic matrix multiplication, A * B</summary>
        /// <param name="B">   an array</param>
        /// <param name="X">   an array to store the result</param>
        /// <returns>     Matrix product, A * B
        /// </returns>
        /// <exception cref="System.ArgumentException">  Matrix inner dimensions must agree.
        /// </exception>
        public virtual void Multiply(NNArray B, NNArray X)
        {
            if (B.Length != n || X.Length != m)
            {
                throw new System.ArgumentException("GeneralMatrix inner dimensions must agree.");
            }
            for (int j = 0; j < m; j++)
            {
                double val = 0;
                for (int i = 0; i < n; i++)
                {
                    val += this[i, j] * B[i];
                }
                X.Values[j] = val;
            }
        }

        public virtual NNMatrix Transpose()
        {
            NNMatrix X = new NNMatrix(this.m, this.n);
            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    X[j, i] = this[i, j];
                }
            }
            return X;
        }

        #region Operator Overloading

        /// <summary>
        ///  Addition of matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static NNMatrix operator +(NNMatrix m1, NNMatrix m2)
        {
            return m1.Add(m2);
        }

        /// <summary>
        /// Subtraction of matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static NNMatrix operator -(NNMatrix m1, NNMatrix m2)
        {
            return m1.Subtract(m2);
        }

        /// <summary>
        /// Multiplication of matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static NNMatrix operator *(NNMatrix m1, NNMatrix m2)
        {
            return m1.Multiply(m2);
        }

        /// <summary>
        /// Multiplication of matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static NNArray operator *(NNMatrix m1, NNArray m2)
        {
            return m1.Multiply(m2);
        }

        #endregion   //Operator Overloading

        /// <summary>Matrix trace.</summary>
        /// <returns>     sum of the diagonal elements.
        /// </returns>

        public virtual double Trace()
        {
            double t = 0;
            for (int i = 0; i < System.Math.Min(m, n); i++)
            {
                t += A[i][i];
            }
            return t;
        }

        /// <summary>Generate matrix with random elements</summary>
        /// <param name="m">   Number of rows.
        /// </param>
        /// <param name="n">   Number of colums.
        /// </param>
        /// <returns>     An m-by-n matrix with uniformly distributed random elements.
        /// </returns>

        public static NNMatrix Random(int m, int n)
        {
            System.Random random = new System.Random();

            NNMatrix A = new NNMatrix(m, n);
            double[][] X = A.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i][j] = random.NextDouble();
                }
            }
            return A;
        }

        /// <summary>Generate identity matrix</summary>
        /// <param name="m">   Number of rows.
        /// </param>
        /// <param name="n">   Number of colums.
        /// </param>
        /// <returns>     An m-by-n matrix with ones on the diagonal and zeros elsewhere.
        /// </returns>
        public static NNMatrix Identity(int m, int n)
        {
            NNMatrix A = new NNMatrix(m, n);
            double[][] X = A.Array;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i][j] = (i == j ? 1.0 : 0.0);
                }
            }
            return A;
        }

        #endregion //  Public Methods

        #region	 Private Methods

        /// <summary>Check if size(A) == size(B) *</summary>

        private void CheckMatrixDimensions(NNMatrix B)
        {
            if (B.m != m || B.n != n)
            {
                throw new System.ArgumentException("GeneralMatrix dimensions must agree.");
            }
        }
        #endregion //  Private Methods

        #region Implement IDisposable
        /// <summary>
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            if (disposing)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~NNMatrix()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion //  Implement IDisposable

        /// <summary>Clone the GeneralMatrix object.</summary>
        public System.Object Clone()
        {
            return this.Copy();
        }

        /// <summary>
        /// A method called when serializing this class
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public DataTable ToDataTable()
        {
            int len1d = this.NbRows;
            int len2d = this.NbCols;

            DataTable dt = new DataTable();
            for (int i = 0; i < len2d; i++)
            {
                dt.Columns.Add("W" + i, typeof(double));
            }

            for (int row = 0; row < len1d; row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < len2d; col++)
                {
                    dr[col] = this[row, col];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public bool Equals(NNMatrix B)
        {
            if ((this.m != B.m) || (this.n != B.n)) return false;
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    if (this[i, j] != B[i, j]) return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            string s = string.Empty;
            for (int row = 0; row < NbRows; row++)
            {
                s += this.Array[row].Select(v => v.ToString("0.###")).Aggregate((i, j) => i + "," + j);

                s += Environment.NewLine;
            }
            return s;
        }
    }
}
