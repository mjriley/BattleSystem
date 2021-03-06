using System.Collections.Generic;

namespace PokeCore {
	using Moves;
namespace Pokemon {

public class PokemonDefinition
{
	Species m_species;
	Dictionary<Stat, int> m_baseStats;
	
	List<string> m_moves;
	List<BattleType> m_types;
	
	float m_maleRatio;
	
	public PokemonDefinition(Species species, BattleType type1, int hp, int atk, int def, int spAtk, int spDef, int spd, List<string> moves, float maleRatio)
	: this(species, type1, BattleType._None, hp, atk, def, spAtk, spDef, spd, moves, maleRatio)
	{
	}
	
	public PokemonDefinition(Species species, BattleType type1, BattleType type2, int hp, int atk, int def, int spAtk, int spDef, int spd, List<string> moves, float maleRatio)
	{
		m_species = species;
		
		m_baseStats = new Dictionary<Stat, int>();
		m_baseStats[Stat.HP] = hp;
		m_baseStats[Stat.Attack] = atk;
		m_baseStats[Stat.Defense] = def;
		m_baseStats[Stat.SpecialAttack] = spAtk;
		m_baseStats[Stat.SpecialDefense] = spDef;
		m_baseStats[Stat.Speed] = spd;
		
		m_moves = moves;
		
		m_types = new List<BattleType>();
		m_types.Add(type1);
		
		// none is only a placeholder -- it shouldn't be part of the final type list
		if (type2 != BattleType._None)
		{
			m_types.Add(type2);
		}
		
		m_maleRatio = maleRatio;
	}
	
	public Species Species { get { return m_species; } }
	
	public List<BattleType> Types { get { return m_types; } }
	
	public int GetStat(Stat stat)
	{
		return m_baseStats[stat];
	}
	
	public string GetMove(int index)
	{
		if (index >= m_moves.Count)
		{
			return "";
		}
		
		return m_moves[index];
	}
	
	public int MoveCount { get { return m_moves.Count; } }
	
	public int HP { get { return GetStat(Stat.HP); } }
	public int Attack { get { return GetStat(Stat.Attack); } }
	public int Defense { get { return GetStat(Stat.Defense); } }
	public int SpecialAttack { get { return GetStat(Stat.SpecialAttack); } }
	public int SpecialDefense { get { return GetStat(Stat.SpecialDefense); } }
	public int Speed { get { return GetStat(Stat.Speed); } }
	
	public float MaleRatio { get { return m_maleRatio; } }
	
	static bool m_isInit = false;
	static Dictionary<Species, PokemonDefinition> m_definitions;
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		m_definitions = new Dictionary<Species, PokemonDefinition>();
		List<string> moves;
		
		// Chespin
		moves = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", 
			"Pin Missile", "Take Down", "Seed Bomb", "Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"};
		AddDefinition(new PokemonDefinition(Species.Chespin, BattleType.Grass, 56, 61, 65, 48, 45, 38, moves, 87.5f));
		
		// Quilladin
		moves = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", "Pin Missile", "Needle Arm", "Take Down", "Seed Bomb",
			"Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"};
		AddDefinition(new PokemonDefinition(Species.Quilladin, BattleType.Grass, 61, 78, 95, 56, 58, 57, moves, 87.5f));
		
