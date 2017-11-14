namespace XGBoost
{
	#region Class: XGBoostError

	/// <summary>
	/// Simple error handle for XGBoost.
	/// </summary>
	class XGBoostError
	{

		#region Methods: Public

		/// <summary>
		/// Checks the return value of C API.
		/// </summary>
		/// <param name="exitCode">Return value form <see cref="XGBoostNative"/> native call.</param>
		/// <exception cref="XGBoostException">Native error.</exception>
		public static void CheckError(int exitCode) {
			if (exitCode != 0) {
				throw new XGBoostException(XGBoostNative.XGBGetLastError());
			}
		}

		#endregion

	}

	#endregion

}
