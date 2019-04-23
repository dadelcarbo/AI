using CNTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CNTKSamples
{
    class Model
    {
        DeviceDescriptor device = DeviceDescriptor.CPUDevice;

        const int TEST1_SIZE = 10;

        Function mlpModel;
        Function cnnModel;

        public Model()
        {
            #region Create Models
            mlpModel = CNTKHelper.CreateMLPModel(device, TEST1_SIZE, 1, TEST1_SIZE);

            cnnModel = CNTKHelper.CreateCNNModel(device, TEST1_SIZE, 1, TEST1_SIZE);
            #endregion
        }

        public void MLPTest()
        {
            //Create 10 images containing horizontal line according to one hot value
            var value = Value.CreateBatch<float>(new int[] { TEST1_SIZE * TEST1_SIZE }, OneHotImages(TEST1_SIZE), DeviceDescriptor.CPUDevice);

            this.TrainAndEvaluateTest(mlpModel, value);
        }
        public void CNNTest()
        {
            var value = Value.CreateBatch<float>(new int[] { TEST1_SIZE, TEST1_SIZE, 1 }, OneHotImages(TEST1_SIZE), DeviceDescriptor.CPUDevice);
            this.TrainAndEvaluateTest(cnnModel, value);
        }

        /// <summary>
        /// Test a simple model which takes a one hot encoded digit as an input and returns the same as an output
        /// </summary>
        private void TrainAndEvaluateTest(Function model, Value inputValue)
        {
            #region Evaluate model before training 

            var inputDataMap = new Dictionary<Variable, Value>() { { model.Arguments[0], inputValue } };
            var outputDataMap = new Dictionary<Variable, Value>() { { model.Output, null } };

            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

            IList<IList<float>> preTrainingOutput = outputDataMap[model.Output].GetDenseData<float>(model.Output);
            for (int i = 0; i < TEST1_SIZE; i++)
            {
                Trace.WriteLine($"Argmax({i}): {CNTKHelper.ArgMax(preTrainingOutput[i].ToArray())}");
            }
            #endregion

            #region Train Model
            var labels = CNTKLib.InputVariable(new int[] { TEST1_SIZE }, DataType.Float, "Error Input");
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(new Variable(model), labels, "lossFunction");
            var prediction = CNTKLib.ClassificationError(new Variable(model), labels, "classificationError");

            // Set per sample learning rate
            CNTK.TrainingParameterScheduleDouble learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.003125, 1);

            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(model.Parameters(), learningRatePerSample) };
            var trainer = Trainer.CreateTrainer(model, trainingLoss, prediction, parameterLearners);

            // Create expected output
            var expectedOutputValue = Value.CreateBatch<float>(new int[] { TEST1_SIZE }, ExpectedOutput(TEST1_SIZE), DeviceDescriptor.CPUDevice);

            var inputMiniBatch = new MinibatchData(inputValue, TEST1_SIZE);
            var outputMiniBatch = new MinibatchData(expectedOutputValue, TEST1_SIZE);

            var arguments = new Dictionary<Variable, MinibatchData>
                {
                    { model.Arguments[0], inputMiniBatch },
                    { labels, outputMiniBatch }
                };
            int epochs = 5;
            while (epochs > 0)
            {
                trainer.TrainMinibatch(arguments, device);

                epochs--;
            }
            #endregion

            #region Evaluate Model after training 

            outputDataMap = new Dictionary<Variable, Value>() { { model.Output, null } };
            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

            IList<IList<float>> postTrainingOutput = outputDataMap[model.Output].GetDenseData<float>(model.Output);
            int nbFail = 0;
            for (int i = 0; i < TEST1_SIZE; i++)
            {
                int prepTrainValue = CNTKHelper.ArgMax(preTrainingOutput[i].ToArray());
                int postTrainValue = CNTKHelper.ArgMax(postTrainingOutput[i].ToArray());
                if (i != postTrainValue) nbFail++;
                Trace.WriteLine($"Argmax({i}): {prepTrainValue} ==>  {postTrainValue}");
            }
            Trace.WriteLine($"Failure rate = ({nbFail}/{TEST1_SIZE})");
            #endregion
        }

        private static float[] ExpectedOutput(int size)
        {
            var result = new float[size * size];
            for (int i = 0; i < size; i++)
            {
                var oneHot = CNTKHelper.OneHot(i, size);
                for (int j = 0; j < size; j++)
                {
                    result[i * size + j] = oneHot[j];
                }
            }
            return result;
        }
        private static float[] OneHotImages(int size)
        {
            var result = new float[size * size * size];
            for (int k = 0; k < size; k++)
            {
                var oneHot = CNTKHelper.OneHot(k, size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[k * size * size + i * size + j] = oneHot[i];
                    }
                }
            }
            return result;
        }

        private void testc()
        {
            var value = Value.CreateBatch<float>(new int[] { 6 }, new float[] { 0, 0.2f, 0.8f, 0, 0.6f, 0 }, DeviceDescriptor.CPUDevice);

            var input = CNTKLib.InputVariable(new int[] { 6 }, DataType.Float, "Input");
            var output = CNTKLib.Softmax(input);

            var inputDataMap = new Dictionary<Variable, Value>() { { input, value } };

            var outputDataMap = new Dictionary<Variable, Value>() { { output, null } };

            output.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

            IList<IList<float>> actualLabelSoftMax = outputDataMap[output].GetDenseData<float>(output);
        }

        private void GenerateValueData(int sampleSize, int inputDim, int numOutputClasses, out Value featureValue, out Value labelValue, DeviceDescriptor device)
        {
            float[] features;
            float[] oneHotLabels;

            GenerateRawDataSamples(sampleSize, inputDim, numOutputClasses, out features, out oneHotLabels);

            featureValue = Value.CreateBatch<float>(new int[] { inputDim }, features, device);
            labelValue = Value.CreateBatch<float>(new int[] { numOutputClasses }, oneHotLabels, device);
        }

        private void GenerateRawDataSamples(int sampleSize, int inputDim, int numOutputClasses, out float[] features, out float[] oneHotLabels)
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