using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XGBoost
{
	public class XGBoostNative
	{
		private const string XGBoostLib = "libxgboost.dll";

		/// <summary>
		/// Get string message of the last error.
		/// All functions native functions will return 0 when success
		/// and -1 when an error occured. This method can be called
		/// to retrieve the error.
		/// </summary>
		/// <returns></returns>
		[DllImport(XGBoostLib)]
		public static extern string XGBGetLastError();

		/// <summary>
		/// Loads a data matrix from a specified file.
		/// </summary>
		/// <param name="fname">The name of the file.</param>
		/// <param name="silent">Whetthe print messages dusring loading.</param>
		/// <param name="DMtrxHandle">A loaded data matrix.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixCreateFromFile(
			string fname,
			int silent,
			out IntPtr DMtrxHandle);

		/// <summary>
		/// Create matrix content from dense matrix.
		/// </summary>
		/// <param name="data">Pointer to the data space.</param>
		/// <param name="nrow">Number of rows.</param>
		/// <param name="ncol">Number of columns.</param>
		/// <param name="missing">Which value to represent missing value.</param>
		/// <param name="handle">Pointer to created <see cref="DMatrix"/>.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixCreateFromMat(
			float[] data,
			ulong nrow,
			ulong ncol,
			float missing,
			out IntPtr handle);

		/// <summary>
		/// Create a mtrix content from CSC (Compressed Sparse Column) format.
		/// </summary>
		/// <param name="colPtr">Column headers - cumulative sum of presenting entries per column, starting with 0.</param>
		/// <param name="indicies">The row indices of presenting entries.</param>
		/// <param name="data">Matrix data.</param>
		/// <param name="nindptr">Number of columns in a matrix + 1.</param>
		/// <param name="nelem">Number of nonzero elements in the matrix.</param>
		/// <param name="numRow">Number of rows. When it's set to 0, then guess from data.</param>
		/// <param name="handle">Pointer to created matrix.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixCreateFromCSCEx(
			UIntPtr[] colPtr,
			int[] indicies,
			float[] data,
			UIntPtr nindptr,
			UIntPtr nelem,
			UIntPtr numRow,
			out IntPtr handle);

		/// <summary>
		/// Create a mtrix content from CSC (Compressed Sparse Column) format.
		/// </summary>
		/// <param name="rowPtr">Row headers - cumulative sum of presenting entries per row, starting with 0.</param>
		/// <param name="indicies">The column indices of presenting entries.</param>
		/// <param name="data">Matrix data.</param>
		/// <param name="nindptr">Number of rows in the matrix + 1.</param>
		/// <param name="nelem">Number of nonzero elements in the matrix.</param>
		/// <param name="numCol">Number of columns. When it's set to 0, then guess from data.</param>
		/// <param name="handle"></param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixCreateFromCSREx(
			UIntPtr[] rowPtr,
			int[] indicies,
			float[] data,
			UIntPtr nindptr,
			UIntPtr nelem,
			UIntPtr numCol,
			out IntPtr handle);

		/// <summary>
		/// Get number of rows.
		/// </summary>
		/// <param name="handle">Pointer to the matrix.</param>
		/// <param name="nrow">Address to hold number of rows.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixNumRow(IntPtr handle, out ulong nrow);

		/// <summary>
		/// Get the number of columns.
		/// </summary>
		/// <param name="handle">The pointer to the matrix.</param>
		/// <param name="ncol">The address to hold number of columns.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixNumCol(IntPtr handle, out ulong ncol);

		/// <summary>
		/// Free space in data matrix.
		/// </summary>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixFree(IntPtr handle);

		/// <summary>
		/// Get float info vector from matrix.
		/// </summary>
		/// <param name="handle">The pointer to the matrix.</param>
		/// <param name="field">Field name.</param>
		/// <param name="length">Address to set the result length.</param>
		/// <param name="result">Pointer to the result.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixGetFloatInfo(
			IntPtr handle,
			string field,
			out ulong length,
			out IntPtr result);

		/// <summary>
		/// Set float vector to a content in info.
		/// </summary>
		/// <param name="handle">Pointer to the matrix.</param>
		/// <param name="field">Field name. Can be "label", "weight".</param>
		/// <param name="array">Array of values to set.</param>
		/// <param name="length">Length of array.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixSetFloatInfo(
			IntPtr handle,
			string field,
			float[] array,
			ulong length);

		/// <summary>
		/// Creates a new <see cref="DMatrix"/> from sliced content of existing matrix.
		/// </summary>
		/// <param name="handle">Pointer to the matrix to be sliced.</param>
		/// <param name="idxset">Index set.</param>
		/// <param name="length">Length of index set.</param>
		/// <param name="sliced">New sliced <see cref="DMatrix"/>.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGDMatrixSliceDMatrix(
			IntPtr handle,
			int[] idxset,
			ulong length,
			out IntPtr sliced);

		/// <summary>
		/// Creates XGBoost learner.
		/// </summary>
		/// <param name="dmats">Matrices, that are set to be cached.</param>
		/// <param name="len">Length of matrices array.</param>
		/// <param name="handle">Handle to created <see cref="Booster"/>.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterCreate(
			IntPtr[] dmats,
			ulong len,
			out IntPtr handle);

		/// <summary>
		/// Free the model.
		/// </summary>
		/// <param name="handle">The pointer to the booster.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterFree(IntPtr handle);

		/// <summary>
		/// Set booster parameter.
		/// </summary>
		/// <param name="handle">The pointer to the booster.</param>
		/// <param name="name">Parameter name.</param>
		/// <param name="val">Parameter value to set.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterSetParam(
			IntPtr handle,
			string name,
			string val);

		/// <summary>
		/// Update the model in one round using training matrix.
		/// </summary>
		/// <param name="bHandle">The pointer to the model.</param>
		/// <param name="iter">Current iteration rounds.</param>
		/// <param name="dHandle">The pointer to the training data.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterUpdateOneIter(
			IntPtr bHandle,
			int iter,
			IntPtr dHandle);

		/// <summary>
		/// Update the model, by directly specify gradient and second order gradient,
		/// this can be used to replcae <see cref="XGBoostNative.XGBoosterUpdateOneIter"/>,
		/// to support customized loss function.
		/// </summary>
		/// <param name="bHandle">The pointer to the model.</param>
		/// <param name="dHandle">The pointer to the training data.</param>
		/// <param name="grad">Gradient statistics.</param>
		/// <param name="hess">Second order gradient statistics.</param>
		/// <param name="len">Length of gradient arrays.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterBoostOneIter(
			IntPtr bHandle,
			IntPtr dHandle,
			float[] grad,
			float[] hess,
			ulong len);

		/// <summary>
		/// Get evaluation statistics.
		/// </summary>
		/// <param name="bHandle">The pointer to the model.</param>
		/// <param name="iter">Current iteration rounds.</param>
		/// <param name="dMtrices">Matrices, that are set to be evaluated.</param>
		/// <param name="names">Name of each matrix.</param>
		/// <param name="len">Length of matrices array.</param>
		/// <param name="result">String, containing evaluation statistics.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterEvalOneIter(
			IntPtr bHandle,
			int iter,
			IntPtr[] dMtrices,
			string[] names,
			ulong len,
			out string result);


		/// <summary>
		/// Make predictions based on given matrix.
		/// </summary>
		/// <param name="bHandle">The pointer to the model.</param>
		/// <param name="dHandle">Data matrix.</param>
		/// <param name="optionMask">
		/// Bit-mask of options taken in prediction, possible values:
		/// 0 - Normal prediction
		/// 1 - Output margin instead of transformed value
		/// 2 - Leaf index of trees instead of leaf value, note leaf index is unique per tree.
		/// </param>
		/// <param name="ntreeLimit">
		/// Limit number of trees used for prediction, this is only valid for boosted trees.
		/// When parameter is set to 0, all the trees will be used.
		/// </param>
		/// <param name="predsLen">Used to store length of returning result.</param>
		/// <param name="predsPtr">Used to set a pointer to array.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterPredict(
			IntPtr bHandle,
			IntPtr dHandle,
			int optionMask,
			int ntreeLimit,
			out ulong predsLen,
			out IntPtr predsPtr);

		//TODO: Documentation
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterSaveModel(IntPtr bHandle, string fileName);

		/// <summary>
		/// Load model from existing file.
		/// </summary>
		/// <param name="bHandle">Booster handle to store the loaded data.</param>
		/// <param name="fileName">File name.</param>
		/// <returns>0 when success, -1 otherwise.</returns>
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterLoadModel(
			IntPtr bHandle,
			string fileName);

		//TODO: Documentation
		[DllImport(XGBoostLib)]
		public static extern int XGBoosterDumpModel(
			IntPtr handle,
			string fmap,
			int with_stats,
			out int out_len,
			out string[] dumpStr);
	}

}
