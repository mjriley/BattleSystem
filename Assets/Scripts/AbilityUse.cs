
public class AbilityUse : ITurnAction
{
	public Character m_actor;
	public Player m_targetPlayer;
	public Ability m_ability;
	
	public AbilityUse(Character actor, Player targetPlayer, Ability ability)
	{
		m_actor = actor;
		m_targetPlayer = targetPlayer;
		m_ability = ability;
	}
	
	public virtual ActionStatus Execute()
	{
		return m_ability.Execute(m_actor, m_targetPlayer);
	}
}