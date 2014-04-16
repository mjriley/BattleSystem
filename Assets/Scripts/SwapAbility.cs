using System.Collections.Generic;

public class SwapAbility : ITurnAction
{
	private int m_newIndex;
	private Player m_player;
	private Character m_subject;
	
	private enum State
	{
		Recall,
		Deploy
	};
	
	private State m_currentState = State.Recall;
	
	public SwapAbility(Player player, int newIndex)
	{
		m_newIndex = newIndex;
		m_player = player;
		m_subject = m_player.ActivePokemon;
	}
	
	public Character Subject { get { return m_subject; } }
	
	public string Name { get { return "Swap"; } }
	
	public ActionStatus Execute()
	{
		ActionStatus result = new ActionStatus();
		if (m_currentState == State.Recall)
		{
			result.turnComplete = false;
			result.isComplete = false;
			result.messages.Add(m_player.ActivePokemon.Name + "! Come back!\nSwap out!");
			m_currentState = State.Deploy;
		}
		else if (m_currentState == State.Deploy)
		{
			result.turnComplete = true;
			result.isComplete = true;
			m_player.setActivePokemon(m_newIndex);
			result.messages.Add("Go! " + m_player.ActivePokemon.Name + "!");
		}
		
		return result;
	}
}
