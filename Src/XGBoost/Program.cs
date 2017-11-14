using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;

namespace XGBoost
{
	public class Program
	{
		private static float ParseFloat(string str) {
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		private static float[] ParseLabels(string fileName) {
			return File.ReadAllLines(fileName)
				.Select(ParseFloat)
				.ToArray();
		}

		private static DMatrix ParseMatrix(string fileName) {
			string[] lines = File.ReadAllLines(fileName);
			int nrow = lines.Length;
			List<float> values = lines[0].Split(' ').Select(ParseFloat).ToList();
			int ncol = values.Count;
			for (int i = 1; i < lines.Length; i++) {
				values.AddRange(lines[i].Split(' ').Select(ParseFloat));
			}
			if (values.Count != ncol * nrow) {
				throw new Exception("Somthing went wrong during parsing!");
			}
			return new DMatrix(values.ToArray(), nrow, ncol);
		}

		public static void Main(string[] args) {
			Console.WriteLine("Loading data");
			string directory = "D:/ML/RPackage";
			var trainMatrix = ParseMatrix($"{directory}/x_train.txt");
			var testMatrix = ParseMatrix($"{directory}/x_test.txt");
			var trainLabels = ParseLabels($"{directory}/y_train.txt");
			var testLabels = ParseLabels($"{directory}/y_test.txt");
			trainMatrix.Labels = trainLabels;
			testMatrix.Labels = testLabels;

			Console.WriteLine($"Train matrix: {trainMatrix.RowsCount} x {trainMatrix.ColumnsCount}");
			Console.WriteLine($"Test matrix:  {testMatrix.RowsCount} x {testMatrix.ColumnsCount}");

			Booster model;
			try {
				model = XGBoost.Train(trainMatrix, new Dictionary<string, object> {
					{ "booster", "gblinear" },
					{ "eta", "0.1" },
					{ "alpha", "0.0" },
					{ "lambda", "0.8" },
					{ "eval_metric", "merror" },
					{ "objective", "multi:softprob" },
					{ "num_class", 2 }
				}, 200, new Dictionary<string, DMatrix> {
					{ "train", trainMatrix },
					{ "test", testMatrix }
				}, null);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				throw;
			}
			float[] result = model.Predict(testMatrix, false, 0, false);
			float[] predictedResult = new float[result.Length / 2];
			for (int i = 0; i < result.Length; i += 2) {
				predictedResult[i / 2] = result[i + 1] > 0.5 ? 1f : 0f;
			}

			int tp = 0, tn = 0, fp = 0, fn = 0;
			for (int i = 0; i < predictedResult.Length; i++) {
				if (testLabels[i] == predictedResult[i]) {
					if (predictedResult[i] == 1f) tp++; else tn++;
				} else {
					if (predictedResult[i] == 1f) fp++; else fn++;
				}
			}
			Console.WriteLine("\nConfusion matrix for 0.5 probability threshold:\n");
			Console.WriteLine("Predicted:              Disqualified  Sale");
			Console.WriteLine("Actual:  Disqualified {0,14} {1,5}", tn, fp);
			Console.WriteLine("                 Sale {0,14} {1,5}", fn, tp);
			Console.WriteLine("\nAccuracy: {0:0.##}", (tp + tn) * 1.0 / (tp + tn + fp + fn));
		}	

	}

}
