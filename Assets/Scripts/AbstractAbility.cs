using System;

public abstract class AbstractAbility
{
	public string Name { get; protected set; }
	public string Description { get; set; }
	public int CurrentPP { get; protected set; }
	public int MaxPP { get; protected set; }
	public AbilityType AbilityType { get; protected set; }
	public BattleType BattleType { get; protected set; }
	
	public AbstractAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, string description="")
	{
		this.Name = name;
		this.AbilityType = abilityType;
		this.BattleType = battleType;
		this.MaxPP = maxPP;
		this.CurrentPP = maxPP;
		this.Description = description;
	}
	
	public ActionStatus Execute(Character actor, Player targetPlayer)
	{
		if (CurrentPP <= 0)
		{
			throw new Exception("No ability charges left");
		}
		
		ActionStatus status = new ActionStatus();
		
		if (actor.isDead())
		{
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		status.events.Add(new StatusUpdateEventArgs(actor.Name + " used " + Name + "!"));
		
		DeductPP();
		
		ExecuteImpl(actor, targetPlayer, ref status);
		
		return status;
	}
	
	private void DeductPP()
	{
		// currently hard-coded to be 1, but could change depending on abilites
		CurrentPP -= 1;
	}
	
	protected abstract ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status);
}
