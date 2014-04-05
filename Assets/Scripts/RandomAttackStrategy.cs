using System;
using System.Linq;
using System.Collections.Generic;

public class RandomAttackStrategy : IAttackStrategy
{
	private System.Random m_generator;
	
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
	
	/******
	 * Attempts to find the target to use the provided ability on
	 * Returns a list of the target or null if no target could be found
	 **********/
	private List<Character> DetermineTargets(Ability ability, Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		List<Character> targets = null;
		
		if (enemies.Count() > 0)
		{
			int targetIndex = m_generator.Next(0, enemies.Count());
			
			targets = new List<Character>();
			targets.Add(enemies.ElementAt(targetIndex));
		}
		
		return targets;
	}
	
	/*********
	 * Determine an ability to use on a target, at random
	 * Returns an AbilityUse struct
	 ***************/
	public AbilityUse Execute(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		Ability ability = DetermineAbility(actor);
	
		List<Character> targets = DetermineTargets(ability, actor, allies, enemies);
		
		return new AbilityUse(actor, targets, ability);
	}
}
