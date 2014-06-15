using System;

namespace PokeCore {

public class ActionSort
{
	// Unlike a traditional sort, actions can never be 'equal'
	// if two actions would execute at the same time, one is determined to go first randomly
	public static int ByPriority(ITurnAction x, ITurnAction y, IRandom generator=null)
	{
		if (System.Object.ReferenceEquals(x, y))
		{
			return 0;
		}
		
		if (x.Priority != y.Priority)
		{
			return x.Priority.CompareTo(y.Priority);
		}
		
		if (x.Subject.Spd != y.Subject.Spd)
		{
			return x.Subject.Spd.CompareTo(y.Subject.Spd);
		}
		
		if (generator == null)
		{
			generator = new CRandom();
		}
		
		return (generator.Next() % 2 == 0) ? 1 : -1;
	}
}

}