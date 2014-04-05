using System.Collections.Generic;

public struct AbilityUse
{
	public Character actor;
	public List<Character> targets;
	public Ability ability;
	
	public AbilityUse(Character actor, List<Character> targets, Ability ability)
	{
		this.actor = actor;
		this.targets = targets;
		this.ability = ability;
	}
}