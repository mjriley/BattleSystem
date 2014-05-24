public class EffectAbility : AbstractAbility
{
	public delegate void BasicEffect(AbstractAbility ability, Character actor, Player targetPlayer, ref ActionStatus status);
	
	BasicEffect EffectImpl;
	
	public EffectAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, BasicEffect effectImpl, int priority=0, string description="")
	: base(name, abilityType, battleType, maxPP, priority, description)
	{
		EffectImpl = effectImpl;
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status)
	{
		EffectImpl(this, actor, targetPlayer, ref status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}
