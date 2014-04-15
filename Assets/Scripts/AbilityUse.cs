using System.Collections.Generic;

public class AbilityUse
{
	public Character actor;
	public List<Character> targets;
	public IAbility ability;
	
	public AbilityUse(Character actor, List<Character> targets, IAbility ability)
	{
		this.actor = actor;
		this.targets = targets;
		this.ability = ability;
	}
}