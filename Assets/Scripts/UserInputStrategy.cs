using System.Collections.Generic;
using System.Linq;

public class UserInputStrategy : IAttackStrategy
{

	public delegate int GetUserInput();
	
	private BattleSystem.PlayerAbilityChoiceHandler m_inputDelegate;
	
	public UserInputStrategy(BattleSystem.PlayerAbilityChoiceHandler inputDelegate)
	{
		m_inputDelegate = inputDelegate;
	}
	
	public AbilityUse Execute(Character actor, IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		System.Console.WriteLine("Hello World");
		int abilityIndex = m_inputDelegate(actor.getAbilities());
		List<Character> targets = new List<Character>();
		targets.Add(enemies.First());
		return new AbilityUse(actor, targets, actor.getAbilities()[abilityIndex]);
	}
}

