using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CNTK;
using ML.NET.App.PacMan.Model;

namespace ML.NET.App.PacMan.Agents
{
    public class MLAgent : IAgent
    {
        public string Name => "Machine Learning Agent";

        Function model;
        DeviceDescriptor device = DeviceDescriptor.CPUDevice;
        Dictionary<Variable, Value> inputDataMap;
        Dictionary<Variable, Value> outputDataMap;

        Dictionary<Variable, Value> inputTrainBatch;
        Dictionary<Variable, Value> outputTrainBatch;

        Trainer trainer;

        Variable inputVariable;

        Random rnd = new Random();

        int epsilon = 50; // Percentage of random action

        List<State> states = new List<State>();
        public MLAgent()
        {
            int inputLayer = 3; // 1 layer for WALLs - Coins - Player
            int inputSize = World.SIZE;
            int outputSize = Enum.GetValues(typeof(PlayAction)).Length;

            model = CNTKHelper.CNTKHelper.CreateCNNModel(device, inputSize, inputLayer, outputSize);

            inputVariable = model.Arguments.First(a => a.IsInput);
            inputDataMap = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputDataMap = new Dictionary<Variable, Value>() { { model.Output, null } };

            inputTrainBatch = new Dictionary<Variable, Value>() { { inputVariable, null } };
            outputTrainBatch = new Dictionary<Variable, Value>() { { model.Output, null } };


            // set per sample learning rate
            CNTK.TrainingParameterScheduleDouble learningRatePerSample = new CNTK.TrainingParameterScheduleDouble(0.003125, 1);

            var actions = CNTKLib.InputVariable(new int[] { World.PLAY_ACTION_COUNT }, DataType.Float, "Actions");
            var trainingLoss = CNTKLib.CrossEntropyWithSoftmax(new Variable(model), actions, "lossFunction");
            var prediction = CNTKLib.ClassificationError(new Variable(model), actions, "classificationError");

            IList<Learner> parameterLearners = new List<Learner>() { Learner.SGDLearner(model.Parameters(), learningRatePerSample) };
            trainer = Trainer.CreateTrainer(model, trainingLoss, prediction, parameterLearners);
        }
        public void Activate()
        {
            World.Instance.MovePerformed += OnWorldMovePerformed;
            World.Instance.GameCompleted += OnGameCompleted;
        }

        public void Deactivate()
        {
            World.Instance.MovePerformed -= OnWorldMovePerformed;
            World.Instance.GameCompleted -= OnGameCompleted;
            states.Clear();
        }

        private void OnGameCompleted(object sender, EventArgs e)
        {
            Trace.WriteLine("OnGameCompleted");
            previousScore = 0;


            states.Clear();
        }

        private int previousScore = 0;
        private int batchSize = 10;
        private float decay = 0.9f;
        private void OnWorldMovePerformed(World world, PlayAction action)
        {
            // Calculate reward
            state.Reward = world.Score - previousScore;
            previousScore = world.Score;
            //Trace.WriteLine($"OnWorldMovePerformed => {action} Reward = {state.Reward}");

            states.Insert(0, state);
            if (states.Count >= batchSize)
            {
                Trace.WriteLine($"Train batch");

                // Calculate reward and expected output
                float reward = 0;
                var values = new float[states.First().Value.Length * states.Count];
                var actions = new float[World.PLAY_ACTION_COUNT * states.Count];
                int i = 0;
                foreach (var state in states)
                {
                    state.Value.CopyTo(values, i * state.Value.Length);

                    reward = decay * reward + state.Reward;

                    Trace.WriteLine($"Train batch - Reward: {reward}");
                    var expectedActions = CNTKHelper.CNTKHelper.OneHot((int)state.Action, World.PLAY_ACTION_COUNT, reward);
                    expectedActions.CopyTo(actions, i * World.PLAY_ACTION_COUNT);

                    i++;
                }

                // Create Minibatches
                var inputs = Value.CreateBatch<float>(model.Arguments[0].Shape, values, device);
                var inputMinibatch = new MinibatchData(inputs);

                var outputs = Value.CreateBatch<float>(model.Output.Shape, actions, device);
                var outputMinibatch = new MinibatchData(outputs);

                // Apply learning
                var arguments = new Dictionary<Variable, MinibatchData>
                {
                    { inputVariable, inputMinibatch },
                    { model, outputMinibatch }
                };
                trainer.TrainMinibatch(arguments, device);

                // Go for next 
                states.Clear();
            }
        }

        State state;
        public PlayAction Decide(World world)
        {
            // Convert world to model data
            var worldValue = WorldToValue();

            PlayAction action = PlayAction.NOP;
            if (rnd.Next(100) > epsilon)
            {
                // Trace.WriteLine("Calculated Action");

                inputDataMap[inputVariable] = Value.CreateBatch<float>(model.Arguments[0].Shape, worldValue, device);
                outputDataMap[model.Output] = null;

                model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

                var output = outputDataMap[model].GetDenseData<float>(model)[0].ToArray();

                // Convert output to PlayAction
                int maxIndex = CNTKHelper.CNTKHelper.ArgMax(output);

                action = (PlayAction)maxIndex;
            }
            else
            {
                //Trace.WriteLine("Random Action");
                action = RandomAgent.GetRandomAction();
            }
            state = new State
            {
                Value = worldValue,
                Action = action,
                Reward = 0
            };
            return action;
        }

        private float[] WorldToValue()
        {
            var worldValues = World.Instance.Values;
            int worldSurface = World.SIZE * World.SIZE;
            float[] values = new float[3 * worldSurface];

            // Create wall array
            for (int i = 0; i < World.SIZE; i++) // i => Y
            {
                for (int j = 0; j < World.SIZE; j++) // j => X
                {
                    switch (worldValues[i, j])
                    {
                        case 1: // Wall
                            values[i * World.SIZE + j] = 1;
                            break;
                        case 2: // Coin
                            values[worldSurface + i * World.SIZE + j] = 1;
                            break;
                    }
                }
            }
            var p = World.Instance.Pacman.Position;
            values[worldSurface * 2 + p.Y * World.SIZE + p.X] = 1;

            return values;
        }

    }
}
