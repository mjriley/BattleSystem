using PokeCore;

namespace PokeCore {

public interface IActionRequest
{
	
	void SubmitAction(ITurnAction action);
	void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback);
	RequestType RequestType { get; }
}

}