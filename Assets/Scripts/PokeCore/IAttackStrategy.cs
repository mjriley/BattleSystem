using PokeCore;
using System.Collections.Generic;

namespace PokeCore {

public interface IAttackStrategy
{
	void UpdateConditions(Character actor, Player enemyPlayer);
	ITurnAction Execute();
}

}