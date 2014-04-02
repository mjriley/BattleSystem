using System;

public class Ability
{
	private Character m_parent;
	
	private string m_name;
	public string Name
	{
		get { return m_name; }
	}
	
	private string m_type;
	public string Type
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
	}
	
	public Ability(string name, string type, int damageAmount, uint maxUses, Character c)
	{
		m_name = name;
		m_type = type;
		m_damageAmount = damageAmount;
		m_maxUses = maxUses;
		m_currentUses = m_maxUses;
		m_parent = c;
	}
	
	public void Execute(Character[] targets)
	{
		if (m_currentUses <= 0)
		{
			throw new Exception("No ability charges left");
		}
		
		foreach (Character target in targets)
		{
			target.TakeDamage(m_damageAmount);
		}
		
		m_currentUses -= 1;
	}
}
