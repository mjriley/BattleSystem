namespace PokeCore {

// A class meant to always turn down the option for counter-replacing a Pokemon.
// This is only intended to be used by the AI player, existing so that the 
// battle system can remain symmetrical and agnostic of the types of players it is dealing with
public class NoCounterRequest : IActionRequest
{
	public NoCounterRequest()
	{
	}
	
	// Not utilized
	public void SubmitAction(ITurnAction action) { }
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		NoTurnAction action = new NoTurnAction(player.ActivePokemon);
		actionCallback(action);
	}
	
	public RequestType RequestType { get { return RequestType.CounterReplace; } }
}

}

