using System.Collections.Generic;

namespace PokeCore {

// Represents only the deploy action of putting a pokemon onto the field to start a battle,
// or replacing a dead pokemon that has already been removed from the field.
// This could probably be combined with the SwapAbility
public class DeployAbility : ITurnAction
{
	Player m_player;
	Character m_subject;
	int m_newIndex;
	bool m_friendly;
	
	public DeployAbility(Player player, int newIndex, bool friendly)
	{
		m_player = player;
		m_newIndex = newIndex;
		m_friendly = friendly;
		
		m_subject = player.Pokemon[newIndex];
	}
	
	public ICharacter Subject { get { return m_subject; } }
	
	public int Priority { get { return 6; } }
	
	public ActionStatus Execute()
	{
		ActionStatus result = new ActionStatus();
		
		result.turnComplete = true;
		result.isComplete = true;
		
		m_player.setActivePokemon(m_newIndex);
		if (m_friendly)
		{
			result.events.Add(new StatusUpdateEventArgs("Go! " + m_player.ActivePokemon.Name + "!"));
		}
		else
		{
			result.events.Add(new StatusUpdateEventArgs(m_player.Name + " sent out " + m_player.ActivePokemon.Name + "!"));
		}
		result.events.Add(new DeployEventArgs(m_player.ActivePokemon));
		
		return result;
	}
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}