		// Chesnaught
		moves = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout",
			"Bite", "Leech Seed", "Pin Missile", "Needle Arm", "Take Down", "Seed Bomb", "Spiky Shield", "Mud Shot",
			"Bulk Up", "Body Slam", "Pain Split", "Wood Hammer", "Hammer Arm", "Giga Impact", "Feint", "Hammer Arm", "Belly Drum"};
		AddDefinition(new PokemonDefinition(Species.Chesnaught, BattleType.Grass, BattleType.Fight, 88, 107, 122, 74, 75, 64, moves, 87.5f));
		
		// Fennekin
		moves = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Flamethrower",
			"Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room", "Fire Blast" };
		AddDefinition(new PokemonDefinition(Species.Fennekin, BattleType.Fire, 40, 45, 40, 62, 60, 60, moves, 87.5f));
		
		// Braixen
		moves = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Flamethrower",
			"Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room", "Fire Blast" };
		AddDefinition(new PokemonDefinition(Species.Braixen, BattleType.Fire, 59, 59, 58, 90, 70, 73, moves, 87.5f));
		
		// Delphox
		moves = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl",
			"Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Mystical Fire", "Flamethrower", "Will-O-Wisp",
			"Psychic", "Sunny Day", "Magic Room", "Fire Blast", "Future Sight", "Mystical Fire", "Future Sight", "Role Play", "Switcheroo", "Shadow Ball"};
		AddDefinition(new PokemonDefinition(Species.Delphox, BattleType.Fire, BattleType.Psychic, 75, 69, 72, 114, 100, 104, moves, 87.5f));
		
		// Froakie
		moves = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling", "Smack Down", "Substitute", "Bounce",
			"Double Team", "Hydro Pump"};
		AddDefinition(new PokemonDefinition(Species.Froakie, BattleType.Water, 41, 56, 40, 62, 44, 71, moves, 87.5f));
		
		// Frogadier
		moves = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling", "Smack Down", "Substitute", "Bounce",
			"Double Team", "Hydro Pump"};
		AddDefinition(new PokemonDefinition(Species.Frogadier, BattleType.Water, 54, 63, 52, 83, 56, 97, moves, 87.5f));
		
		// Greninja
		moves = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen",
			"Shadow Sneak", "Spikes", "Feint Attack", "Water Shuriken", "Substitute", "Extrasensory", "Double Team", "Haze", "Hydro Pump",
			"Night Slash", "Role Play", "Mat Block"};
		AddDefinition(new PokemonDefinition(Species.Greninja, BattleType.Water, BattleType.Dark, 72, 95, 67, 103, 71, 122, moves, 87.5f));
		
		// Bunnelby
		moves = new List<string> {"Tackle", "Agility", "Leer", "Quick Attack", "Double Slap", "Mud-Slap", "Double Kick", "Odor Sleuth", "Flail", "Dig",
			"Bounce", "Super Fang", "Facade", "Earthquake"};
		AddDefinition(new PokemonDefinition(Species.Bunnelby, BattleType.Normal, 38, 36, 38, 32, 36, 57, moves, 50));
		
		// Diggersby
		moves = new List<string> {"Tackle", "Agility", "Leer", "Quick Attack", "Mud-Slap", "Take Down",
			"Mud Shot", "Double Kick", "Odor Sleuth", "Flail", "Dig", "Bounce", "Super Fang", "Facade", "Earthquake",
			"Hammer Arm", "Rototiller", "Bulldoze", "Swords Dance"};
		AddDefinition(new PokemonDefinition(Species.Diggersby, BattleType.Normal, BattleType.Ground, 85, 56, 77, 50, 77, 78, moves, 50));
		
		// Zigzagoon
		moves = new List<string> {"Growl", "Tackle", "Tail Whip", "Headbutt", "Baby-Doll Eyes", "Sand Attack", "Odor Sleuth", "Mud Sport", "Pin Missile", "Covet",
			"Bestow", "Flail", "Rest", "Belly Drum", "Fling"};
		AddDefinition(new PokemonDefinition(Species.Zigzagoon, BattleType.Normal, 38, 30, 41, 30, 41, 60, moves, 50));
		
		// Bulbasaur
		moves = new List<string> {"Tackle", "Growl", "Leech Seed", "Vine Whip", "Poison Powder", "Sleep Powder", "Take Down", "Razor Leaf", "Sweet Scent", "Growth", 
			"Double-Edge", "Worry Seed", "Synthesis", "Seed Bomb"};
		AddDefinition(new PokemonDefinition(Species.Bulbasaur, BattleType.Grass, BattleType.Poison, 45, 49, 49, 65, 65, 45, moves, 87.5f));
		
		// Charmander
		moves = new List<string> {"Scratch", "Growl", "Ember", "Smokescreen", "Dragon Rage", "Scary Face", "Fire Fang", "Flame Burst", "Slash", "Flamethrower", "Fire Spin",
			"Inferno"};
		AddDefinition(new PokemonDefinition(Species.Charmander, BattleType.Fire, 39, 52, 43, 60, 50, 65, moves, 87.5f));
		
		// Magikarp
		moves = new List<string> {"Splash", "Tackle", "Flail"};
		AddDefinition(new PokemonDefinition(Species.Magikarp, BattleType.Water, 20, 10, 55, 15, 20, 80, moves, 50));
		
		// Pikachu
		moves = new List<string> {"Tail Whip", "Thunder Shock", "Growl", "Play Nice", "Quick Attack", 
			"Thunder Wave", "Electro Ball", "Double Team", "Nuzzle", "Slam", "Thunderbolt", "Feint", "Agility", 
			"Discharge", "Light Screen", "Thunder" };
		AddDefinition(new PokemonDefinition(Species.Pikachu, BattleType.Electric, 35, 55, 30, 50, 40, 90, moves, 50));
		
		// Squirtle
		moves = new List<string> {"Tackle", "Tail Whip", "Water Gun", "Withdraw", "Bubble", "Bite", "Rapid Spin", "Protect", "Water Pulse", "Aqua Tail", "Skull Bash",
			"Iron Defense", "Rain Dance", "Hydro Pump"};
		AddDefinition(new PokemonDefinition(Species.Squirtle, BattleType.Water, 44, 48, 65, 50, 64, 43, moves, 87.5f));
		m_isInit = true;
	}
	
	static void AddDefinition(PokemonDefinition definition)
	{
		m_definitions[definition.Species] = definition;
	}
	
	public static PokemonDefinition GetEntry(Species species)
	{
		Init();
		
		return m_definitions[species];
	}
	
}

}}
