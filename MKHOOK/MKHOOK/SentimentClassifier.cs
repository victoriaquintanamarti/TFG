using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.Text;

namespace MKHOOK
{
    public class SentimentData
    {
        [LoadColumn(0)]
        public string SentimentText;

        [LoadColumn(1), ColumnName("Label")]
        public bool Sentiment;
    }

    public class SentimentPrediction : SentimentData
    {

        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
    public class SentimentClassifier
    {
        private static string pathString;
        private static string words;
        private TrainTestData splitDataView;
        private static IDataView dataView;

        public void trainClassifier()
        {
            MLContext mlContext = new MLContext();
            splitDataView = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, splitDataView.TrainSet);
            Evaluate(mlContext, model, splitDataView.TestSet);
        }

        public void trainedClassifier()
        {
            MLContext mlContext = new MLContext();
            DataViewSchema modelSchema;
            ITransformer trainedModel = mlContext.Model.Load(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\model.zip", out modelSchema);
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\words.txt");
            pathString = System.IO.Path.Combine(@"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\classifier.txt");
            using (System.IO.FileStream fs = System.IO.File.Create(pathString))
            { }
            Console.WriteLine();
            Console.WriteLine("=============== Prediction Test of model with a single sample and test dataset ===============");
            foreach (string line in lines)
            {
                if (line != "")
                    UseModelWithSingleItem(mlContext, trainedModel, line);
            }

            Console.WriteLine("=============== End of Predictions ===============");

        }

        /*  Carga los datos.
            Divide el conjunto de datos cargado en conjuntos de datos de entrenamiento y prueba.
            Devuelve los conjuntos de datos divididos en entrenamiento y prueba.
         */

        public static TrainTestData LoadData(MLContext mlContext)
        {
            var dataPath = @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\sentimientos2.txt";
            dataView = mlContext.Data.LoadFromTextFile<SentimentData>(dataPath, hasHeader: false);
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            return splitDataView;
        }

        /*  Extrae y transforma los datos.
            Entrena el modelo.
            Predice sentimientos en función de datos de prueba.
            Devuelve el modelo.
        */

        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView splitTrainSet)
        {
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            Console.WriteLine("=============== Create and Train the Model ===============");
            var model = estimator.Fit(splitTrainSet);
            mlContext.Model.Save(model, dataView.Schema, @"C:\Users\Victoria\Documents\IngenieríaInformática\TFG\TFG\MKHOOK\MKHOOK\bin\x86\Debug\model.zip");
            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();
            return model;
        }
        public static void Evaluate(MLContext mlContext, ITransformer model, IDataView splitTestSet)
        {
            Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            IDataView predictions = model.Transform(splitTestSet);
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");

        }

        private static void UseModelWithSingleItem(MLContext mlContext, ITransformer model, string line)
        {

            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = line 
            };

            var resultprediction = predictionFunction.Predict(sampleStatement);

            Console.WriteLine();
            words = words + $"Sentiment: {resultprediction.SentimentText} | Prediction: {(Convert.ToBoolean(resultprediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultprediction.Probability}\r\n";
            File.WriteAllText(pathString, words);
            Console.WriteLine($"Sentiment: {resultprediction.SentimentText} | Prediction: {(Convert.ToBoolean(resultprediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultprediction.Probability} ");

        }

    }
}
