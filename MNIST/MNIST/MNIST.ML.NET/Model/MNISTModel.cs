using Microsoft.ML;
using MNIST.ML.NET.DataUtils;
using System.Collections.Generic;

namespace MNIST.ML.NET.Model
{
    public class MNISTModel
    {
        SortedDictionary<byte, double[]> oneHotLabel = new SortedDictionary<byte, double[]>();

        MLContext mLContext = new MLContext();


        public void Train(IEnumerable<MNISTData> images)
        {
            // Create a new context for ML.NET operations. It can be used for exception tracking and logging,
            // as a catalog of available operations and as the source of randomness.

            // Turn the data into the ML.NET data view.
            // We can use CreateDataView or CreateStreamingDataView, depending on whether 'churnData' is an IList,
            // or merely an IEnumerable.
            var trainData = mLContext.Data.LoadFromEnumerable(images);

            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mLContext.Transforms.Concatenate(nameof(MNISTData.Feature), nameof(MNISTData.Values));

            //// STEP 3: Set the training algorithm, then create and config the modelBuilder
            //var trainer = mLContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(labelColumn: "Feature", featureColumn: DefaultColumnNames.Features);
            //var trainingPipeline = dataProcessPipeline.Append(trainer);

            //// STEP 4: Train the model fitting to the DataSet
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //ITransformer trainedModel = trainingPipeline.Fit(trainData);
            //long elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine($"***** Training time: {elapsedMs / 1000} seconds *****");

        }

    }
}
