using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using XGBoost;

namespace XGBoost.Tests
{
	[TestFixture]
	public class DMatrixTestCase
	{

		[Test]
		public void CreateFromDenseMatrix() {
			int nrow = 10;
			int ncol = 5;
			float[] data = new float[nrow * ncol];
			Random rand = new Random(42);
			for (int i = 0; i < nrow * ncol; i++) {
				data[i] = (float)rand.NextDouble();
			}

			DMatrix matrix = new DMatrix(data, nrow, ncol);

			matrix.RowsCount.Should().Be(nrow);
			matrix.ColumnsCount.Should().Be(ncol);
		}

	}

}
