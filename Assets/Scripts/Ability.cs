using System;
using System.Collections.Generic;

public class Ability : IAbility
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
	
	public Ability(string name, BattleType type, int damageAmount, int accuracy, uint maxUses)
	{
		m_name = name;
		m_type = type;
		m_damageAmount = damageAmount;
		m_maxUses = maxUses;
		m_currentUses = m_maxUses;
		m_accuracy = accuracy;
	}
	
	// TODO: Figure out a proper return value to show what happened
	public virtual ActionStatus Execute(Character actor, Player targetPlayer)
	{
		if (m_currentUses <= 0)
		{
			throw new Exception("No ability charges left");
		}
		
		Character target = targetPlayer.ActivePokemon;
		float multiplier = DamageCalculations.getDamageMultiplier(m_type, target.Types);
		
		int amount = (int)(m_damageAmount * multiplier);
		if (amount != 0)
		{
			target.TakeDamage(amount);
		}
		
		m_currentUses -= 1;
		
		ActionStatus status = new ActionStatus();
		status.isComplete = true;
		
		status.messages.Add(actor.Name + " used " + Name + "!");
		if (multiplier > 1)
		{
			status.messages.Add("It's super effective!");
		}
		else if (multiplier == 0)
		{
			status.messages.Add("It doesn't affect the opposing " + target.Name + "...");
		}
		else if (multiplier < 1)
		{
			status.messages.Add("It's not very effective...");
		}
		
		return status;
	}
	
//	public void Use()
//	{
//		if (m_currentUses <= 0)
//		{
//			throw new Exception("No ability charges left");
//		}
//		
//		m_currentUses -= 1;
//	}
}
