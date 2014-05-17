using System.Collections.Generic;

public class PokemonPrototype
{
	Pokemon.Species m_species;
	Dictionary<Stat, int> m_baseStats;
	
	List<string> m_abilities;
	List<BattleType> m_types;
	
	float m_maleRatio;
	
	public PokemonPrototype(Pokemon.Species species, BattleType type1, int hp, int atk, int def, int spAtk, int spDef, int spd, List<string> abilities, float maleRatio)
	: this(species, type1, BattleType._None, hp, atk, def, spAtk, spDef, spd, abilities, maleRatio)
	{
	}
	
	public PokemonPrototype(Pokemon.Species species, BattleType type1, BattleType type2, int hp, int atk, int def, int spAtk, int spDef, int spd, List<string> abilities, float maleRatio)
	{
		m_species = species;
		
		m_baseStats = new Dictionary<Stat, int>();
		m_baseStats[Stat.HP] = hp;
		m_baseStats[Stat.Attack] = atk;
		m_baseStats[Stat.Defense] = def;
		m_baseStats[Stat.SpecialAttack] = spAtk;
		m_baseStats[Stat.Speed] = spd;
		
		m_abilities = abilities;
		
		m_types = new List<BattleType>();
		m_types.Add(type1);
		m_types.Add(type2);
		
		m_maleRatio = maleRatio;
	}
	
	public Pokemon.Species Species { get { return m_species; } }
	
	public List<BattleType> Types { get { return m_types; } }
	
	public int GetStat(Stat stat)
	{
		return m_baseStats[stat];
	}
	
	public string GetAbility(int index)
	{
		if (index >= m_abilities.Count)
		{
			return "";
		}
		
		return m_abilities[index];
	}
	
	public int AbilityCount { get { return m_abilities.Count; } }
	
	public int HP { get { return GetStat(Stat.HP); } }
	public int Attack { get { return GetStat(Stat.Attack); } }
	public int Defense { get { return GetStat(Stat.Defense); } }
	public int SpecialAttack { get { return GetStat(Stat.SpecialAttack); } }
	public int SpecialDefense { get { return GetStat(Stat.SpecialDefense); } }
	public int Speed { get { return GetStat(Stat.Speed); } }
	
	public float MaleRatio { get { return m_maleRatio; } }
}
