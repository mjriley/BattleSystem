using System.Collections.Generic;
using System.Linq;

public class UserInputStrategy : IAttackStrategy
{
	public delegate void ReceiveConditions(List<AbstractAbility> abilities);
	public delegate ITurnAction GetUserInput();
	
	//private ReceiveConditions m_conditionsDelegate;
	private GetUserInput m_inputDelegate;
	
	public UserInputStrategy(ReceiveConditions conditionsDelegate, GetUserInput inputDelegate)
	{
		//m_conditionsDelegate = conditionsDelegate;
		m_inputDelegate = inputDelegate;
	}
	
	public void UpdateConditions(Character actor, Player enemyPlayer)
	{
		//m_conditionsDelegate(actor.getAbilities());
	}
	
	public ITurnAction Execute()
	{
		return m_inputDelegate();
	}
}

