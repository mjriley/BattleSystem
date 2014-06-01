using PokeCore;
using System;

namespace Abilities {

public class EffectAbility : AbstractAbility
{
	public delegate void BasicEffect(AbstractAbility ability, Character actor, Player targetPlayer, ref ActionStatus status);
	
	BasicEffect EffectImpl;
	Random m_generator;
	
	public int Accuracy { get; protected set; }
	
	public EffectAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, BasicEffect effectImpl, int accuracy=100, 
		int priority=0, string description="", Random generator=null)
	: base(name, abilityType, battleType, maxPP, priority, description)
	{
		EffectImpl = effectImpl;
		
		if (generator == null)
		{
			generator = new Random();
		}
		m_generator = generator;
		
		Accuracy = accuracy;
	}
	
	protected bool CheckHit()
	{
		int rand = m_generator.Next(100);
		
		return (rand < Accuracy);
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null)
	{
		if (!CheckHit())
		{
			status.events.Add(new StatusUpdateEventArgs(actor.Name + " missed!"));
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		EffectImpl(this, actor, targetPlayer, ref status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}