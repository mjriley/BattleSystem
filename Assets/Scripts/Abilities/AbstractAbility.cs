using System;
using PokeCore;

namespace Abilities {

public abstract class AbstractAbility : IAbilityImpl
{
	public string Name { get; protected set; }
	public string Description { get; set; }
	public int CurrentPP { get; protected set; }
	public int MaxPP { get; protected set; }
	public AbilityType AbilityType { get; protected set; }
	public BattleType BattleType { get; protected set; }
	
	public int Priority { get; protected set;  }
	
	public AbstractAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, int priority=0, string description="")
	{
		this.Name = name;
		this.AbilityType = abilityType;
		this.BattleType = battleType;
		this.MaxPP = maxPP;
		this.CurrentPP = maxPP;
		this.Priority = priority;
		this.Description = description;
		
	}
	
	public virtual void Reset()
	{
	}
	
	public ActionStatus Execute(Character actor, Player targetPlayer, IAbilityImpl parentAbility=null)
	{
		ActionStatus status = new ActionStatus();
		
		ExecuteImpl(actor, targetPlayer, ref status, parentAbility);
		
		return status;
	}
	
	protected abstract ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null);
}

}
