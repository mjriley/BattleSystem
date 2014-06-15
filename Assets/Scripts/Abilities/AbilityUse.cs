using PokeCore;
using System.Collections.Generic;

namespace Abilities {

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
	
	public string Name { get { return m_ability.Name; } }
	
	public virtual ICharacter Subject { get { return m_actor; } }
	
	public virtual ActionStatus Execute()
	{
		return m_ability.Execute(m_actor, m_targetPlayer);
	}
	
	public int Priority { get { return m_ability.Priority; } } 
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}