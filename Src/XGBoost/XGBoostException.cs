namespace XGBoost
{
	using System;

	#region Class: XGBoostException

	/// <summary>
	/// Custom exception class for XGBoost.
	/// </summary>
	class XGBoostException : Exception
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of <see cref="XGBoostException"/> with given error message.
		/// </summary>
		/// <param name="message">Error message.</param>
		public XGBoostException(string message) : base(message) {
		}

		#endregion

	}

	#endregion

}
