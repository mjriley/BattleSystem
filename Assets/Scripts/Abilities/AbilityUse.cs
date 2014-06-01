using PokeCore;

namespace Abilities {

public class AbilityUse : ITurnAction
{
	public Character m_actor;
	public Player m_targetPlayer;
	//public AbstractAbility m_ability;
	public Ability m_ability;
	
	//public AbilityUse(Character actor, Player targetPlayer, AbstractAbility ability)
	public AbilityUse(Character actor, Player targetPlayer, Ability ability)
	{
		m_actor = actor;
		m_targetPlayer = targetPlayer;
		m_ability = ability;
	}
	
	public virtual ICharacter Subject { get { return m_actor; } }
	
	public virtual ActionStatus Execute()
	{
		return m_ability.Execute(m_actor, m_targetPlayer);
	}
	
	public int Priority { get { return m_ability.Priority; } } 
}

}