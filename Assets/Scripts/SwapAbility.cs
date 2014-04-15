using System.Collections.Generic;

public class SwapAbility : IAbility
{
	private int m_newIndex;
	
	private enum State
	{
		Recall,
		Deploy
	};
	
	private State m_currentState = State.Recall;
	
	public SwapAbility(int newIndex)
	{
		m_newIndex = newIndex;
	}
	
	public string Name { get { return "Swap"; } }
	
	public AbilityStatus Execute(Character actor, List<Character> enemies)
	{
		AbilityStatus result = new AbilityStatus();
		result.messages = new List<string>();
		if (m_currentState == State.Recall)
		{
			result.isDone = false;
			result.messages.Add(actor.Name + "! Come back!\nSwap out!");
			m_currentState = State.Deploy;
		}
		else if (m_currentState == State.Deploy)
		{
			result.isDone = true;
			actor.Owner.setActivePokemon(m_newIndex);
			result.messages.Add("Go! " + actor.Owner.ActivePokemon.Name + "!");
		}
		
		return result;
	}
}
