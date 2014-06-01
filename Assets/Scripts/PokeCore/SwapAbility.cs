using System.Collections.Generic;

namespace PokeCore {

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
	
	public ICharacter Subject { get { return m_subject; } }
	
	public string Name { get { return "Swap"; } }
	
	public int Priority { get { return 6; } }
	
	public ActionStatus Execute()
	{
		ActionStatus result = new ActionStatus();
		
		if (m_currentState == State.Recall)
		{
			result.turnComplete = false;
			result.isComplete = false;
			result.events.Add(new StatusUpdateEventArgs(m_player.ActivePokemon.Name + "! Come back!\nSwap out!"));
			result.events.Add(new WithdrawEventArgs(m_player.ActivePokemon));
			m_currentState = State.Deploy;
		}
		else if (m_currentState == State.Deploy)
		{
			result.turnComplete = true;
			result.isComplete = true;
			m_player.setActivePokemon(m_newIndex);
			result.events.Add(new StatusUpdateEventArgs("Go! " + m_player.ActivePokemon.Name + "!"));
			result.events.Add(new DeployEventArgs(m_player.ActivePokemon));
		}
		
		return result;
	}
}

}