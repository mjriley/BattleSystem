using System;
using System.Linq;
using System.Collections.Generic;

public class PokemonFactory
{
	static Dictionary<Pokemon.Species, PokemonPrototype> m_pokemonPrototypes;
	static bool m_isInit = false;
	
	static void AddPrototype(PokemonPrototype prototype)
	{
		m_pokemonPrototypes[prototype.Species] = prototype;
	}
	
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		m_pokemonPrototypes = new Dictionary<Pokemon.Species, PokemonPrototype>();
		
		List<string> abilities;
		
		// Chespin
		abilities = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", 
			"Pin Missile", "Take Down", "Seed Bomb", "Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Chespin, BattleType.Grass, 56, 61, 65, 48, 45, 38, abilities, 87.5f));
		
		// Quilladin
		abilities = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", "Pin Missile", "Needle Arm", "Take Down", "Seed Bomb",
			"Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Quilladin, BattleType.Grass, 61, 78, 95, 56, 58, 57, abilities, 87.5f));
		
		// Chesnaught
		abilities = new List<string> {"Tackle", "Growl", "Vine Whip", "Rollout",
			"Bite", "Leech Seed", "Pin Missile", "Needle Arm", "Take Down", "Seed Bomb", "Spiky Shield", "Mud Shot",
			"Bulk Up", "Body Slam", "Pain Split", "Wood Hammer", "Hammer Arm", "Giga Impact", "Feint", "Hammer Arm", "Belly Drum"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Chesnaught, BattleType.Grass, BattleType.Fight, 88, 107, 122, 74, 75, 64, abilities, 87.5f));
		
		// Fennekin
		abilities = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Flamethrower",
			"Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room", "Fire Blast" };
		AddPrototype(new PokemonPrototype(Pokemon.Species.Fennekin, BattleType.Fire, 40, 45, 40, 62, 60, 60, abilities, 87.5f));
		
		// Braixen
		abilities = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Flamethrower",
			"Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room", "Fire Blast" };
		AddPrototype(new PokemonPrototype(Pokemon.Species.Braixen, BattleType.Fire, 59, 59, 58, 90, 70, 73, abilities, 87.5f));
		
		// Delphox
		abilities = new List<string> {"Scratch", "Tail Whip", "Ember", "Howl",
			"Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Mystical Fire", "Flamethrower", "Will-O-Wisp",
			"Psychic", "Sunny Day", "Magic Room", "Fire Blast", "Future Sight", "Mystical Fire", "Future Sight", "Role Play", "Switcheroo", "Shadow Ball"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Delphox, BattleType.Fire, BattleType.Psychic, 75, 69, 72, 114, 100, 104, abilities, 87.5f));
		
		// Froakie
		abilities = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling", "Smack Down", "Substitute", "Bounce",
			"Double Team", "Hydro Pump"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Froakie, BattleType.Water, 41, 56, 40, 62, 44, 71, abilities, 87.5f));
		
		// Frogadier
		abilities = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling", "Smack Down", "Substitute", "Bounce",
			"Double Team", "Hydro Pump"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Frogadier, BattleType.Water, 54, 63, 52, 83, 56, 97, abilities, 87.5f));
		
		// Greninja
		abilities = new List<string> {"Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen",
			"Shadow Sneak", "Spikes", "Feint Attack", "Water Shuriken", "Substitute", "Extrasensory", "Double Team", "Haze", "Hydro Pump",
			"Night Slash", "Role Play", "Mat Block"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Greninja, BattleType.Water, BattleType.Dark, 72, 95, 67, 103, 71, 122, abilities, 87.5f));
		
		// Bunnelby
		abilities = new List<string> {"Tackle", "Agility", "Leer", "Quick Attack", "Double Slap", "Mud-Slap", "Double Kick", "Odor Sleuth", "Flail", "Dig",
			"Bounce", "Super Fang", "Facade", "Earthquake"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Bunnelby, BattleType.Normal, 38, 36, 38, 32, 36, 57, abilities, 50));
		
		// Diggersby
		abilities = new List<string> {"Tackle", "Agility", "Leer", "Quick Attack", "Mud-Slap", "Take Down",
			"Mud Shot", "Double Kick", "Odor Sleuth", "Flail", "Dig", "Bounce", "Super Fang", "Facade", "Earthquake",
			"Hammer Arm", "Rototiller", "Bulldoze", "Swords Dance"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Diggersby, BattleType.Normal, BattleType.Ground, 85, 56, 77, 50, 77, 78, abilities, 50));
		
		// Zigzagoon
		abilities = new List<string> {"Growl", "Tackle", "Tail Whip", "Headbutt", "Baby-Doll Eyes", "Sand Attack", "Odor Sleuth", "Mud Sport", "Pin Missile", "Covet",
			"Bestow", "Flail", "Rest", "Belly Drum", "Fling"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Zigzagoon, BattleType.Normal, 38, 30, 41, 30, 41, 60, abilities, 50));
		
		// Bulbasaur
		abilities = new List<string> {"Tackle", "Growl", "Leech Seed", "Vine Whip", "Poison Powder", "Sleep Powder", "Take Down", "Razor Leaf", "Sweet Scent", "Growth", 
			"Double-Edge", "Worry Seed", "Synthesis", "Seed Bomb"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Bulbasaur, BattleType.Grass, BattleType.Poison, 45, 49, 49, 65, 65, 45, abilities, 87.5f));
		
		// Charmander
		abilities = new List<string> {"Scratch", "Growl", "Ember", "Smokescreen", "Dragon Rage", "Scary Face", "Fire Fang", "Flame Burst", "Slash", "Flamethrower", "Fire Spin",
			"Inferno"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Charmander, BattleType.Fire, 39, 52, 43, 60, 50, 65, abilities, 87.5f));
		
		// Magikarp
		abilities = new List<string> {"Splash", "Tackle", "Flail"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Magikarp, BattleType.Water, 20, 10, 55, 15, 20, 80, abilities, 50));
		
		// Pikachu
		abilities = new List<string> {"Tail Whip", "Thunder Shock", "Growl", "Play Nice", "Quick Attack", 
			"Thunder Wave", "Electro Ball", "Double Team", "Nuzzle", "Slam", "Thunderbolt", "Feint", "Agility", 
			"Discharge", "Light Screen", "Thunder" };
		AddPrototype(new PokemonPrototype(Pokemon.Species.Pikachu, BattleType.Electric, 35, 55, 30, 50, 40, 90, abilities, 50));
		
		// Squirtle
		abilities = new List<string> {"Tackle", "Tail Whip", "Water Gun", "Withdraw", "Bubble", "Bite", "Rapid Spin", "Protect", "Water Pulse", "Aqua Tail", "Skull Bash",
			"Iron Defense", "Rain Dance", "Hydro Pump"};
		AddPrototype(new PokemonPrototype(Pokemon.Species.Squirtle, BattleType.Water, 44, 48, 65, 50, 64, 43, abilities, 87.5f));
		
		m_isInit = true;
	}
	
	public static Character CreatePokemon(Pokemon.Species species, uint level, string name="", Pokemon.Gender gender=Pokemon.Gender.Random, IAttackStrategy strategy=null)
	{
		Init();
		
		PokemonPrototype prototype = m_pokemonPrototypes[species];
		
		if (name == "")
		{
			name = species.ToString();
		}
		
		if (gender == Pokemon.Gender.Random)
		{
			Random generator = new Random();
			double rand = generator.NextDouble() * 100;
			gender = (rand < prototype.MaleRatio) ? Pokemon.Gender.Male : Pokemon.Gender.Female;
		}
		
		Character pokemon = new Character(name, prototype, gender, level, strategy);
		
		for (int i = 0; i < 4; ++i)
		{
			string abilityName = prototype.GetAbility(i);
			
			if (abilityName == "")
			{
				// If it's an empty string, there are no more abilities to fetch
				break;
			}
			
			AbstractAbility ability = AbilityFactory.GetAbility(abilityName);
			pokemon.addAbility(ability);
		}
		
		return pokemon;
	}
}
