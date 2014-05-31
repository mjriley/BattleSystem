using System;

namespace Abilities {

public class Ability
{
	AbstractAbility m_abilityImpl;
	
	public int CurrentPP { get; protected set; }
	public int MaxPP { get; protected set; }
	
	public string Name { get { return m_abilityImpl.Name; } }
	public AbilityType AbilityType { get { return m_abilityImpl.AbilityType; } }
	public BattleType BattleType { get { return m_abilityImpl.BattleType; } }
	public int Priority { get { return m_abilityImpl.Priority; } }
	
	bool m_turnStarted;
	bool m_hasStarted;
	
	public Ability(AbstractAbility abilityImpl, int maxPP)
	{
		m_abilityImpl = abilityImpl;
		
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
			
			status.events.Add(new StatusUpdateEventArgs(actor.Name + " flinched and couldn't move!"));
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
				throw new Exception("No ability charges left");
			}
			DeductPP();
		}
		
		if (!m_turnStarted)
		{
			status.events.Add(new StatusUpdateEventArgs(actor.Name + " used " + Name + "!"));
		}
		
		ActionStatus exectionStatus = m_abilityImpl.Execute(actor, targetPlayer);
		
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

}

