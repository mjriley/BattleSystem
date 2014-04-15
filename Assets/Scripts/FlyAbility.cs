using System.Collections.Generic;

public class FlyAbility : Ability
{
	private int m_currentTurn = 0;
	
	public FlyAbility(string name, BattleType type, int damageAmount, int accuracy, uint maxUses)
	: base(name, type, damageAmount, accuracy, maxUses)
	{
	}
	
	public override AbilityStatus Execute(Character actor, List<Character> enemies)
	{
		AbilityStatus status;
		
		if (m_currentTurn == 0)
		{
			// send the pokemon flying
			actor.IsInvisible = true;
			++m_currentTurn;
			
			status = new AbilityStatus();
			status.isDone = true;
			status.messages = new List<string>();
			status.messages.Add("Flew up in the air!");
		}
		else
		{
			// bring the pokemon back to the earth
			actor.IsInvisible = false;
			
			// do damage
			status = base.Execute(actor, enemies);
			
			// reset the ability state
			m_currentTurn = 0;
		}
		
		return status;
	}
}

