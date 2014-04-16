using System;
using System.Linq;
using System.Collections.Generic;

public class RandomAttackStrategy : IAttackStrategy
{
	private System.Random m_generator;
	
	private AbilityUse m_turnInfo;
	
	/*******
	 * Basic Constructor
	 * the generator parameter is provided for testing purposes
	 ************/
	public RandomAttackStrategy(Random generator = null)
	{
		if (generator == null)
		{
			m_generator = new Random();
		}
		else
		{
			m_generator = generator;
		}
	}
	
	/******
	 * Attempts to find an ability (at random)
	 * Returns the desired ability or null if no ability with sufficient uses could be found
	 **********/
	private Ability DetermineAbility(Character actor)
	{
		uint cost = actor.getUsageCost();
		
		List<Ability> abilities = actor.getAbilities();
		
		Ability ability = null;
		
		while (abilities.Count() > 0)
		{
			int abilityIndex = m_generator.Next(0, abilities.Count());
			ability = abilities.ElementAt(abilityIndex);
			
			if (ability.CurrentUses < cost)
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
	
	public void UpdateConditions(Character actor, Player enemyPlayer)
	{
		Ability ability = DetermineAbility(actor);
		
		m_turnInfo = new AbilityUse(actor, enemyPlayer, ability);
	}
	
	/*********
	 * Determine an ability to use on a target, at random
	 * Returns an AbilityUse struct
	 ***************/
	public ITurnAction Execute()
	{
		return m_turnInfo;
	}
}
