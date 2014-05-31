using System;

namespace Abilities {

// An attack that hits multiple (variable) times within the same turn. The distribution_gen parameter allows the user
// to provide a weighted distribution. In the case of abilities like Fury Swipes, the select of 2 to 5 hits is weighted more towards the lower hit #s
// This is a separate Random parameter so that this uneven distribution will not be used for the parent randomization behavior, like accuracy calculations
public class MultiHitAbility : DamageAbility
{
	uint m_minHits;
	uint m_maxHits;
	
	uint m_currentAttackCount;
	uint m_finalAttackCount;
	
	Random m_distribution_gen;
	
	public MultiHitAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, 
		uint power, int accuracy, uint minHits, uint maxHits, int priority=0,
		bool highCritRate=false, DamageAbility.DamageFormula damageFormula=null, DamageAbility.OnHitEffect onHitHandler=null, string description="", Random generator=null,
		Random distribution_gen=null)
	: base(name, abilityType, battleType, maxPP, power, accuracy, priority, highCritRate, damageFormula, onHitHandler, description, generator)
	{
		m_minHits = minHits;
		m_maxHits = maxHits;
		
		if (distribution_gen == null)
		{
			distribution_gen = new Random();
		}
		
		m_distribution_gen = distribution_gen;
		
		Reset();
	}
	
	public override void Reset()
	{
		base.Reset();
		m_currentAttackCount = 0;
		m_finalAttackCount = (uint)m_distribution_gen.Next((int)m_minHits, (int)m_maxHits);
	}
	
	protected override ActionStatus ExecuteImpl(Character attacker, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null)
	{
		// only check whether we hit the first time
		if (m_currentAttackCount == 0)
		{
			if (!CheckHit())
			{
				status.events.Add(new StatusUpdateEventArgs(attacker.Name + " missed!"));
				status.turnComplete = true;
				status.isComplete = true;
				
				return status;
			}
		}
		
		Character defender = targetPlayer.ActivePokemon;
		
		DamageResult result = ApplyDamage(attacker, defender, ref status, parentAbility);	
		
		++m_currentAttackCount;
		
		if ((m_currentAttackCount >= m_finalAttackCount) || defender.isDead())
		{
			status.turnComplete = true;
			status.isComplete = true;
			status.events.Add(new StatusUpdateEventArgs("Hit " + m_finalAttackCount + " times!"));
			Reset(); // TODO: Should probably be called explitly by the battle system.
		}
		else
		{
			status.turnComplete = false;
			status.isComplete = false;
		}
		
		HandleTargetDeath(defender, ref status);
		HandleOnHit(attacker, defender, result, ref status);
		
		return status;
	}
}

}