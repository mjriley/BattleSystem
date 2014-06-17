using System;

namespace Moves
{
	// this is meant to give unequal weighting to the lower hit #s
	// As there are 4 possible options (2, 3, 4, 5), and each of the bottom 2 are twice as likely
	// we give the bottom of the distribution 2/3rds of the probability space and 
	// the top the remaining 1/3rd
	public class FiveHitDistribution : Random
	{
		protected override double Sample()
		{
			double unweighted = base.Sample();
			double remaining = 0.0;
			if (unweighted > 0.5)
			{
				remaining = unweighted - 0.5;
				unweighted = 0.5;
			}
			
			return unweighted * 4 / 3 + remaining * 2 / 3;
		}
	}
}

