using CNTK;
using System;
using System.Collections.Generic;

namespace WpfApp5
{
    static class Model
    {
        private static void GenerateValueData(int sampleSize, int inputDim, int numOutputClasses, out Value featureValue, out Value labelValue, DeviceDescriptor device)
        {
            float[] features;
            float[] oneHotLabels;

            GenerateRawDataSamples(sampleSize, inputDim, numOutputClasses, out features, out oneHotLabels);

            featureValue = Value.CreateBatch<float>(new int[] { inputDim }, features, device);
            labelValue = Value.CreateBatch<float>(new int[] { numOutputClasses }, oneHotLabels, device);
        }

        private static void GenerateRawDataSamples(int sampleSize, int inputDim, int numOutputClasses, out float[] features, out float[] oneHotLabels)
        {
            Random random = new Random(0);

            features = new float[sampleSize * inputDim];
            oneHotLabels = new float[sampleSize * numOutputClasses];

            for (int sample = 0; sample < sampleSize; sample++)
            {
                int label = random.Next(numOutputClasses);
                for (int i = 0; i < numOutputClasses; i++)
                {
                    oneHotLabels[sample * numOutputClasses + i] = label == i ? 1 : 0;
                }

                for (int i = 0; i < inputDim; i++)
                {
                    features[sample * inputDim + i] = (float)GenerateGaussianNoise(3, 1, random) * (label + 1);
                }
            }

        }

        // <summary>
        /// https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
        /// https://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>

        /// <returns></returns>
        static double GenerateGaussianNoise(double mean, double stdDev, Random random)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double stdNormalRandomValue = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * stdNormalRandomValue;
        }
    }
}