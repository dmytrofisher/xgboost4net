namespace XGBoost
{
	using System;
	using System.Runtime.InteropServices;

	#region Class: DMatrix

	/// <summary>
	/// Represent DMatrix for XGBoost.
	/// </summary>
	public class DMatrix : IDisposable
	{

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

		#region Fields: Private

		private IntPtr _handle;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Creates an instance of <see cref="DMatrix"/> from specified file in LibSVM format.
		/// </summary>
		/// <param name="filePath">The path to the data.</param>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public DMatrix(string filePath) {
			if (filePath == null) {
				throw new ArgumentNullException("filePath");
			}
			int exitCode = XGBoostNative.XGDMatrixCreateFromFile(filePath, 1, out _handle);
			XGBoostError.CheckError(exitCode);
		}

		/// <summary>
		/// Creates an instance of <see cref="DMatrix"/> from a given data row by row.
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

		#endregion

		#region Properties: Public

		/// <summary>
		/// Get the number of rows.
		/// </summary>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public int RowsCount {
			get {
				ulong nrow;
				int exitCode = XGBoostNative.XGDMatrixNumRow(_handle, out nrow);
				XGBoostError.CheckError(exitCode);
				return (int)nrow;
			}
		}

		/// <summary>
		/// Get the number of columns.
		/// </summary>
		/// <exception cref="XGBoostException">Native call error.</exception>
		public int ColumnsCount {
			get {
				ulong ncol;
				int exitCode = XGBoostNative.XGDMatrixNumCol(_handle, out ncol);
				XGBoostError.CheckError(exitCode);
				return (int)ncol;
			}
		}

		/// <summary>
		/// Gets the pointer to underlying unmanaged matrix.
		/// </summary>
		public IntPtr Handle {
			get {
				return _handle;
			}
		}

		/// <summary>
		/// Gets or sets the matrix labels.
		/// Amount of weights is expeted to be equal to the number of rows in the matrix,
		/// <see cref="XGBoostException"/> would be thrown otherwise.
		/// </summary>		
		public float[] Labels {
			get {
				return GetFloatInfo("label");
			}
			set {
				if (value.Length != RowsCount) {
					throw new XGBoostException("The length of labels must equal to the number of rows");
				}
				SetFloatInfo("label", value);
			}
		}

		/// <summary>
		/// Gets or sets weights for each matrix row.
		/// Amount of weights is expeted to be equal to the number of rows in the matrix,
		/// <see cref="XGBoostException"/> would be thrown otherwise.
		/// </summary>
		public float[] Weights {
			get {
				return GetFloatInfo("weight");
			}
			set {
				if (value.Length != RowsCount) {
					throw new XGBoostException("The length of weights must equal to the number of rows");
				}
				SetFloatInfo("weight", value);
			}
		}

		#endregion

		#region Methods: Private

		private void SetFloatInfo(string field, float[] values) {
			int exitCode = XGBoostNative.XGDMatrixSetFloatInfo(_handle, field, values, (ulong)values.Length);
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

		#endregion

		/// <summary>
		/// Slice the <see cref="DMatrix"/> and return a new <see cref="DMatrix"/>, 
		/// that only contains specified rows.
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <returns></returns>
		public DMatrix Slice(int[] rowIndex) {
			IntPtr slicedPtr;
			int exitCode = XGBoostNative.XGDMatrixSliceDMatrix(_handle, rowIndex, (ulong)rowIndex.Length, out slicedPtr);
			XGBoostError.CheckError(exitCode);
			return new DMatrix(slicedPtr);
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

	#endregion

}
