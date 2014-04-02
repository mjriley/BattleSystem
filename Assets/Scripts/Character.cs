using UnityEngine;
using System.Collections;

public class Character
{
	
	private int m_maxHP;
	private int m_currentHP;
	private Ability[] m_abilities = new Ability[4];
	
	public int CurrentHP
	{
		get { return m_currentHP; }
	}
	
	public Character(int maxHP)
	{
		m_maxHP = maxHP;
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
	
	public Ability[] getAbilities()
	{
		return m_abilities;
	}
	
	public void setAbility(uint index, Ability ability)
	{
		m_abilities[index] = ability;
	}

}
