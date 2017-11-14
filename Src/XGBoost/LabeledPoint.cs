namespace XGBoost
{
	/// <summary>
	/// Labeled data point for training examples.
	/// Represents a sparse training instance.
	/// </summary>
	public class LabeledPoint
	{
		private LabeledPoint() {
		}

		public LabeledPoint(float label, float[] values) {
			Label = label;
			Values = values;
		}

		public float Label { get; private set; }
		public float[] Values { get; private set; }
	}
}
