using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGBoost
{
	//TODO: Documentation.
	class XGBoost
	{
		public Booster Train(DMatrix train, Dictionary<string, object> parameters, int round,
				Dictionary<string, DMatrix> watchers, float[][] metrics) {
			int n = watchers.Count;
			string[] evalNames = new string[n];
			DMatrix[] evalMatrices = new DMatrix[n];
			evalMatrices[0] = train;
			int ix = 0;
			foreach (var watcher in watchers) {
				evalNames[ix] = watcher.Key;
				evalMatrices[ix] = watcher.Value;
				ix++;
			}

			DMatrix[] allMatrices = new DMatrix[n + 1];
			allMatrices[0] = train;
			if (evalMatrices.Length > 0) {
				Array.Copy(evalMatrices, 0, allMatrices, 1, n);
			}

			Booster booster = new Booster(parameters, allMatrices);

			for (int iter = 0; iter < round; iter++) {
				booster.Update(train, iter);
				if (n > 0) {
					string evalInfo;
					if (metrics == null) {
						evalInfo = booster.EvalSet(evalMatrices, evalNames, iter);
					} else {
						float[] m = new float[n];
						evalInfo = booster.EvalSet(evalMatrices, evalNames, iter, out m);
						for (int i = 0; i < n; i++) {
							metrics[i][iter] = m[i];
						}
					}
				}
			}
			return booster;
		}

	}
}
