// <SnippetAddUsings>
using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
// </SnippetAddUsings>

namespace Stocks.ML.SentimentAnalysis
{
    public interface ISentimentAnalysisService
    {
        PredictionResult Analyze(string text);
    }

    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "yelp_labelled.txt");

        private MLContext _mlContext;
        private ITransformer _model;

        public SentimentAnalysisService()
        {
            // Create ML.NET context/local environment - allows you to add steps in order to keep everything together
            // as you discover the ML.NET trainers and transforms
            // <SnippetCreateMLContext>
            _mlContext = new MLContext();
            // </SnippetCreateMLContext>

            // <SnippetCallLoadData>
            TrainTestData splitDataView = LoadData();
            // </SnippetCallLoadData>

            // <SnippetCallBuildAndTrainModel>
            _model = BuildAndTrainModel(splitDataView.TrainSet);
            // </SnippetCallBuildAndTrainModel>

            // <SnippetCallEvaluate>
            Evaluate(splitDataView.TestSet);
            // </SnippetCallEvaluate>
        }
        
        public PredictionResult Analyze(string text)
        {
            // <SnippetCreatePredictionEngine1>
            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction =
                _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
            // </SnippetCreatePredictionEngine1>

            // <SnippetCreateTestIssue1>
            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = text
            };
            // </SnippetCreateTestIssue1>

            // <SnippetPredict>
            var resultPrediction = predictionFunction.Predict(sampleStatement);
            // </SnippetPredict>
            // <SnippetOutputPrediction>
            //Console.WriteLine($"Sentiment: {resultPrediction.SentimentText} | Prediction: {(Convert.ToBoolean(resultPrediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultPrediction.Probability} ");

            return new PredictionResult() { IsPositive = resultPrediction.Prediction, Probability = resultPrediction.Probability };
            // </SnippetOutputPrediction>
        }

        private TrainTestData LoadData()
        {
            // Note that this case, loading your training data from a file,
            // is the easiest way to get started, but ML.NET also allows you
            // to load data from databases or in-memory collections.
            // <SnippetLoadData>
            IDataView dataView = _mlContext.Data.LoadFromTextFile<SentimentData>(_dataPath, hasHeader: false);
            // </SnippetLoadData>

            // You need both a training dataset to train the model and a test dataset to evaluate the model.
            // Split the loaded dataset into train and test datasets
            // Specify test dataset percentage with the `testFraction`parameter
            // <SnippetSplitData>
            TrainTestData splitDataView = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            // </SnippetSplitData>

            // <SnippetReturnSplitData>
            return splitDataView;
            // </SnippetReturnSplitData>
        }

        private ITransformer BuildAndTrainModel(IDataView splitTrainSet)
        {
            // Create a flexible pipeline (composed by a chain of estimators) for creating/training the model.
            // This is used to format and clean the data.
            // Convert the text column to numeric vectors (Features column)
            // <SnippetFeaturizeText>
            var estimator = _mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
            //</SnippetFeaturizeText>
            // append the machine learning task to the estimator
            // <SnippetAddTrainer>
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
            // </SnippetAddTrainer>

            // Create and train the model based on the dataset that has been loaded, transformed.
            // <SnippetTrainModel>
            var model = estimator.Fit(splitTrainSet);
            // </SnippetTrainModel>

            // Returns the model we trained to use for evaluation.
            // <SnippetReturnModel>
            return model;
            // </SnippetReturnModel>
        }

        private CalibratedBinaryClassificationMetrics Evaluate(IDataView splitTestSet)
        {
            // Evaluate the model and show accuracy stats

            //Take the data in, make transformations, output the data.
            // <SnippetTransformData>
            IDataView predictions = _model.Transform(splitTestSet);
            // </SnippetTransformData>

            // BinaryClassificationContext.Evaluate returns a BinaryClassificationEvaluator.CalibratedResult
            // that contains the computed overall metrics.
            // <SnippetEvaluate>
            CalibratedBinaryClassificationMetrics metrics = _mlContext.BinaryClassification.Evaluate(predictions, "Label");
            // </SnippetEvaluate>

            // The Accuracy metric gets the accuracy of a model, which is the proportion
            // of correct predictions in the test set.

            // The AreaUnderROCCurve metric is equal to the probability that the algorithm ranks
            // a randomly chosen positive instance higher than a randomly chosen negative one
            // (assuming 'positive' ranks higher than 'negative').

            // The F1Score metric gets the model's F1 score.
            // The F1 score is the harmonic mean of precision and recall:
            //  2 * precision * recall / (precision + recall).

            // <SnippetDisplayMetrics>
            //Console.WriteLine("Model quality metrics evaluation");
            //Console.WriteLine("--------------------------------");
            //Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            //Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            //Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            //Console.WriteLine("=============== End of model evaluation ===============");
            //</SnippetDisplayMetrics>

            return metrics;
        }
    }
}
