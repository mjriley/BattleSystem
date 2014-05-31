namespace Abilities {

public class NoOpAbility : AbstractAbility
{
	string m_moveMessage;
	
	public NoOpAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, string moveMessage = "", int priority=0, string description="")
	: base(name, abilityType, battleType, maxPP, priority, description)
	{
		m_moveMessage = moveMessage;
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null)
	{
		if (m_moveMessage != "")
		{
			status.events.Add(new StatusUpdateEventArgs(m_moveMessage));
		}
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}