//using System.Collections.Generic;
//
//public class FlyAbility : Ability
//{
//	private int m_currentTurn = 0;
//	
//	public FlyAbility(string name, BattleType type, int damageAmount, int accuracy, uint maxUses)
//	: base(name, type, damageAmount, accuracy, maxUses)
//	{
//	}
//	
//	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ActionStatus status)
//	{
//		if (m_currentTurn == 0)
//		{
//			// send the pokemon flying
//			actor.IsInvisible = true;
//			++m_currentTurn;
//			
//			status = new ActionStatus();
//			status.turnComplete = true;
//			status.isComplete = false;
//			status.events.Add(new StatusUpdateEventArgs("Flew up in the air!"));
//		}
//		else
//		{
//			// bring the pokemon back to the earth
//			actor.IsInvisible = false;
//			
//			// do damage
//			status = base.Execute(actor, targetPlayer);
//			
//			// reset the ability state
//			m_currentTurn = 0;
//		}
//		
//		return status;
//	}
//}
//
