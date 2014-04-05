using System;
using System.Collections.Generic;

public class BattleSystem
{
	List<Character> m_playerPokemon = new List<Character>();
	Character m_activePokemon = null;
	
	Random m_generator = null;
	
	PlayerAbilityChoiceHandler m_abilityHandler;
	TextAlertHandler m_textHandler;
	
	public delegate int PlayerAbilityChoiceHandler(List<Ability> abilities);
	public delegate void TextAlertHandler(string message);
	
	public BattleSystem(TextAlertHandler textHandler, PlayerAbilityChoiceHandler abilityHandler, Random generator = null)
	{
		m_textHandler = textHandler;
		m_abilityHandler = abilityHandler;
		
		m_generator = m_generator;
		if (m_generator == null)
		{
			m_generator = new Random();
		}
	}
	
	public void CreatePlayerPokemon()
	{
		UserInputStrategy userStrategy = new UserInputStrategy(m_abilityHandler);
		Character player_pokemon = new Character("My Pokemon", 70, userStrategy);
		
		Ability ability0 = new Ability("Ability 0", "Normal", 50, 20);
		Ability ability1 = new Ability("Ability 1", "Normal", 10, 20);
		Ability ability2 = new Ability("Ability 2", "Normal", 10, 20);
		Ability ability3 = new Ability("Ability 3", "Normal", 10, 20);
		player_pokemon.addAbility(ability0);
		player_pokemon.addAbility(ability1);
		player_pokemon.addAbility(ability2);
		player_pokemon.addAbility(ability3);
		
		addPlayerPokemon(player_pokemon);
		setActivePokemon(0);
	}
	
	
	public void Start()
	{
		Character enemy = generateEnemyPokemon();
//		do
//		{
			List<Character> allies = new List<Character>();
			allies.Add(m_activePokemon);
			
			List<Character> enemies = new List<Character>();
			enemies.Add(enemy);
			AbilityUse player_turn = m_activePokemon.getTurnAbility(null, enemies);
			AbilityUse enemy_turn = enemy.getTurnAbility(null, allies);
			
			// determine order of the turns in the queue
			
			resolveTurn(player_turn);
			resolveTurn(enemy_turn);
			
//		} while (!m_activePokemon.isDead() && !enemy.isDead());
	}
	
	public void addPlayerPokemon(Character pokemon)
	{
		m_playerPokemon.Add(pokemon);
	}
	
	public void setActivePokemon(uint index)
	{
		m_activePokemon = m_playerPokemon[(int)index];
	}
	
	public Character generateEnemyPokemon()
	{
		int index = m_generator.Next(0, Pokemon.Names.Length);
		string name = Pokemon.Names[index];
		
		RandomAttackStrategy enemy_strategy = new RandomAttackStrategy();
		
		Character enemy = new Character(name, 70, enemy_strategy);
		Ability ability0 = new Ability("Enemy Ability 0", "Normal", 20, 20);
		Ability ability1 = new Ability("Enemy Ability 1", "Normal", 20, 20);
		Ability ability2 = new Ability("Enemy Ability 2", "Normal", 20, 20);
		Ability ability3 = new Ability("Enemy Ability 3", "Normal", 20, 20);
		
		enemy.addAbility(ability0);
		enemy.addAbility(ability1);
		enemy.addAbility(ability2);
		enemy.addAbility(ability3);
		
		return enemy;
	}
	
	private void resolveTurn(AbilityUse turnInfo)
	{
		m_textHandler(turnInfo.actor.Name + " used ability " + turnInfo.ability.Name);
	}
}
