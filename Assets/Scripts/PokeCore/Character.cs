using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Abilities;
using Abilities.Status;
using Pokemon;

namespace PokeCore {

public class Character : ICharacter
{
	static Dictionary<Stat, int> PerfectIVs = new Dictionary<Stat, int>() {
		{Stat.HP, 31},
		{Stat.Attack, 31},
		{Stat.Defense, 31},
		{Stat.SpecialAttack, 31},
		{Stat.SpecialDefense, 31},
		{Stat.Speed, 31}
	};
	
	static Dictionary<Stat, int> ZeroedEVs = new Dictionary<Stat, int>() {
		{Stat.HP, 0},
		{Stat.Attack, 0},
		{Stat.Defense, 0},
		{Stat.SpecialAttack, 0},
		{Stat.SpecialDefense, 0},
		{Stat.Speed, 0}
	};
	
	public Dictionary<Stat, int> m_baseStats = new Dictionary<Stat, int>();
	public Dictionary<Stat, int> m_stages = new Dictionary<Stat, int>();
	Dictionary<Stat, int> m_IVs = new Dictionary<Stat, int>();
	Dictionary<Stat, int> m_EVs = new Dictionary<Stat, int>();
	Nature m_nature;
	
	// stats
	public int MaxHP { get { return m_baseStats[Stat.HP]; } }
	
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
	private List<Ability> m_abilities = new List<Ability>();
	private string m_name;
	
	private WeakReference m_refOwner;
	public Player Owner
	{
		get { return (Player)m_refOwner.Target; }
		set { m_refOwner = new WeakReference(value); }
	}
	
	private bool m_isInvisible = false;
	public bool IsInvisible
	{
		get { return m_isInvisible; }
		set { m_isInvisible = value; }
	}
	
	public string Name
	{
		get { return m_name; }
	}
	
	public int CurrentHP
	{
		get { return m_currentHP; }
	}
	
	public List<BattleType> Types { get { return m_definition.Types; } }
	
	private Pokemon.Gender m_gender;
	public Pokemon.Gender Gender
	{
		get { return m_gender; }
	}
	
	public Pokemon.Species Species { get { return m_definition.Species; } }
	
	PokemonDefinition m_definition;
	
	public Character(string name, PokemonDefinition definition, Pokemon.Gender gender, uint level, 
		Dictionary<Stat, int> ivs=null, Dictionary<Stat, int> evs=null, Nature nature=null)
	{
		m_name = name;
		m_definition = definition;
		m_gender = gender;
		Level = level;
		
		if (ivs == null)
		{
			ivs = PerfectIVs;
		}
		m_IVs = ivs;
		
		if (evs == null)
		{
			evs = ZeroedEVs;
		}
		m_EVs = evs;
		
		if (nature == null)
		{
			// This nature has no effect on any attributes
			nature = NatureFactory.GetNature(Nature.Type.Hardy);
		}
		m_nature = nature;
		
		InitStats();
		
		Reset();
	}
	
	private void CalculateBaseStat(Stat stat)
	{
		int constantBase = (stat == Stat.HP) ? 10 : 5;
		int multiplierBase = (stat == Stat.HP) ? 100 : 0;
		double natureMultiplier = (stat == Stat.HP) ? 1.0 : m_nature.GetMultiplier(stat);
		
		int iv = 0;
		m_IVs.TryGetValue(stat, out iv);
		
		int ev = 0;
		m_EVs.TryGetValue(stat, out ev);
		
		double perLevel = iv + 2 * m_definition.GetStat(stat) + ev / 4.0 + multiplierBase;
		m_baseStats[stat] = (int)((perLevel * (int)Level / 100 + constantBase) * natureMultiplier);
	}
	
	private void InitStats()
	{
		CalculateBaseStat(Stat.HP);
		CalculateBaseStat(Stat.Attack);
		CalculateBaseStat(Stat.Defense);
		CalculateBaseStat(Stat.SpecialAttack);
		CalculateBaseStat(Stat.SpecialDefense);
		CalculateBaseStat(Stat.Speed);
		
		m_currentHP = MaxHP;
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
	
	public static string GetStageModificationMessage(string name, Stat stat, int stage_change, bool isIncrease)
	{
		string suffix;
		if (isIncrease)
		{
			switch (stage_change)
			{
				case 0: 
					suffix = L18N.Get("STAGE_INCREASE_0");
					break;
				case 1:
					suffix = L18N.Get("STAGE_INCREASE_1");
					break;
				case 2:
					suffix = L18N.Get("STAGE_INCREASE_2");
					break;
				default:
					suffix = L18N.Get("STAGE_INCREASE_3");
					break;
			}
		}
		else
		{
			switch (stage_change)
			{
				case 0:
					suffix = L18N.Get("STAGE_DECREASE_0");
					break;
				case -1:
					suffix = L18N.Get("STAGE_DECREASE_1");
					break;
				case -2:
					suffix = L18N.Get("STAGE_DECREASE_2");
					break;
				default:
					suffix = L18N.Get("STAGE_DECREASE_3");
					break;
			}
		}
		
		string format = L18N.Get("STAT_CHANGE");
		string result = String.Format(format, name, stat.LocalName(), suffix);
		
		return result;
	}
	
	// Applies the provided amount of "damage" (can heal)
	// and returns the actual amount that occurred
	public int TakeDamage(int damage)
	{
		int initialHP = m_currentHP;
		m_currentHP -= damage;
		
		m_currentHP = Mathf.Min(m_currentHP, MaxHP);
		m_currentHP = Mathf.Max(m_currentHP, 0);
		
		
		// REMOVE, just to speed up testing
		m_currentHP = 0;
		
		
		//return (initialHP - m_currentHP);
		return initialHP;
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
	
	public bool Burned { get; set; }
	public bool Frozen { get; set; }
	public bool Paralyzed { get; set; }
	public bool Poisoned { get; set; }
	
	// temporary statuses
	public bool Flinching { get; set; }
	
	// statuses that clear on switch
	public bool Seeded { get; set; }
	
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
		
		Seeded = false;
	}
	
	public bool ClearStatus(StatusType status)
	{
		return false;
	}
	
	public void CompleteTurn(Character enemy, ref ActionStatus status)
	{
		if (Owner.ActivePokemon != this)
		{
			// only calculate effects if this pokemon is still on the field
			//return messages;
			return;
		}
		
		if (Burned)
		{
			status.events.Add(new StatusUpdateEventArgs(m_name + " was damaged by the burn!"));
			int amount = (int)(1.0f / 8.0f * MaxHP);
			TakeDamage(amount);
			status.events.Add(new DamageEventArgs(this.Owner, amount));
		}
		
		if (Poisoned)
		{
			status.events.Add(new StatusUpdateEventArgs(m_name + " was damaged by the poison!"));
			int amount = (int)(1.0f / 8.0f * MaxHP);
			TakeDamage(amount);
			status.events.Add(new DamageEventArgs(this.Owner, amount));
		}
		
		if (IsSleeping())
		{
			m_numTurnsSleeping -= 1;
			
			if (!IsSleeping())
			{
				status.events.Add(new StatusUpdateEventArgs(m_name + " woke up!"));
			}
		}
		
		if (Seeded)
		{
			SeededStatus.ProcessStatus(this, enemy, ref status);
		}
	}
	
	public bool IsStatusAfflicted()
	{
		return (Burned || Frozen || Paralyzed || Poisoned || IsSleeping());
	}
}

}
