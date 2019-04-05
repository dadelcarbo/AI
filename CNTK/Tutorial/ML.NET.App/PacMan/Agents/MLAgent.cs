using System;
using System.Collections.Generic;
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
            Console.WriteLine("OnGameCompleted");
            previousScore = 0;


            states.Clear();
        }

        private int previousScore = 0;
        private int batchSize = 10;
        private void OnWorldMovePerformed(World world, PlayAction action)
        {
            // Calculate reward
            state.Reward = world.Score - previousScore;
            previousScore = world.Score;
            //Console.WriteLine($"OnWorldMovePerformed => {action} Reward = {state.Reward}");

            states.Add(state);
            if (states.Count >= batchSize)
            {
                Console.WriteLine($"Train batch");

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
                // Console.WriteLine("Calculated Action");

                inputDataMap[inputVariable] = worldValue;
                outputDataMap[model.Output] = null;

                model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

                var output = outputDataMap[model].GetDenseData<float>(model)[0].ToArray();

                // Convert output to PlayAction
                int maxIndex = 0;
                float max = output[0];
                for (int i = 1; i < output.Length; i++)
                {
                    if (output[i] > max)
                    {
                        maxIndex = i;
                        max = output[i];
                    }
                }

                action = (PlayAction)maxIndex;
            }
            else
            {
                //Console.WriteLine("Random Action");
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

        private Value WorldToValue()
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

            return Value.CreateBatch<float>(model.Arguments[0].Shape, values, device);
        }

    }
}
