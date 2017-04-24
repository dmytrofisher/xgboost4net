using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace XGBoost
{
	/// <summary>
	/// Represent DMatrix for XGBoost.
	/// </summary>
	public class DMatrix : IDisposable
	{
		private IntPtr _handle = IntPtr.Zero;

		/// <summary>
		/// Sparse matrix storage type.
		/// </summary>
		public enum SparseType
		{
			/// <summary>
			/// Compressed sparse row.
			/// </summary>
			CSR,

			/// <summary>
			/// Compressed sparse column.
			/// </summary>
			CSC
		}

		/// <summary>
		/// Creates an instance of <see cref="DMatrix"/> from specified file in LibSVM format.
		/// </summary>
		/// <param name="filePath">The path to the data.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public DMatrix(string filePath) {
			if (filePath == null) {
				throw new ArgumentNullException(nameof(filePath));
			}
			int exitCode = XGBoostNative.XGDMatrixCreateFromFile(filePath, 1, out _handle);
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Creates an instance of <see cref="DMatrix"/> from a given dense matrix.
		/// </summary>
		/// <param name="data">Matrix values.</param>
		/// <param name="nrow">Number of rows.</param>
		/// <param name="ncol">Number of columns.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public DMatrix(float[] data, int nrow, int ncol) {
			int exitCode = XGBoostNative.XGDMatrixCreateFromMat(data, (ulong)nrow, (ulong)ncol, 0.0f, out _handle);
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Creates an instance of <see cref="DMatrix"/> from a given dense matrix.
		/// </summary>
		/// <param name="data">Matrix values.</param>
		/// <param name="nrow">Number of rows.</param>
		/// <param name="ncol">Number of columns.</param>
		/// <param name="missing">The specified value to represent the missing value.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public DMatrix(float[] data, int nrow, int ncol, float missing) {
			int exitCode = XGBoostNative.XGDMatrixCreateFromMat(data, (ulong)nrow, (ulong)ncol, missing, out _handle);
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// This constructor is used for matrix slice.
		/// </summary>
		/// <param name="handle">Native matrix pointer.</param>
		protected DMatrix(IntPtr handle) {
			_handle = handle;
		}

		/// <summary>
		/// Set labels for <see cref="DMatrix"/>.
		/// </summary>
		/// <param name="labels">Array of labels.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public void SetLabels(float[] labels) {
			int exitCode = XGBoostNative.XGDMatrixSetFloatInfo(_handle, "label", labels, (ulong)labels.Length);
			XGBoostError.CheckError(exitCode);
		}

		private float[] GetFloatInfo(string field) {
			ulong length;
			IntPtr resultPtr;
			int exitCode = XGBoostNative.XGDMatrixGetFloatInfo(_handle, field, out length, out resultPtr);
			XGBoostError.CheckError(exitCode);			
			float[] floatInfo = new float[(int)length];
			Marshal.Copy(resultPtr, floatInfo, 0, (int)length);
			return floatInfo;
		}

		/// <summary>
		/// Gets matrix labels.
		/// </summary>
		/// <returns>Labels array.</returns>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public float[] GetLabels() {
			return GetFloatInfo("label");
		}

		public DMatrix Slice(int[] rowIndex) {
			IntPtr slicedPtr;
			int exitCode = XGBoostNative.XGDMatrixSliceDMatrix(_handle, rowIndex, (ulong)rowIndex.Length, out slicedPtr);
			XGBoostError.CheckError(exitCode);
			return new DMatrix(slicedPtr);
		}

		/// <summary>
		/// Get the number of rows.
		/// </summary>
		/// <returns>Number of rows in the matrix.</returns>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public int GetRowNumber() {
			ulong nrow;
			int exitCode = XGBoostNative.XGDMatrixNumRow(_handle, out nrow);
			return (int)nrow;
		}

		/// <summary>
		/// Get the number of columns.
		/// </summary>
		/// <returns>Number of columns in the matrix.</returns>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public int GetColNumber() {
			ulong ncol;
			int exitCode = XGBoostNative.XGDMatrixNumCol(_handle, out ncol);
			return (int)ncol;
		}

		/// <summary>
		/// Returns the pointer to underlying unmanaged matrix. 
		/// </summary>
		/// <returns></returns>
		public IntPtr GetHandle() {
			return _handle;
		}

		/// <summary>
		/// Implementation of Dispose pattern.
		/// </summary>
		/// <param name="disposing">If managed resouses dispose required.</param>
		protected virtual void Dispose(bool disposing) {
			if (_handle != IntPtr.Zero) {
				XGBoostNative.XGDMatrixFree(_handle);
				_handle = IntPtr.Zero;
			}
		}

		~DMatrix() {		
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
