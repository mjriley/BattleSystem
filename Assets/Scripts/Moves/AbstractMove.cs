using System;
using PokeCore;

namespace Moves {

public abstract class AbstractMove : IMoveImpl
{
	public string Name { get; protected set; }
	public string Description { get; set; }
	public int CurrentPP { get; protected set; }
	public int MaxPP { get; protected set; }
	public MoveType MoveType { get; protected set; }
	public BattleType BattleType { get; protected set; }
	
	public int Priority { get; protected set;  }
	
	public AbstractMove(string name, MoveType moveType, BattleType battleType, int maxPP, int priority=0, string description="")
	{
		this.Name = name;
		this.MoveType = moveType;
		this.BattleType = battleType;
		this.MaxPP = maxPP;
		this.CurrentPP = maxPP;
		this.Priority = priority;
		this.Description = description;
		
	}
	
	public virtual void Reset()
	{
	}
	
	public ActionStatus Execute(Character actor, Player targetPlayer, IMoveImpl parentMove=null)
	{
		ActionStatus status = new ActionStatus();
		
		ExecuteImpl(actor, targetPlayer, ref status, parentMove);
		
		return status;
	}
	
	protected abstract ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IMoveImpl parentMove=null);
}

}
