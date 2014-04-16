using UnityEngine;
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
}
