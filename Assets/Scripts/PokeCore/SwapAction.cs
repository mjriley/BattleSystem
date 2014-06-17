using System.Collections.Generic;

namespace PokeCore {

// Differs from Deploy action with the intent that this needs to swap an existing (alive) pokemon for another one
public class SwapAction : ITurnAction
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
	
	public SwapAction(Player player, int newIndex)
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
			string format = L18N.Get("MSG_SWAP"); // <X>! Come back! Swap out!
			string message = string.Format(format, m_player.ActivePokemon.Name);
			result.events.Add(new StatusUpdateEventArgs(message));
			result.events.Add(new WithdrawEventArgs(m_player.ActivePokemon));
			m_currentState = State.Deploy;
		}
		else if (m_currentState == State.Deploy)
		{
			result.turnComplete = true;
			result.isComplete = true;
			m_player.setActivePokemon(m_newIndex);
			string format = L18N.Get("MSG_GO"); // Go! <X>!
			string message = string.Format(format, m_player.ActivePokemon.Name);
			result.events.Add(new StatusUpdateEventArgs(message));
			result.events.Add(new DeployEventArgs(m_player.ActivePokemon));
		}
		
		return result;
	}
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}