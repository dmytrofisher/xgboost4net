using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGBoost
{
	class XGBClassifier
	{
		private Dictionary<string, object> _parameters = new Dictionary<string, object>();
		private Booster _booster;

		public XGBClassifier(int maxDepth = 3, float learningRate = 0.1F, int nEstimators = 100,
				bool silent = true, string objective = "binary:logistic",
				int nThread = -1, float gamma = 0, int minChildWeight = 1,
				int maxDeltaStep = 0, float subsample = 1, float colSampleByTree = 1,
				float colSampleByLevel = 1, float regAlpha = 0, float regLambda = 1,
				float scalePosWeight = 1, float baseScore = 0.5F, int seed = 0,
				float missing = float.NaN, float numClass = 0) {
			_parameters["max_depth"] = maxDepth;
			_parameters["learning_rate"] = learningRate;
			_parameters["n_estimators"] = nEstimators;
			_parameters["silent"] = silent;
			_parameters["objective"] = objective;

			_parameters["nthread"] = nThread;
			_parameters["gamma"] = gamma;
			_parameters["min_child_weight"] = minChildWeight;
			_parameters["max_delta_step"] = maxDeltaStep;
			_parameters["subsample"] = subsample;
			_parameters["colsample_bytree"] = colSampleByTree;
			_parameters["colsample_bylevel"] = colSampleByLevel;
			_parameters["reg_alpha"] = regAlpha;
			_parameters["reg_lambda"] = regLambda;
			_parameters["scale_pos_weight"] = scalePosWeight;

			_parameters["base_score"] = baseScore;
			_parameters["seed"] = seed;
			_parameters["missing"] = missing;
			_parameters["_Booster"] = null;
		}

		public XGBClassifier(Dictionary<string, object> parameters) {
			_parameters = parameters;
		}

		public void Fit(float[][] data, float[] labels) {
			throw new NotImplementedException();

		}

	}
}
