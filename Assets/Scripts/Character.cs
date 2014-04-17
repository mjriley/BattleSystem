﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character
{
	public enum Sex
	{
		Male,
		Female
	}
	
	private int m_maxHP;
	private int m_currentHP;
	private List<Ability> m_abilities = new List<Ability>();
	private string m_name;
	
	private WeakReference m_refOwner;
	public Player Owner
	{
		get { return (Player)m_refOwner.Target; }
		set { m_refOwner = new WeakReference(value); }
	}
	
	private List<BattleType> m_types = new List<BattleType>();
	
	private bool m_isInvisible = false;
	public bool IsInvisible
	{
		get { return m_isInvisible; }
		set { m_isInvisible = value; }
	}
	
	private IAttackStrategy m_strategy;
	
	public string Name
	{
		get { return m_name; }
	}
	
	public int CurrentHP
	{
		get { return m_currentHP; }
	}
	
	public int MaxHP { get { return m_maxHP; } }
	
	public List<BattleType> Types { get { return m_types; } }
	
	private Sex m_gender;
	public Sex Gender
	{
		get { return m_gender; }
	}
	
	public Character(string name, Sex gender, int maxHP, BattleType type, IAttackStrategy strategy) :
		this(name, gender, maxHP, type, BattleType._None, strategy)
	{
	}
	
	public Character(string name, Sex gender, int maxHP, BattleType type1, BattleType type2, IAttackStrategy strategy = null)
	{
		m_name = name;
		m_gender = gender;
		m_maxHP = maxHP;
		m_currentHP = m_maxHP;
		
		m_strategy = strategy;
		
		m_types.Add(type1);
		if (type2 != BattleType._None)
		{
			m_types.Add(type2);
		}
		
		ClearStatuses();
	}
	
	public void Reset()
	{
		m_currentHP = m_maxHP;
	}
	
	public void TakeDamage(int damage)
	{
		m_currentHP -= damage;
		
		m_currentHP = Mathf.Min(m_currentHP, m_maxHP);
		m_currentHP = Mathf.Max(m_currentHP, 0);
	}
	
	public bool isDead()
	{
		return (m_currentHP == 0);
	}
	
	public List<Ability> getAbilities()
	{
		return new List<Ability>(m_abilities);
	}
	
	public void addAbility(Ability ability)
	{
		m_abilities.Add(ability);
	}
	
	public void replaceAbility(int index, Ability ability)
	{
		m_abilities[index] = ability;
	}
	
	// Stubbed to only return 1 right now,
	// but accounts for ability costs changes that occur due to something like the 'Pressure' ability
	public uint getUsageCost()
	{
		return 1;
	}
	
	public void UpdateBattleConditions(Player enemyPlayer)
	{
		m_strategy.UpdateConditions(this, enemyPlayer);
	}
	
	public ITurnAction getTurn()
	{
		return m_strategy.Execute();
	}
	
	public bool Burned { get; set; }
	public bool Frozen { get; set; }
	public bool Paralyzed { get; set; }
	public bool Poisoned { get; set; }
	
	private int m_numTurnsSleeping = 0;	
	public bool IsSleeping()
	{
		return (m_numTurnsSleeping > 0);
	}
	
	public void FallAsleep()
	{
		System.Random r = new System.Random();
		
		int turnsSleeping = r.Next(1, 4);
		m_numTurnsSleeping = Math.Max(m_numTurnsSleeping, turnsSleeping);
	}
	
	private void ClearStatuses()
	{
		Burned = false;
		Frozen = false;
		Paralyzed = false;
		Poisoned = false;
		m_numTurnsSleeping = 0;
	}
	
	public List<string> CompleteTurn()
	{
		List<string> messages = new List<string>();
		
		if (Owner.ActivePokemon != this)
		{
			// only calculate effects if this pokemon is still on the field
			return messages;
		}
		
		if (Burned)
		{
			messages.Add(m_name + " was damaged by the burn!");
			TakeDamage((int)(1.0f / 8.0f * m_maxHP));
		}
		
		if (Poisoned)
		{
			messages.Add(m_name + " was damaged by the poison!");
			TakeDamage((int)(1.0f / 8.0f * m_maxHP));
		}
		
		if (IsSleeping())
		{
			m_numTurnsSleeping -= 1;
			
			if (!IsSleeping())
			{
				messages.Add(m_name + " woke up!");
			}
		}
		
		return messages;
	}
	
	public bool IsStatusAfflicted()
	{
		return (Burned || Frozen || Paralyzed || Poisoned || IsSleeping());
	}
}

