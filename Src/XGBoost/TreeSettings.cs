namespace XGBoost
{
	using System.Collections.Generic;

	#region Class: TreeSettings

	/// <summary>
	/// Parameters of tree booser.
	/// Check official documentation for details.
	/// <para>http://xgboost.readthedocs.io/en/latest/parameter.html</para>
	/// </summary>
	public class TreeSettings
	{

		#region Fileds: Private

		private readonly Dictionary<string, int> _intParams;
		private readonly Dictionary<string, float> _floatParams;

		#endregion

		#region Constructors: Public

		//TODO: Documentation
		public TreeSettings() {
			_intParams = new Dictionary<string,int> {
				{ "max_depth", 6 },
				{ "min_child_weight", 1 },
				{ "max_delta_step", 0 }
			};
			_floatParams = new Dictionary<string, float> {
				{ "learning_rate", 0.6f },
				{ "min_split_loss", 0.0f },
				{ "subsample", 1.0f }
			};
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Step size shrinkage used in update to prevent overfitting. After each boosting step, we can
		/// directly get the weights of new feature, and <see cref="LearningRate"/> actually shrinks
		/// the feature weights to make the boosting process more conservative.
		/// <para>This parameter also denoted as "eta".</para>
		/// <para>Default: 0.6</para>
		/// <para>Range:   [0,1]</para>
		/// <para>Alias:   "learning_rate"</para>
		/// </summary>
		public float LearningRate {
			get {
				return _floatParams["learning_rate"];
			}
			set {
				if (0 <= value && value <= 1) {
					_floatParams["learning_rate"] = value;
				} else {
					_floatParams["learning_rate"] = 0.6f;
				}
			}
		}

		/// <summary>
		/// Minimum loss reduction required to make a further partition on a leaf node of the tree.
		/// The larger, the more conservative the algorithm will be.
		/// /// <para>this parameter also denoted as "gamma".</para>
		/// <para>Default: 0.0</para>
		/// <para>Range:   [0,Inf)</para>
		/// <para>Alias:   "min_split_loss"</para>
		/// </summary>
		public float MinSplitLoss {
			get {
				return _floatParams["min_split_loss"];
			}
			set {
				_floatParams["min_split_loss"] = value > 0f ? value : 0f;
			}
		}

		/// <summary>
		/// Maximum depth of a tree, increase this value will make the model
		/// more complex / likely to be overfitting.
		/// <para>Default: 6</para>
		/// <para>Range:   [1, Inf)</para>
		/// <para>Alias:   "max_depth"</para>
		/// </summary>
		public int MaxDepth {
			get {
				return _intParams["max_depth"];
			}
			set {
				_intParams["max_depth"] = value >= 1 ? value : 6;
			}
		}

		/// <summary>
		/// Minimum sum of instance weight (hessian) needed in a child. If the tree
		/// partition step results in a leaf node with the sum of instance weight less
		/// than <see cref="MinChildWeight"/>, then the building process will give up further
		/// partitioning. The larger, the more conservative the algorithm will be.
		/// <para>Default: 1</para>
		/// <para>Range:   [0, Inf)</para>
		/// <para>Alias:   "min_child_weight"</para>
		/// </summary>
		public int MinChildWeight {
			get {
				return _intParams["min_child_weight"];
			}
			set {
				_intParams["min_child_weight"] = value >= 0 ? value : 1;
			}
		}

		/// <summary>
		/// Maximum delta step we allow each tree’s weight estimation to be.
		/// If the value is set to 0, it means there is no constraint. If it is set to a positive value,
		/// it can help making the update step more conservative. Usually this parameter is not needed.
		/// <para>Default: 0</para>
		/// <para>Range:   [0, Inf)</para>
		/// <para>Alias:   "max_delta_step"</para>
		/// </summary>
		public int MaxDeltaStep {
			get {
				return _intParams["max_delta_step"];
			}
			set {
				_intParams["max_delta_step"] = value >= 0 ? value : 0;
			}
		}

		/// <summary>
		/// subsample ratio of the training instance. Setting it to 0.5 means that XGBoost randomly
		/// collected half of the data instances to grow trees and this will prevent overfitting.
		/// <para>Default: 1</para>
		/// <para>Range:   (0, 1]</para>
		/// <para>Alias:   "subsample"</para>
		/// </summary>
		public float Subsample {
			get {
				return _floatParams["subsample"];
			}
			set {
				if (0f < value && value <= 1f) {
					_floatParams["subsample"] = value;
				} else {
					_floatParams["subsample"] = 1f;
				}
			}
		}

		// TODO: Implemetn and update documentation ("n_estimators")
		public int Estimators { get; set; }

		// TODO: Implemetn and update documentation ("silent")
		public bool Silent { get; set; }

		// TODO: Implemetn and update documentation ("objective")
		public string Objective { get; set; }

		// TODO: Implemetn and update documentation ("nthread")
		public int Threads { get; set; }

		// TODO: Implemetn and update documentation ("colsample_bytree")
		public int ColSampleByTree { get; set; }

		// TODO: Implemetn and update documentation ("colsample_bylevel")
		public int ColSampleByLevel { get; set; }

		// TODO: Implemetn and update documentation ("reg_alpha")
		public float RegAlpha { get; set; }

		// TODO: Implemetn and update documentation ("reg_lambda")
		public float RegLambda { get; set; }

		// TODO: Implemetn and update documentation ("scale_pos_weight")
		public float ScalePosWeight { get; set; }

		// TODO: Implemetn and update documentation ("base_score")
		public float BaseScore { get; set; }

		// TODO: Implemetn and update documentation "seed")
		public int Seed { get; set; }

		// TODO: Implemetn and update documentation ("missing")
		public float Missing { get; set;
		}

		// TODO: Implemetn and update documentation ("booster")
		public object Booster { get; set; }

		#endregion

	}

	#endregion

}
