using System.Collections.Generic;

public struct AbilityUse
{
	public List<Character> targets;
	public Ability ability;
	
	public AbilityUse(List<Character> targets, Ability ability)
	{
		this.targets = targets;
		this.ability = ability;
	}
}