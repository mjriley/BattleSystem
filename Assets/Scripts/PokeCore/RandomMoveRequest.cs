using System;
using System.Collections.Generic;
using Abilities;

namespace PokeCore {

public class RandomMoveRequest : IActionRequest
{
	Random m_generator;
	
	public RandomMoveRequest(Random generator = null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		m_generator = generator;
	}
	
	Ability DetermineAbility(Character actor)
	{
		uint cost = actor.getUsageCost();
		
		List<Ability> abilities = actor.getAbilities();
		
		Ability ability = null;
		
		while (abilities.Count > 0)
		{
			int abilityIndex = m_generator.Next(0, abilities.Count);
			ability = abilities[abilityIndex];
			
			if (ability.CurrentPP < cost)
			{
				abilities.Remove(ability);
			}
			else
			{
				return ability;
			}
		}
		
		return null;
	}
	
	// Not utilized -- should be refactored
	public void SubmitAction(ITurnAction action)
	{
	
	}
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		Ability ability = DetermineAbility(player.ActivePokemon);
		AbilityUse action = new AbilityUse(player.ActivePokemon, enemyPlayer, ability);
		actionCallback(action);
	}
	
	public RequestType RequestType { get { return RequestType.Turn; } }
}

}

