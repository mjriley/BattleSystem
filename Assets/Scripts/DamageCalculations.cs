using System;
using System.Collections.Generic;

public struct DamageResult
{
	public int amount;
	public bool crit;
	public Effectiveness effectiveness;
};

public class DamageCalculations
{
	const double SAME_TYPE_MULTIPLIER = 1.5;
	
	const double BASE_CRIT_MULTIPLIER = 1.5;
	
	// always a 1 in X chance of crit.
	// Storing the denominators rather than the fraction to avoid 1/3
	static int[] CRIT_TABLE = new int[] { 16, 8, 4, 3, 2 };
		
	public static int CalculateBaseDamage(int attack, int defense, uint level, 
		uint power, double same_type_multiplier, double effectiveness_multiplier, 
		double crit_multiplier, double variance_multiplier)
	{
		double level_factor = (2.0 * level + 10) / 250.0;
		double attack_ratio = (double)attack / (double)defense;
		double raw_damage = level_factor * attack_ratio * power + 2;
		
		// TODO: Account for items, abilities, etc
		double modifier = same_type_multiplier * effectiveness_multiplier * crit_multiplier * variance_multiplier;
		
		return (int)(raw_damage * modifier);
	}
	
	private static Effectiveness GetEffectiveness(double multiplier)
	{
		if (multiplier > 1.0)
		{
			return Effectiveness.SuperEffective;
		}
		else if (multiplier == 0)
		{
			return Effectiveness.Immune;
		}
		else if (multiplier < 1.0)
		{
			return Effectiveness.Weak;
		}
		
		return Effectiveness.Normal;
	}
	
	private static bool WillCrit(Character attacker, Random generator)
	{
		int crit_liklihood = CRIT_TABLE[CalculateCritStage(attacker)];
		int rand = generator.Next(crit_liklihood);
		
		if (rand == 0)
		{
			return true;
		}
		
		return false;
	}
	
	public static DamageResult CalculateDamage(DamageAbility ability, Character attacker, Character defender, Random generator)
	{
		DamageResult result = new DamageResult();
		
		double same_type_multiplier = attacker.Types.Contains(ability.BattleType) ? SAME_TYPE_MULTIPLIER : 1.0;
		
		result.crit = WillCrit(attacker, generator);
		double crit_multiplier = result.crit ? BASE_CRIT_MULTIPLIER : 1.0;
		
		double effectiveness_multiplier = getDamageMultiplier(ability.BattleType, defender.Types);
		result.effectiveness = GetEffectiveness(effectiveness_multiplier);
		
		double variance_multiplier = GetDamageVariance(generator);
		
		int attack, defense;
		if (ability.AbilityType == AbilityType.Physical)
		{
			attack = attacker.Atk;
			defense = defender.Def;
		}
		else
		{
			attack = attacker.SpAtk;
			defense = defender.SpDef;
		}
		
		result.amount = CalculateBaseDamage(attack, defense, attacker.Level, ability.Power, same_type_multiplier, effectiveness_multiplier, crit_multiplier, variance_multiplier);
		
		return result;
	}
		
	private static int CalculateCritStage(Character pokemon)
	{
		return 0;
	}
	// Returns a value between 0.85 and 1.0 to be the damage 'noise'
	private static double GetDamageVariance(Random generator)
	{
		double rand = generator.NextDouble();
		double baseMultiplier = 1.0;
		double varianceRange = 0.15;
		
		return baseMultiplier - varianceRange * rand;
	}

	public static double getDamageMultiplier(BattleType attackType, List<BattleType> pokemonTypes)
	{
		Init();
		
		double multiplier = 1.0;
		
		foreach (BattleType pokemonType in pokemonTypes)
		{
			multiplier *= m_damageTable[pokemonType][attackType];
		}
		
		return multiplier;
	}
	
	private static bool m_isInit = false;
	
	private static void Init()
	{
		if (!m_isInit)
		{
			createDamageTable();
			m_isInit = true;
		}
	}
	
	private static Dictionary<BattleType, Dictionary<BattleType, float> > m_damageTable;
	
	// helper function to initialize all values to 1.0
	// the values should be tweaked afterwards
	private static Dictionary<BattleType, float> createTypeTable()
	{
		Dictionary<BattleType, float> dict = new Dictionary<BattleType, float>();
		string[] types = Enum.GetNames(typeof(BattleType));
		foreach (string name in types)
		{
			BattleType type = (BattleType)Enum.Parse(typeof(BattleType), name);
			dict[type] = 1.0f;
		}
		
		return dict;
	}
	
