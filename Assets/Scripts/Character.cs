using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character
{
//	public enum Sex
//	{
//		Male,
//		Female
//	}
	
	public Dictionary<Stat, int> m_baseStats = new Dictionary<Stat, int>();
	public Dictionary<Stat, int> m_stages = new Dictionary<Stat, int>();
	
	// stats
	public int MaxHP {
		get
		{
			return (m_definition.HP * 2 + 100) * (int)Level / 100 + 10;
		}
	}
	
	public int Atk { get { return GetCurrentStatValue(Stat.Attack); } }
	public int Def { get { return GetCurrentStatValue(Stat.Defense); } }
	public int SpAtk { get { return GetCurrentStatValue(Stat.SpecialAttack); } }
	public int SpDef { get { return GetCurrentStatValue(Stat.SpecialDefense); } }
	public int Spd { get { return GetCurrentStatValue(Stat.Speed); } }
	public int Accuracy { get { return GetCurrentStatValue(Stat.Accuracy); } }
	public int Evasion { get { return GetCurrentStatValue(Stat.Evasion); } }
	public uint Level { get; set; }
	
	public const int MAX_STAGE = 6;
	public const int MIN_STAGE = -6;
	
	private int m_currentHP;
	private List<AbstractAbility> m_abilities = new List<AbstractAbility>();
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
	
	public List<BattleType> Types { get { return m_types; } }
	
	private Pokemon.Gender m_gender;
	public Pokemon.Gender Gender
	{
		get { return m_gender; }
	}
	
	public Pokemon.Species Species { get { return m_definition.Species; } }
	
	PokemonDefinition m_definition;
	
	public Character(string name, PokemonDefinition definition, Pokemon.Gender gender, uint level, IAttackStrategy strategy)
	{
		m_name = name;
		m_definition = definition;
		m_gender = gender;
		Level = level;
		
		m_strategy = strategy;
		
		InitStats();
		
		Reset();
	}
	
	private void InitStats()
	{
		m_baseStats[Stat.Attack] = 50;
		m_baseStats[Stat.Defense] = 50;
		m_baseStats[Stat.SpecialAttack] = 50;
		m_baseStats[Stat.SpecialDefense] = 50;
		m_baseStats[Stat.Speed] = 50;
	}
	
	public void Reset()
	{
		m_currentHP = MaxHP;
		ResetStages();
		ClearStatuses();
	}
	
	public void ResetStages()
	{
		m_stages.Clear();
		
		m_stages[Stat.Attack] = 0;
		m_stages[Stat.Defense] = 0;
		m_stages[Stat.SpecialAttack] = 0;
		m_stages[Stat.SpecialDefense] = 0;
		m_stages[Stat.Speed] = 0;
		
		m_stages[Stat.Accuracy] = 0;
		m_stages[Stat.Evasion] = 0;
	}
	
	// accuracy stat is whether the stat pertains to accuracy (i.e. accuracy/evasion)
	private double CalculateStageMultiplier(int level, bool accuracyStat)
	{
		int numerator = (accuracyStat) ? 3 : 2;
		int denominator = (accuracyStat) ? 3 : 2;
		
		if (level < 0)
		{
			denominator -= level;
		}
		else if (level > 0)
		{
			numerator += level;
		}
		
		return (double)numerator / (double)denominator;
	}
	
	static Stat[] AccuracyStats = new Stat[] { Stat.Accuracy, Stat.Evasion };
	public int GetCurrentStatValue(Stat stat)
	{
		bool accuracyStat = AccuracyStats.Contains(stat);
		int baseStat = (accuracyStat) ? 100 : m_baseStats[stat];
		
		double computedStat = baseStat * CalculateStageMultiplier(m_stages[stat], accuracyStat);
		
		return (int)computedStat;
	}
	
	// attempts to modify the indicated stat's stage by the specified amount, returns the actual change
	// (change may be less than requested)
	public int ModifyStage(Stat stat, int amount)
	{
		int new_value = m_stages[stat] + amount;
		
		if (new_value > MAX_STAGE)
		{
			new_value = MAX_STAGE;
		}
		else if (new_value < MIN_STAGE)
		{
			new_value = MIN_STAGE;
		}
		
		int difference = new_value - m_stages[stat];
		
		m_stages[stat] = new_value;
		
		return difference;
	}
	
	public void TakeDamage(int damage)
	{
		m_currentHP -= damage;
		
		m_currentHP = Mathf.Min(m_currentHP, MaxHP);
		m_currentHP = Mathf.Max(m_currentHP, 0);
		
		// TODO: REMOVE -- just speeds up testing
		m_currentHP = 0;
	}
	
	public bool isDead()
	{
		return (m_currentHP == 0);
	}
	
	public List<AbstractAbility> getAbilities()
	{
		return new List<AbstractAbility>(m_abilities);
	}
	
	public void addAbility(AbstractAbility ability)
	{
		m_abilities.Add(ability);
	}
	
	public void replaceAbility(int index, AbstractAbility ability)
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
	
	// temporary statuses
	public bool Flinching { get; set; }
	
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
		
		Flinching = false;
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
			TakeDamage((int)(1.0f / 8.0f * MaxHP));
		}
		
		if (Poisoned)
		{
			messages.Add(m_name + " was damaged by the poison!");
			TakeDamage((int)(1.0f / 8.0f * MaxHP));
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

