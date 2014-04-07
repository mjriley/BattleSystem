using System.Collections.Generic;

public interface IAttackStrategy
{
	void UpdateConditions(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies);
	AbilityUse Execute();
}
