using System.Collections.Generic;
using System.Linq;

public class UserInputStrategy : IAttackStrategy
{

	public delegate int GetUserInput();
	
	private GetUserInput m_inputDelegate;
	
	public UserInputStrategy(GetUserInput inputDelegate)
	{
		m_inputDelegate = inputDelegate;
	}
	
	public AbilityUse Execute(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		int abilityIndex = m_inputDelegate();
		if (abilityIndex == -1)
		{
			return null;
		}
		
		List<Character> targets = new List<Character>();
		targets.Add(enemies.First());
		return new AbilityUse(actor, targets, actor.getAbilities()[abilityIndex]);
	}
}

