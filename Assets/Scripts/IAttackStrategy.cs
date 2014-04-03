using System.Collections.Generic;

public interface IAttackStrategy
{
	AbilityUse Execute(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies);
}
