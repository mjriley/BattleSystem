using System.Collections.Generic;

namespace PokeCore {

public class SequentialPokemonRequest : IActionRequest
{
	public SequentialPokemonRequest()
	{
	}
	
	// Not utilized
	public void SubmitAction(ITurnAction action) { }
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		List<Character> myPokemon = player.Pokemon;
		int index = myPokemon.IndexOf(player.ActivePokemon);
		
		bool foundPokemon = false;
		int nextIndex = (index + 1) % myPokemon.Count;
		
		// The current pokemon cannot be the next pokemon
		// Hence why we search through count - 1 pokemon
		// Generally this request is only made if the current pokemon is dead
		// But it is relevant if the active pokemon is forced out instead of killed
		for (int i = 0; i < myPokemon.Count - 1; ++i)
		{
			if (!myPokemon[nextIndex].isDead() && (nextIndex != index))
			{
				foundPokemon = true;
				break;
			}
			
			nextIndex = (nextIndex + 1) % myPokemon.Count;
		}
		
		ITurnAction action;
		
		if (foundPokemon)
		{
			action = new DeployAbility(player, nextIndex, false);
		}
		else
		{
			// If the current pokemon isn't dead and we can find no suitable replacement
			// For now...just returning no action. Depending on the scenario, it might be appropriate to
			// create a Forfeit action as well
			action = new NoTurnAction(player.ActivePokemon);
		}
		
		actionCallback(action);
	}
	
	public RequestType RequestType { get { return RequestType.Replace; } }
}

}

