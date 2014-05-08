using System;

public class MultiHitAbility : DamageAbility
{
	uint m_minHits;
	uint m_maxHits;
	
	uint m_currentAttackCount;
	uint m_finalAttackCount;
	
	// Moves like Fury Swipes don't have an even distribution for the chance of the # of strikes
	// to mimic this, pass in a custom random generator that reflects that distribution
	public MultiHitAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, 
		uint power, int accuracy, uint minHits, uint maxHits, bool highCritRate=false, DamageAbility.OnHitEffect onHitHandler=null, string description="", Random generator=null)
	: base(name, abilityType, battleType, maxPP, power, accuracy, highCritRate, onHitHandler, description, generator)
	{
		m_minHits = minHits;
		m_maxHits = maxHits;
		
		Reset();
	}
	
	public void Reset()
	{
		m_currentAttackCount = 0;
		m_finalAttackCount = (uint)m_generator.Next((int)m_minHits, (int)m_maxHits);
	}
	
	protected override ActionStatus ExecuteImpl(Character attacker, Player targetPlayer, ref ActionStatus status)
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
		
		DamageResult result = ApplyDamage(attacker, defender, ref status);	
		
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
