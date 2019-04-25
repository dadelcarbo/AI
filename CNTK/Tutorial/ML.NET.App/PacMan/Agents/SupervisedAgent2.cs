using CNTK;
using ML.NET.App.PacMan.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ML.NET.App.PacMan.Agents
{
    public class SupervisedAgent2 : IAgent
    {
        public string Name => "No Border Agent";

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
        public SupervisedAgent2()
        {
            int inputLayer = 2; // 1 layer for WALLs - Coins - Player
            int inputSize = World.SIZE - 2;
            int outputSize = Enum.GetValues(typeof(PlayAction)).Length;

            model = CNTKHelper.CNTKHelper.CreateMLPModel2D(device, inputSize, inputLayer, outputSize);

            inputVariable = model.Arguments.First(a => a.IsInput);
            inputDataMap = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputDataMap = new Dictionary<Variable, Value>() { { model.Output, null } };

            inputTrainBatch = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputTrainBatch = new Dictionary<Variable, Value>() { { model.Output, null } };

            // Set per sample learning rate
            CNTK.TrainingParameterScheduleDouble learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.01, 1);

            actionVariable = CNTKLib.InputVariable(new int[] { World.PLAY_ACTION_COUNT }, DataType.Float, "Actions");
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(new Variable(model), actionVariable, "lossFunction");
            var prediction = CNTKLib.ClassificationError(new Variable(model), actionVariable, "classificationError");

            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(model.Parameters(), learningRatePerSample) };
            trainer = Trainer.CreateTrainer(model, trainingLoss, prediction, parameterLearners);
        }

        bool trained = false;
        public void Activate()
        {
            if (!trained)
            {
                Train();
            }
        }

        private void Train()
        {
            uint SIZE_IN_NB_LAYER = 2;
            uint SIZE_IN = (World.SIZE - 2) * (World.SIZE - 2);
            uint nbTries = (World.SIZE - 2) * (World.SIZE - 2) * (World.SIZE - 2) * (World.SIZE - 2) - (World.SIZE - 2) * (World.SIZE - 2);

            var values = new float[nbTries * SIZE_IN * SIZE_IN_NB_LAYER];
            var actions = new float[nbTries * World.PLAY_ACTION_COUNT];

            int index = 0;
            for (int i = 1; i < World.SIZE - 1; i++) // i => Y
            {
                for (int j = 1; j < World.SIZE - 1; j++) // j => X
                {
                    var coinPosition = new Position(j, i);

                    for (int k = 1; k < World.SIZE - 1; k++)
                    {
                        for (int l = 1; l < World.SIZE - 1; l++)
                        {
                            if (k == i && l == j) continue;

                            var playerPosition = new Position(l, k);

                            var worldValue = WorldToValue(coinPosition, playerPosition);
                            worldValue.CopyTo(values, index * SIZE_IN * SIZE_IN_NB_LAYER);

                            var dx = coinPosition.X - playerPosition.X;
                            var dy = coinPosition.Y - playerPosition.Y;

                            if (Math.Abs(dx) > Math.Abs(dy))
                            {
                                if (dx > 0)
                                    actions[index * World.PLAY_ACTION_COUNT + (int)PlayAction.Right] = 1;
                                else
                                    actions[index * World.PLAY_ACTION_COUNT + (int)PlayAction.Left] = 1;
                            }
                            else if (Math.Abs(dx) < Math.Abs(dy))
                            {
                                if (dy > 0)
                                    actions[index * World.PLAY_ACTION_COUNT + (int)PlayAction.Down] = 1;
                                else
                                    actions[index * World.PLAY_ACTION_COUNT + (int)PlayAction.Up] = 1;
                            }

                            index++;
                        }
                    }
                }
            }
            Trace.WriteLine($"NbTries={nbTries} Index {index}");

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
            int epoc = 0;
            while (epoc < 50)
            {
                trainer.TrainMinibatch(arguments, device);

                CNTKHelper.CNTKHelper.PrintTrainingProgress(trainer, epoc);

                //float trainLossValue = (float)trainer.PreviousMinibatchLossAverage();
                //if (trainLossValue < 0.005)
                //    break;

                epoc++;

                // Test

                inputDataMap[inputVariable] = inputs;
                outputDataMap[model.Output] = null;

                model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

                var testOutputs = outputDataMap[model].GetDenseData<float>(model);
                int testId = 0;
                int success = 0;
                foreach (var actualValues in testOutputs)
                {
                    var expectedValues = actions.Skip(testId * World.PLAY_ACTION_COUNT).Take(World.PLAY_ACTION_COUNT).ToArray();
                    var expectedAction = (PlayAction)CNTKHelper.CNTKHelper.ArgMax(expectedValues);
                    var actualAction = (PlayAction)CNTKHelper.CNTKHelper.ArgMax(actualValues);

                    if (actualAction == expectedAction)
                        success++;
                }
                Trace.WriteLine($"Success {success}/{testOutputs.Count}");
            }

            trained = true;
        }
        public void Deactivate()
        {
        }

        private void OnGameCompleted(object sender, EventArgs e)
        {
            Trace.WriteLine("OnGameCompleted");
            previousScore = 0;
        }

        private int previousScore = 0;
        private int batchSize = 10;
        private float decay = 0.9f;

        State state;
        public PlayAction Decide(World world)
        {
            // Convert world to model data
            var worldValue = WorldToValue();

            PlayAction action = PlayAction.NOP;
            int odd = rnd.Next(100);
            if (odd >= world.Epsilon)
            {
                // Trace.WriteLine("Calculated Action");

                inputDataMap[inputVariable] = Value.CreateBatch<float>(model.Arguments[0].Shape, worldValue, device);
                outputDataMap[model.Output] = null;

                model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

                var output = outputDataMap[model].GetDenseData<float>(model)[0];

                // Convert output to PlayAction
                int maxIndex = CNTKHelper.CNTKHelper.ArgMax(output);
                if (maxIndex == -1)
                    return PlayAction.NOP;
                action = (PlayAction)maxIndex;
            }
            else
            {
                //Trace.WriteLine("Random Action");
                action = RandomAgent.GetRandomAction();

            }
            return action;
        }

        private float[] WorldToValue()
        {
            var worldValues = World.Instance.Values;
            int size = World.SIZE - 2;
            int worldSurface = size * size;
            int nbLayers = 2;
            float[] values = new float[nbLayers * worldSurface];

            // Create wall array
            for (int i = 1; i < World.SIZE - 1; i++) // i => Y
            {
                for (int j = 1; j < World.SIZE - 1; j++) // j => X
                {
                    switch (worldValues[i * World.SIZE + j])
                    {
                        case 2: // Coin
                            values[(i - 1) * size + (j - 1)] = 1;
                            break;
                    }
                }
            }
            var p = World.Instance.Pacman.Position;
            values[worldSurface + (p.Y - 1) * size + (p.X - 1)] = 1;

            //TraceValues(values, "WorldToValue");

            return values;
        }

        private float[] WorldToValue(Position coinPosition, Position playerPosition)
        {
            int size = World.SIZE - 2;
            int worldSurface = size * size;
            int nbLayers = 2;
            float[] values = new float[nbLayers * worldSurface];

            values[(coinPosition.Y - 1) * size + (coinPosition.X - 1)] = 1;
            values[worldSurface + (playerPosition.Y - 1) * size + (playerPosition.X - 1)] = 1;

            //TraceValues(values, "WorldToValue");

            return values;
        }

        private static void TraceValues(float[] values, string header)
        {
            Trace.WriteLine(header);
            int size = World.SIZE - 2;
            int worldSurface = size * size;
            for (int i = 0; i < size; i++) // i => Y
            {
                var dump = string.Empty;
                for (int j = 0; j < size; j++) // j => X
                {
                    if (values[i * size + j] != 0)
                    {
                        dump += 2;
                    }
                    else if (values[worldSurface + i * size + j] != 0)
                    {
                        dump += 9;
                    }
                    else
                    {
                        dump += 0;
                    }
                }
                Trace.WriteLine(dump);
            }
        }
    }
}
