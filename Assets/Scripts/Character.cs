using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character
{
	
	private int m_maxHP;
	private int m_currentHP;
	private List<Ability> m_abilities = new List<Ability>();
	
	private IAttackStrategy m_strategy;
	
	public int CurrentHP
	{
		get { return m_currentHP; }
	}
	
	public Character(int maxHP, IAttackStrategy strategy = null)
	{
		m_maxHP = maxHP;
		m_currentHP = m_maxHP;
		
		m_strategy = strategy;
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
	
	public void handleTurn(IEnumerable<Character> allies, IEnumerable<Character> enemies)
	{
		uint usageCost = getUsageCost();
		
		AbilityUse turnInfo = m_strategy.Execute(this, allies, enemies);
		
		if (turnInfo.ability == null)
		{
			return;
		}
		
		foreach (Character target in turnInfo.targets)
		{
			target.TakeDamage(turnInfo.ability.DamageAmount);
		}
		
		turnInfo.ability.CurrentUses -= usageCost;
	}

}
