using System;
using System.Collections.Generic;

public class Ability
{
	private string m_name;
	public string Name
	{
		get { return m_name; }
	}
	
	private BattleType m_type;
	public BattleType Type
	{
		get { return m_type; }
	}
	
	private int m_damageAmount;
	public int DamageAmount
	{
		get { return m_damageAmount; }
	}
	
	private uint m_maxUses;
	public uint MaxUses
	{
		get { return m_maxUses; }
	}
	
	private uint m_currentUses;
	public uint CurrentUses
	{
		get { return m_currentUses; }
		set { m_currentUses = value; }
	}
	
	private int m_accuracy;
	public int Accuracy
	{
		get { return m_accuracy; }
	}
	
	private AbilityEffect m_effect;
	
	public Ability(string name, BattleType type, int damageAmount, int accuracy, uint maxUses)
	: this(name, type, damageAmount, accuracy, maxUses, new AbilityEffect(AbilityEffect.EffectType.None, 0))
	{
	}
	
	public Ability(string name, BattleType type, int damageAmount, int accuracy, uint maxUses, AbilityEffect effect)
	{
		m_name = name;
		m_type = type;
		m_damageAmount = damageAmount;
		m_maxUses = maxUses;
		m_currentUses = m_maxUses;
		m_accuracy = accuracy;
		m_effect = effect;
	}
	
	public virtual ActionStatus Execute(Character actor, Player targetPlayer)
	{
		if (m_currentUses <= 0)
		{
			throw new Exception("No ability charges left");
		}
		
		ActionStatus status = new ActionStatus();
		
		Character target = targetPlayer.ActivePokemon;
		
		if (actor.isDead())
		{
			// do not process abilities for dead pokemon
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		float multiplier = DamageCalculations.getDamageMultiplier(m_type, target.Types);
		
		int amount = (int)(m_damageAmount * multiplier);
		if (amount != 0)
		{
			target.TakeDamage(amount);
		}
		
		m_currentUses -= 1;
		
		status.turnComplete = true;
		status.isComplete = true;
		
		status.events.Add(new StatusUpdateEventArgs(actor.Name + " used " + Name + "!"));
		status.events.Add(new DamageEventArgs(targetPlayer, amount));
		
		if (multiplier > 1)
		{
			status.events.Add(new StatusUpdateEventArgs("It's super effective!"));
		}
		else if (multiplier == 0)
		{
			status.events.Add(new StatusUpdateEventArgs("It doesn't affect the opposing " + target.Name + "..."));
		}
		else if (multiplier < 1)
		{
			status.events.Add(new StatusUpdateEventArgs("It's not very effective..."));
		}
		
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
		
		
		return status;
	}
}
