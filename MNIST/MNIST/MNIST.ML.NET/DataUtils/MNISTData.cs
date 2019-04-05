using Microsoft.ML.Data;

namespace MNIST.ML.NET.DataUtils
{
    public class MNISTData
    {
        const int size = 28;
        public int Feature { get; set; }
        [VectorType(size * size)]
        public double[] Values { get; set; }
    }
}
