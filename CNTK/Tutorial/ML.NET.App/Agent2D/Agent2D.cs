using CNTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.NET.App.Agent2D
{
    public class Agent2D
    {
        public string Name => "Agent2D";

        Function model;
        DeviceDescriptor device = DeviceDescriptor.CPUDevice;
        Dictionary<Variable, Value> inputDataMap;
        Dictionary<Variable, Value> outputDataMap;

        Dictionary<Variable, Value> inputTrainBatch;
        Dictionary<Variable, Value> outputTrainBatch;

        Trainer trainer;

        Variable inputVariable;
        Variable actionVariable;

        Random rnd = new Random();

        const int SIZE_IN = 50;
        const int SIZE_OUT = 2;
        const int SIZE_IN_NB_LAYER = 2; // 1 layer for Coins - Player

        public Agent2D()
        {
            int inputSize = SIZE_IN;
            int outputSize = SIZE_OUT;

            model = CNTKHelper.CNTKHelper.CreateMLPModel1D(device, inputSize, SIZE_IN_NB_LAYER, outputSize);

            inputVariable = model.Arguments.First(a => a.IsInput);
            inputDataMap = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputDataMap = new Dictionary<Variable, Value>() { { model.Output, null } };

            inputTrainBatch = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputTrainBatch = new Dictionary<Variable, Value>() { { model.Output, null } };

            // Set per sample learning rate
            CNTK.TrainingParameterScheduleDouble learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.001, 1);

            actionVariable = CNTKLib.InputVariable(new int[] { SIZE_OUT }, DataType.Float, "Actions");
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(new Variable(model), actionVariable, "lossFunction");
            var prediction = CNTKLib.ClassificationError(new Variable(model), actionVariable, "classificationError");

            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(model.Parameters(), learningRatePerSample) };
            trainer = Trainer.CreateTrainer(model, trainingLoss, prediction, parameterLearners);
        }

        public void Test()
        {
            int success = 0, total = 0;
            for (int p = 0; p < SIZE_IN; p++)
            {
                for (int c = 0; c < SIZE_IN; c++)
                {
                    if (p != c)
                    {
                        var worldValue = WorldToValue(c, p);

                        inputDataMap[inputVariable] = Value.CreateBatch<float>(model.Arguments[0].Shape, worldValue, device);
                        outputDataMap[model.Output] = null;

                        model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

                        var output = outputDataMap[model].GetDenseData<float>(model)[0];

                        string expected = p > c ? "Left " : "Right";
                        string actual = output[0] > output[1] ? "Left " : "Right";
                        if (expected == actual) success++;
                        total++;
                    }
                }
            }
            Trace.WriteLine($"Success {success}/{total}");
        }

        public void Train()
        {
            uint nbTries = SIZE_IN * (SIZE_IN - 1);
            var values = new float[nbTries * SIZE_IN * SIZE_IN_NB_LAYER];
            var actions = new float[nbTries * SIZE_OUT];

            // Generate training data
            int index = 0;
            for (int p = 0; p < SIZE_IN; p++)
            {
                for (int c = 0; c < SIZE_IN; c++)
                {
                    if (p != c)
                    {
                        var worldValue = WorldToValue(c, p);
                        worldValue.CopyTo(values, index * SIZE_IN * SIZE_IN_NB_LAYER);
                        if (p > c)
                            actions[index * SIZE_OUT] = 1; // Left
                        else
                            actions[index * SIZE_OUT + 1] = 1; // Right
                        index++;
                    }
                }
            }

            // Create Minibatches
            var inputs = Value.CreateBatch<float>(model.Arguments[0].Shape, values, device);
            var inputMinibatch = new MinibatchData(inputs, nbTries);

            var outputs = Value.CreateBatch<float>(model.Output.Shape, actions, device);
            var outputMinibatch = new MinibatchData(outputs, nbTries);


            // Apply learning
            var arguments = new Dictionary<Variable, MinibatchData>
                            {
                                { inputVariable, inputMinibatch },
                                { actionVariable, outputMinibatch }
                            };
            int epoc = 25;
            while (epoc > 0)
            {
                trainer.TrainMinibatch(arguments, device);

                CNTKHelper.CNTKHelper.PrintTrainingProgress(trainer, epoc);

                epoc--;
            }
        }

        private float[] WorldToValue(int coinPos, int playerPos)
        {
            float[] values = new float[2 * SIZE_IN];
            values[coinPos] = 1;
            values[SIZE_IN + playerPos] = 1;
            return values;
        }
    }
}
