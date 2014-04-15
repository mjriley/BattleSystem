using System.Collections.Generic;

public class SwapAbility : IAbility
{
	private int m_newIndex;
	
	public SwapAbility(int newIndex)
	{
		m_newIndex = newIndex;
	}
	
	public string Name { get { return "Swap"; } }
	
	public AbilityStatus Execute(Character actor, List<Character> enemies)
	{
		actor.Owner.setActivePokemon(m_newIndex);
		
		AbilityStatus result = new AbilityStatus();
		result.isDone = true;
		result.messages = new List<string>();
		result.messages.Add(actor.Name + "! Come back!\nSwap out!");
		result.messages.Add("Go! " + actor.Owner.ActivePokemon.Name + "!");
		
		return result;
	}
}
