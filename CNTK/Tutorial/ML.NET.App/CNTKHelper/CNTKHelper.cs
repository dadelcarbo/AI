using CNTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ML.NET.App.CNTKHelper
{
    public enum Activation
    {
        None,
        ReLU,
        Sigmoid,
        Tanh
    }
    public class CNTKHelper
    {
        /// <summary>
        /// Create a MLP image classifier for MNIST data.
        /// </summary>
        /// <param name="device">CPU or GPU device to run training and evaluation</param>
        /// <param name="inputSize">Width or Height of the square input image</param>
        /// <param name="inputLayers">Number of layers in the input image</param>
        public static Function CreateMLPModel(DeviceDescriptor device, int inputSize, int inputLayers, int outputSize, float inputScaleFactor = 1.0f)
        {
            var classifierName = "ClassifierOutput";
            Function classifierOutput;
            int imageSize = inputSize * inputSize;
            int[] imageDim = new int[] { imageSize * inputLayers};

            // build the network
            var input = CNTKLib.InputVariable(imageDim, DataType.Float, "Input");

            // For MLP, we like to have the middle layer to have certain amount of states.
            int hiddenLayerDim = (imageDim[0] + outputSize) / 2;
            if (inputScaleFactor == 1.0f)
            {
                classifierOutput = CreateMLPClassifier(device, outputSize, hiddenLayerDim, input, classifierName);
            }
            else
            {
                var scaledInput = CNTKLib.ElementTimes(Constant.Scalar<float>(inputScaleFactor, device), input);
                classifierOutput = CreateMLPClassifier(device, outputSize, hiddenLayerDim, scaledInput, classifierName);
            }

            return classifierOutput;
        }
        /// <summary>
        /// Create a CNN image classifier for MNIST data.
        /// </summary>
        /// <param name="device">CPU or GPU device to run training and evaluation</param>
        public static Function CreateCNNModel(DeviceDescriptor device, int inputSize, int inputLayers, int outputSize, float inputScaleFactor = 1.0f)
        {
            var classifierName = "ClassifierOutput";
            Function classifierOutput;
            int imageSize = inputSize * inputSize;
            int[] imageDim = new int[] { inputSize, inputSize, inputLayers };

            // build the network
            var input = CNTKLib.InputVariable(imageDim, DataType.Float, "Input");
            if (inputScaleFactor == 1.0f)
            {
                classifierOutput = CreateConvolutionalNeuralNetwork(input, inputLayers, outputSize, device, classifierName);
            }
            else
            {
                var scaledInput = CNTKLib.ElementTimes(Constant.Scalar<float>(inputScaleFactor, device), input);
                classifierOutput = CreateConvolutionalNeuralNetwork(scaledInput, inputLayers, outputSize, device, classifierName);
            }

            return classifierOutput;
        }
        private static Function CreateMLPClassifier(DeviceDescriptor device, int numOutputClasses, int hiddenLayerDim, Function scaledInput, string classifierName)
        {
            Function dense = CNTKHelper.Dense(scaledInput, hiddenLayerDim, device, Activation.Sigmoid, "");
            // dense = CNTKHelper.Dense(dense, (hiddenLayerDim + numOutputClasses)/2, device, Activation.ReLU, "");
            Function classifierOutput = CNTKHelper.Dense(dense, numOutputClasses, device, Activation.None, classifierName);
            classifierOutput = CNTKLib.Softmax(classifierOutput);

            return classifierOutput;
        }

        /// <summary>
        /// Create convolution neural network
        /// </summary>
        /// <param name="features">input feature variable</param>
        /// <param name="outDims">number of output classes</param>
        /// <param name="device">CPU or GPU device to run the model</param>
        /// <param name="classifierName">name of the classifier</param>
        /// <returns>the convolution neural network classifier</returns>
        static Function CreateConvolutionalNeuralNetwork(Variable features, int inputLayers, int outDims, DeviceDescriptor device, string classifierName)
        {
            // 28x28x1 -> 14x14x4
            int kernelWidth1 = 3, kernelHeight1 = 3, outFeatureMapCount1 = 4;
            int hStride1 = 2, vStride1 = 2;
            int poolingWindowWidth1 = 3, poolingWindowHeight1 = 3;

            Function pooling1 = ConvolutionWithMaxPooling(features, device, kernelWidth1, kernelHeight1,
                inputLayers, outFeatureMapCount1, hStride1, vStride1, poolingWindowWidth1, poolingWindowHeight1);

            // 14x14x4 -> 7x7x8
            int kernelWidth2 = 3, kernelHeight2 = 3, numInputChannels2 = outFeatureMapCount1, outFeatureMapCount2 = 8;
            int hStride2 = 2, vStride2 = 2;
            int poolingWindowWidth2 = 3, poolingWindowHeight2 = 3;

            Function pooling2 = ConvolutionWithMaxPooling(pooling1, device, kernelWidth2, kernelHeight2,
                numInputChannels2, outFeatureMapCount2, hStride2, vStride2, poolingWindowWidth2, poolingWindowHeight2);

            Function denseLayer = CNTKHelper.Dense(pooling2, outDims, device, Activation.None, classifierName);
            return denseLayer;
        }

        private static Function ConvolutionWithMaxPooling(Variable features, DeviceDescriptor device,
            int kernelWidth, int kernelHeight, int numInputChannels, int outFeatureMapCount,
            int hStride, int vStride, int poolingWindowWidth, int poolingWindowHeight)
        {
            // parameter initialization hyper parameter
            double convWScale = 0.26;
            var convParams = new Parameter(new int[] { kernelWidth, kernelHeight, numInputChannels, outFeatureMapCount }, DataType.Float,
                CNTKLib.GlorotUniformInitializer(convWScale, -1, 2), device);
            Function convFunction = CNTKLib.ReLU(CNTKLib.Convolution(convParams, features, new int[] { 1, 1, numInputChannels } /* strides */));

            Function pooling = CNTKLib.Pooling(convFunction, PoolingType.Max,
                new int[] { poolingWindowWidth, poolingWindowHeight }, new int[] { hStride, vStride }, new bool[] { true });
            return pooling;
        }


        public static Function Dense(Variable input, int outputDim, DeviceDescriptor device,
            Activation activation = Activation.None, string outputName = "")
        {
            if (input.Shape.Rank != 1)
            {
                // 
                int newDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                input = CNTKLib.Reshape(input, new int[] { newDim });
            }

            Function fullyConnected = FullyConnectedLinearLayer(input, outputDim, device, outputName);
            switch (activation)
            {
                default:
                case Activation.None:
                    return fullyConnected;
                case Activation.ReLU:
                    return CNTKLib.ReLU(fullyConnected);
                case Activation.Sigmoid:
                    return CNTKLib.Sigmoid(fullyConnected);
                case Activation.Tanh:
                    return CNTKLib.Tanh(fullyConnected);
            }
        }

        public static Function FullyConnectedLinearLayer(Variable input, int outputDim, DeviceDescriptor device,
            string outputName = "")
        {
            System.Diagnostics.Debug.Assert(input.Shape.Rank == 1);
            int inputDim = input.Shape[0];

            int[] s = { outputDim, inputDim };
            var timesParam = new Parameter((NDShape)s, DataType.Float,
                CNTKLib.GlorotUniformInitializer(
                    CNTKLib.DefaultParamInitScale,
                    CNTKLib.SentinelValueForInferParamInitRank,
                    CNTKLib.SentinelValueForInferParamInitRank, 1),
                device, "timesParam");
            var timesFunction = CNTKLib.Times(timesParam, input, "times");

            int[] s2 = { outputDim };
            var plusParam = new Parameter(s2, 0.0f, device, "plusParam");
            return CNTKLib.Plus(plusParam, timesFunction, outputName);
        }

        public static void PrintTrainingProgress(Trainer trainer)
        {
            float trainLossValue = (float)trainer.PreviousMinibatchLossAverage();
            float evaluationValue = (float)trainer.PreviousMinibatchEvaluationAverage();
            Trace.WriteLine($"CrossEntropyLoss = {trainLossValue}, EvaluationCriterion = {evaluationValue}");
        }

        public static void PrintOutputDims(Function function, string functionName)
        {
            NDShape shape = function.Output.Shape;

            if (shape.Rank == 3)
            {
                Trace.WriteLine($"{functionName} dim0: {shape[0]}, dim1: {shape[1]}, dim2: {shape[2]}");
            }
            else
            {
                Trace.WriteLine($"{functionName} dim0: {shape[0]}");
            }
        }
        /// <summary>
        /// Return the index of the maximum in the parameter
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int ArgMax(float[] array)
        {
            int maxIndex = 0;
            float max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                var val = array[i];
                if (float.IsNaN(val))
                    return -1;
                if (val > max)
                {
                    maxIndex = i;
                    max = val;
                }
            }
            return maxIndex;
        }
        /// <summary>
        /// Create a one hot encoded float array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static float[] OneHot(int value, int size, float ratio = 1.0f)
        {
            var array = new float[size];
            array[value] = ratio;
            return array;
        }
        /// <summary>
        /// Create a one hot encoded float array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static float[] SoftMax(float[] input)
        {
            var array = new float[input.Length];
            double sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += Math.Exp(input[i]);
            }
            for (int i = 0; i < input.Length; i++)
            {
                array[i]= (float)(Math.Exp(input[i])/sum);
            }
            return array;
        }
    }
}
