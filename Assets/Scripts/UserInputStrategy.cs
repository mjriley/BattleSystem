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
		System.Console.WriteLine("Hello World");
		int abilityIndex = m_inputDelegate();
		List<Character> targets = new List<Character>();
		targets.Add(enemies.First());
		return new AbilityUse(targets, actor.getAbilities()[abilityIndex]);
	}
}

