using System;

public class RandomAttackStrategy : IAttackStrategy
{
	private System.Random m_generator;
	
	public RandomAttackStrategy(Random generator = null)
	{
		if (generator == null)
		{
			m_generator = new Random();
		}
		else
		{
			m_generator = generator;
		}
	}
	
	public void Execute(Character actor, Character[] targets)
	{
		Ability[] abilities = actor.getAbilities();
		
		int index = m_generator.Next(0, 4);
		
		abilities[index].Execute(targets);
	}
}
