using System.Collections.Generic;

public interface IAttackStrategy
{
	void UpdateConditions(Character actor, Player enemyPlayer);
	ITurnAction Execute();
}