	private static void createDamageTable()
	{
		m_damageTable = new Dictionary<BattleType, Dictionary<BattleType, float> >();
		
		Dictionary<BattleType, float> normal = createTypeTable();
		normal[BattleType.Fight] = 2.0f;
		normal[BattleType.Ghost] = 0.0f;
		m_damageTable[BattleType.Normal] = normal;
		
		Dictionary<BattleType, float> fire = createTypeTable();
		fire[BattleType.Fire] = 0.5f;
		fire[BattleType.Water] = 2.0f;
		fire[BattleType.Grass] = 0.5f;
		fire[BattleType.Ice] = 0.5f;
		fire[BattleType.Ground] = 2.0f;
		fire[BattleType.Bug] = 0.5f;
		fire[BattleType.Rock] = 2.0f;
		fire[BattleType.Steel] = 0.5f;
		fire[BattleType.Fairy] = 0.5f;
		m_damageTable[BattleType.Fire] = fire;
		
		Dictionary<BattleType, float> water = createTypeTable();
		water[BattleType.Fire] = 0.5f;
		water[BattleType.Water] = 0.5f;
		water[BattleType.Electric] = 2.0f;
		water[BattleType.Grass] = 2.0f;
		water[BattleType.Ice] = 0.5f;
		water[BattleType.Steel] = 0.5f;
		m_damageTable[BattleType.Water] = water;
		
		Dictionary<BattleType, float> electric = createTypeTable();
		electric[BattleType.Electric] = 0.5f;
		electric[BattleType.Ground] = 2.0f;
		electric[BattleType.Flying] = 0.5f;
		electric[BattleType.Steel] = 0.5f;
		m_damageTable[BattleType.Electric] = electric;
		
		Dictionary<BattleType, float> grass = createTypeTable();
		grass[BattleType.Fire] = 2.0f;
		grass[BattleType.Water] = 0.5f;
		grass[BattleType.Electric] = 0.5f;
		grass[BattleType.Grass] = 0.5f;
		grass[BattleType.Ice] = 2.0f;
		grass[BattleType.Poison] = 2.0f;
		grass[BattleType.Ground] = 0.5f;
		grass[BattleType.Flying] = 2.0f;
		grass[BattleType.Bug] = 2.0f;
		m_damageTable[BattleType.Grass] = grass;
		
		Dictionary<BattleType, float> ice = createTypeTable();
		ice[BattleType.Fire] = 2.0f;
		ice[BattleType.Ice] = 0.5f;
		ice[BattleType.Fight] = 2.0f;
		ice[BattleType.Rock] = 2.0f;
		ice[BattleType.Steel] = 2.0f;
		m_damageTable[BattleType.Ice] = ice;
		
		Dictionary<BattleType, float> fight = createTypeTable();
		fight[BattleType.Flying] = 2.0f;
		fight[BattleType.Psychic] = 2.0f;
		fight[BattleType.Bug] = 0.5f;
		fight[BattleType.Rock] = 0.5f;
		fight[BattleType.Dark] = 0.5f;
		fight[BattleType.Fairy] = 2.0f;
		m_damageTable[BattleType.Fight] = fight;
		
		Dictionary<BattleType, float> poison = createTypeTable();
		poison[BattleType.Grass] = 0.5f;
		poison[BattleType.Fight] = 0.5f;
		poison[BattleType.Poison] = 0.5f;
		poison[BattleType.Ground] = 2.0f;
		poison[BattleType.Psychic] = 2.0f;
		poison[BattleType.Bug] = 0.5f;
		poison[BattleType.Fairy] = 0.5f;
		m_damageTable[BattleType.Poison] = poison;
		
		Dictionary<BattleType, float> ground = createTypeTable();
		ground[BattleType.Water] = 2.0f;
		ground[BattleType.Electric] = 0.0f;
		ground[BattleType.Grass] = 2.0f;
		ground[BattleType.Ice] = 2.0f;
		ground[BattleType.Poison] = 0.5f;
		ground[BattleType.Rock] = 0.5f;
		m_damageTable[BattleType.Ground] = ground;
		
		Dictionary<BattleType, float> flying = createTypeTable();
		flying[BattleType.Electric] = 2.0f;
		flying[BattleType.Grass] = 0.5f;
		flying[BattleType.Ice] = 2.0f;
		flying[BattleType.Fight] = 0.5f;
		flying[BattleType.Ground] = 0.0f;
		flying[BattleType.Bug] = 0.5f;
		flying[BattleType.Rock] = 2.0f;
		m_damageTable[BattleType.Flying] = flying;
		
		Dictionary<BattleType, float> psychic = createTypeTable();
		psychic[BattleType.Fight] = 0.5f;
		psychic[BattleType.Psychic] = 0.5f;
		psychic[BattleType.Bug] = 2.0f;
		psychic[BattleType.Ghost] = 2.0f;
		psychic[BattleType.Dark] = 2.0f;
		m_damageTable[BattleType.Psychic] = psychic;
		
		Dictionary<BattleType, float> bug = createTypeTable();
		bug[BattleType.Fire] = 2.0f;
		bug[BattleType.Grass] = 0.5f;
		bug[BattleType.Fight] = 0.5f;
		bug[BattleType.Ground] = 0.5f;
		bug[BattleType.Flying] = 2.0f;
		bug[BattleType.Rock] = 2.0f;
		m_damageTable[BattleType.Bug] = bug;
		
		Dictionary<BattleType, float> rock = createTypeTable();
		rock[BattleType.Normal] = 0.5f;
		rock[BattleType.Fire] = 0.5f;
		rock[BattleType.Water] = 2.0f;
		rock[BattleType.Grass] = 2.0f;
		rock[BattleType.Fight] = 2.0f;
		rock[BattleType.Poison] = 0.5f;
		rock[BattleType.Ground] = 2.0f;
		rock[BattleType.Flying] = 0.5f;
		rock[BattleType.Steel] = 2.0f;
		m_damageTable[BattleType.Rock] = rock;
		
		Dictionary<BattleType, float> ghost = createTypeTable();
		ghost[BattleType.Normal] = 0.0f;
		ghost[BattleType.Fight] = 0.0f;
		ghost[BattleType.Poison] = 0.5f;
		ghost[BattleType.Bug] = 0.5f;
		ghost[BattleType.Ghost] = 2.0f;
		ghost[BattleType.Dark] = 2.0f;
		m_damageTable[BattleType.Ghost] = ghost;
		
		Dictionary<BattleType, float> dragon = createTypeTable();
		dragon[BattleType.Fire] = 0.5f;
		dragon[BattleType.Water] = 0.5f;
		dragon[BattleType.Electric] = 0.5f;
		dragon[BattleType.Grass] = 0.5f;
		dragon[BattleType.Ice] = 2.0f;
		dragon[BattleType.Dragon] = 2.0f;
		dragon[BattleType.Fairy] = 2.0f;
		m_damageTable[BattleType.Dragon] = dragon;
		
		Dictionary<BattleType, float> dark = createTypeTable();
		dark[BattleType.Fight] = 2.0f;
		dark[BattleType.Psychic] = 0.0f;
		dark[BattleType.Bug] = 2.0f;
		dark[BattleType.Ghost] = 0.5f;
		dark[BattleType.Dragon] = 0.5f;
		dark[BattleType.Fairy] = 2.0f;
		m_damageTable[BattleType.Dark] = dark;
		
		Dictionary<BattleType, float> steel = createTypeTable();
		steel[BattleType.Normal] = 0.5f;
		steel[BattleType.Fire] = 2.0f;
		steel[BattleType.Grass] = 0.5f;
		steel[BattleType.Ice] = 0.5f;
		steel[BattleType.Fight] = 2.0f;
		steel[BattleType.Poison] = 0.0f;
		steel[BattleType.Ground] = 2.0f;
		steel[BattleType.Flying] = 0.5f;
		steel[BattleType.Psychic] = 0.5f;
		steel[BattleType.Bug] = 0.5f;
		steel[BattleType.Rock] = 0.5f;
		steel[BattleType.Dragon] = 0.5f;
		steel[BattleType.Steel] = 0.5f;
		steel[BattleType.Fairy] = 0.5f;
		m_damageTable[BattleType.Steel] = steel;
		
		Dictionary<BattleType, float> fairy = createTypeTable();
		fairy[BattleType.Fight] = 0.5f;
		fairy[BattleType.Poison] = 2.0f;
		fairy[BattleType.Bug] = 0.5f;
		fairy[BattleType.Dragon] = 0.0f;
		fairy[BattleType.Dark] = 0.5f;
		fairy[BattleType.Steel] = 2.0f;
		m_damageTable[BattleType.Fairy] = fairy;
	}
	
}
