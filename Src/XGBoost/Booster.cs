namespace XGBoost
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Runtime.InteropServices;

	class Booster
	{
		private IntPtr _handle;

		/// <summary>
		/// Create a new Booster with empty stage.
		/// </summary>
		/// <param name="parameters">Model parameters.</param>
		/// <param name="cacheMatrices">
		/// Cached <see cref="DMatrix"/> entries,
		/// the prediction of these matrics will become faster, than not-cached data.
		/// </param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public Booster(Dictionary<string, object> parameters, DMatrix[] cacheMatrices) {
			IntPtr[] matrixHandles = cacheMatrices.Select(matrix => matrix.GetHandle()).ToArray();
			int exitCode = XGBoostNative.XGBoosterCreate(matrixHandles, (ulong)matrixHandles.Length, out _handle);
			XGBoostError.CheckError(exitCode);
			SetParameter("seed", "0");
			SetParameters(parameters);
		}

		/// <summary>
		/// Loads a new booster from given file.
		/// </summary>
		/// <param name="path">The path to the model.</param>
		/// <returns>The created <see cref="Booster"/>.</returns>
		/// <exception cref="ArgumentNullException">If path is null.</exception>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public static Booster LoadModel(string path) {
			if (path == null) {
				throw new ArgumentNullException("path");
			}
			Booster booster = new Booster(new Dictionary<string, object>(), new DMatrix[0]);
			int exitCode = XGBoostNative.XGBoosterLoadModel(booster._handle, path);
			XGBoostError.CheckError(exitCode);
			return booster;
		}

		/// <summary>
		/// Set parameter to the booster.
		/// </summary>
		/// <param name="key">Parameter name.</param>
		/// <param name="value">Parameter value.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public void SetParameter(string key, object value) {
			int exitCode = XGBoostNative.XGBoosterSetParam(_handle, key, value.ToString());
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Set parameters to the booster.
		/// </summary>
		/// <param name="parameters">Parameters dictionary.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public void SetParameters(Dictionary<string, object> parameters) {
			if (parameters != null) {
				foreach (var entry in parameters) {
					SetParameter(entry.Key, entry.Value.ToString());
				}
			}
		}

		/// <summary>
		/// Update the booster for one iteration.
		/// </summary>
		/// <param name="train">Training data.</param>
		/// <param name="iter">Current iteration number.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public void Update(DMatrix train, int iter) {
			int exitCode = XGBoostNative.XGBoosterUpdateOneIter(_handle, iter, train.GetHandle());
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Update with given gradient and hessian.
		/// </summary>
		/// <param name="train">Training data.</param>
		/// <param name="grad">First order of gradient.</param>
		/// <param name="hess">Second order of gradient.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public void Boost(DMatrix train, float[] grad, float[] hess) {
			if (grad.Length != hess.Length) {
				throw new ArgumentException(string.Format("grad/hess length differ {0}/{1}", grad.Length, hess.Length));
			}
			int exitCode = XGBoostNative.XGBoosterBoostOneIter(_handle, train.GetHandle(), grad, hess, (ulong)grad.Length);
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Evaluate with given matrices.
		/// </summary>
		/// <param name="evalMatrices">Matrices for evaluation.</param>
		/// <param name="evalNames">Name for eval matrices, used to check results.</param>
		/// <param name="iter">Current evaluation iteration.</param>
		/// <returns>Eval information</returns>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public string EvalSet(DMatrix[] evalMatrices, string[] evalNames, int iter) {
			if (evalMatrices.Length != evalNames.Length) {
				throw new ArgumentException(string.Format("evalMatrices/evalNames length differ {0}/{1}", evalMatrices.Length, evalNames.Length));
			}
			IntPtr[] handles = evalMatrices.Select(matrix => matrix.GetHandle()).ToArray();
			string evalInfo;
			int exitCode = XGBoostNative.XGBoosterEvalOneIter(_handle, iter, handles, evalNames, (ulong)handles.Length, out evalInfo);
			XGBoostError.CheckError(exitCode);
			return evalInfo;
		}

		//TODO: Documentation.
		public string EvalSet(DMatrix[] evalMatrices, string[] evalNames, int iter, out float[] metricsOut) {
			throw new NotImplementedException();
		}

		// TODO: Make private.
		public float[] Predict(DMatrix data, bool outputMargin, int treeLimit, bool predLeaf) {
			int optionMask = outputMargin ? 1 : 0;
			optionMask = predLeaf ? 2 : optionMask;
			ulong predictionsLength;
			IntPtr predictionsPtr;
			int exitCode = XGBoostNative.XGBoosterPredict(_handle, data.GetHandle(), optionMask, treeLimit,
				out predictionsLength, out predictionsPtr);
			XGBoostError.CheckError(exitCode);
			int length = (int)predictionsLength;
			float[] rawPredicts = new float[length];
			Marshal.Copy(predictionsPtr, rawPredicts, 0, length);
			return rawPredicts;
		}

		//TODO: Documentation.
		public float[][] PredictLeaf(DMatrix data, int treeLimit) {
			throw new NotImplementedException();
		}

		//TODO: Documentation.
		public float[][] Predict(DMatrix data) {
			throw new NotImplementedException();
		}

		//TODO: Documentation.
		public float[][] Predict(DMatrix data, bool outputMargin) {
			throw new NotImplementedException();
		}

		//TODO: Documentation.
		public float[][] Predict(DMatrix data, bool outputMargin, int treeLimit) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Saves model to given path.
		/// </summary>
		/// <param name="modelPath">Path.</param>
		public void SaveModel(string modelPath) {
			int exitCode = XGBoostNative.XGBoosterSaveModel(_handle, modelPath);
			XGBoostError.CheckError(exitCode);
		}

		//TODO: Documentation.
		public string[] GetModelDump(string featureName, bool withStats) {
			throw new NotImplementedException();
		}

		//TODO: Documentation.
		public Dictionary<string, int> GetFeatureScore(string featureMap) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Implementation of Dispose pattern
		/// </summary>
		/// <param name="disposing">If managed resouses dispose required.</param>
		protected virtual void Dispose(bool disposing) {
			if (_handle != IntPtr.Zero) {
				XGBoostNative.XGBoosterFree(_handle);
				_handle = IntPtr.Zero;
			}
		}

		~Booster() {
			Dispose(false);
		}

		/// <summary>
		/// Implementation of <see cref="IDisposable"/>.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
