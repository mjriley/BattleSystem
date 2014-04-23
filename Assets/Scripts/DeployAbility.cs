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
	
	public Character Subject { get { return m_subject; } }
	
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
}
