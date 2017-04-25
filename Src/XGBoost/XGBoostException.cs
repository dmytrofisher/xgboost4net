using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGBoost
{
	/// <summary>
	/// Custom exception class for XGBoost.
	/// </summary>
	class XGBoostException : Exception
	{
		/// <summary>
		/// Initializes a new instance of <see cref="XGBoostException"/> with given error message.
		/// </summary>
		/// <param name="message">Error message.</param>
		public XGBoostException(string message) : base(message) {
		}
	}
}
