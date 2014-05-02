using System;

public class MultiAttackAbility : Ability
{
	int m_attackCount;
	
	int m_currentAttack;
	int m_minAttackCount;
	int m_maxAttackCount;
	
	public MultiAttackAbility(string name, BattleType type, int damageAmount, int accuracy, uint maxUses, int minAttackCount, int maxAttackCount)
	: base(name, type, damageAmount, accuracy, maxUses)
	{
		m_minAttackCount = minAttackCount;
		m_maxAttackCount = maxAttackCount;
		
		Reset();
	}
	
	public void Reset()
	{
		m_currentAttack = 0;
		Random rand = new Random();
		m_attackCount = rand.Next(m_minAttackCount, m_maxAttackCount + 1);
	}
	
	public override ActionStatus Execute(Character actor, Player targetPlayer)
	{
		if (base.CurrentUses <= 0)
		{
			throw new Exception("No ability charges left");
		}
		
		ActionStatus status = new ActionStatus();
		
		if (actor.isDead())
		{
			// do not process abilities for dead pokemon
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		if (m_currentAttack == 0)
		{
			status.events.Add(new StatusUpdateEventArgs(actor.Name + " used " + base.Name));
		}
		
		Character target = targetPlayer.ActivePokemon;
		
		if ((m_currentAttack < m_attackCount) && (!target.isDead()))
		{
			float multiplier = DamageCalculations.getDamageMultiplier(base.Type, target.Types);
			
			int amount = (int)(base.DamageAmount * multiplier);
			if (amount != 0)
			{
				target.TakeDamage(amount);
				status.events.Add(new DamageEventArgs(targetPlayer, amount));
				status.turnComplete = false;
				status.isComplete = false;
				++m_currentAttack;
			}
			else
			{
				status.events.Add(GetEffectivenessMessage(target, 0));
				status.turnComplete = true;
				status.isComplete = true;
			}
		}
		else
		{
			float multiplier = DamageCalculations.getDamageMultiplier(base.Type, target.Types);
			status.events.Add(GetEffectivenessMessage(target, multiplier));
			status.events.Add(new StatusUpdateEventArgs("Hit " + m_currentAttack + " times!"));
			
			status.turnComplete = true;
			status.isComplete = true;
			
			CurrentUses -= 1;
			if (m_effect.Type != AbilityEffect.EffectType.None)
			{
				System.Random r = new System.Random();
				
				if (r.NextDouble() < m_effect.Rate)
				{
					m_effect.Apply(target);
					status.events.Add(new StatusUpdateEventArgs(target.Name + " " + m_effect.GetActionMessage()));
				}
			}
			
			if (target.isDead())
			{
				status.events.Add(new StatusUpdateEventArgs("The opposing " + target.Name + " fainted!"));
				status.events.Add(new WithdrawEventArgs(target));
			}
			
			// prepare for its next execution
			Reset();
		}
		
		return status;
	}
}

