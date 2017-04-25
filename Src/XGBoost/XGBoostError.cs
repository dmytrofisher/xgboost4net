using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGBoost
{
	/// <summary>
	/// Simple error handle for XGBoost.
	/// </summary>
	class XGBoostError
	{
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
	}
}
