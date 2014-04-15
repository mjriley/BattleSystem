using System.Collections.Generic;
using System.Linq;

public class UserInputStrategy : IAttackStrategy
{
	public delegate void ReceiveConditions(List<Ability> abilities);
	public delegate IAbility GetUserInput();
	
	private ReceiveConditions m_conditionsDelegate;
	private GetUserInput m_inputDelegate;
	
	private Character m_actor;
	private IEnumerable<Character> m_enemies;
	
	
	public UserInputStrategy(ReceiveConditions conditionsDelegate, GetUserInput inputDelegate)
	{
		m_conditionsDelegate = conditionsDelegate;
		m_inputDelegate = inputDelegate;
	}
	
	public void UpdateConditions(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		m_actor = actor;
		m_enemies = enemies;
		
		m_conditionsDelegate(actor.getAbilities());
	}
	
	public AbilityUse Execute()
	{
		IAbility ability = m_inputDelegate();
		if (ability == null)
		{
			return null;
		}
		
		List<Character> targets = new List<Character>();
		targets.Add(m_enemies.First());
		return new AbilityUse(m_actor, targets, ability);
		//return new AbilityUse(m_actor, targets, m_actor.getAbilities()[abilityIndex]);
	}
}

