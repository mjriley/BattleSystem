using System;

namespace PokeCore {
namespace Moves {

public class Move
{
	AbstractMove m_moveImpl;
	
	public int CurrentPP { get; protected set; }
	public int MaxPP { get; protected set; }
	
	public string Name { get { return m_moveImpl.Name; } }
	public MoveType MoveType { get { return m_moveImpl.MoveType; } }
	public BattleType BattleType { get { return m_moveImpl.BattleType; } }
	public int Priority { get { return m_moveImpl.Priority; } }
	
	bool m_turnStarted;
	bool m_hasStarted;
	
	public Move(AbstractMove moveImpl, int maxPP)
	{
		m_moveImpl = moveImpl;
		
		MaxPP = maxPP;
		CurrentPP = MaxPP;
	}
	
	public void Reset()
	{
		m_turnStarted = false;
		m_hasStarted = false;
	}
	
	public void Replenish()
	{
		CurrentPP = MaxPP;
	}
	
	// replenishes a specific amount of PP, returning the actual amount replenished
	public int Replenish(int amount)
	{
		int previousPP = CurrentPP;
		
		int newPP = CurrentPP + amount;
		CurrentPP = Math.Min(newPP, MaxPP);
		
		return CurrentPP - previousPP;
	}
	
	void PerformEligibilityChecks(Character actor, ref ActionStatus status)
	{
		if (actor.isDead())
		{
			status.turnComplete = true;
			status.isComplete = true;
		}
		else if (actor.Flinching)
		{
			status.turnComplete = true;
			status.isComplete = true;
			
			string format = L18N.Get("STATUS_FLINCHED");
			string message = string.Format(format, actor.Name);
			
			status.events.Add(new StatusUpdateEventArgs(message));
		}
	}
	
	public ActionStatus Execute(Character actor, Player targetPlayer)
	{
		ActionStatus status = new ActionStatus();
		
		if (!m_turnStarted)
		{
			PerformEligibilityChecks(actor, ref status);
			if (status.isComplete)
			{
				return status;
			}
		}
		
		if (!m_hasStarted)
		{
			if (CurrentPP <= 0)
			{
				throw new Exception("No move charges left");
			}
			DeductPP();
		}
		
		if (!m_turnStarted)
		{
			string format = L18N.Get("MOVE_USE"); // <X> used <Y>!
			string message = string.Format(format, actor.Name, Name);
			status.events.Add(new StatusUpdateEventArgs(message));
		}
		
		ActionStatus exectionStatus = m_moveImpl.Execute(actor, targetPlayer);
		
		m_turnStarted = !exectionStatus.turnComplete;
		m_hasStarted = !exectionStatus.isComplete;
		
		status.turnComplete = exectionStatus.turnComplete;
		status.isComplete = exectionStatus.isComplete;
		status.events.AddRange(exectionStatus.events);
		
		return status;
	}
	
	void DeductPP()
	{
		// currently hard-coded to be 1, but could change depending on abilites
		CurrentPP -= 1;
	}
}

}}