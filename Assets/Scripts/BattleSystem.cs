using System;
using System.Collections.Generic;

public class BattleSystem
{
	List<Character> m_playerPokemon = new List<Character>();
	Character m_activePokemon = null;
	List<Character> m_enemies = new List<Character>();
	Character m_enemy = null;
	
	Queue<string> m_messages = new Queue<string>();
	
	Random m_generator = null;
	
	
	public delegate bool IsMessageProcessed();
	public delegate int PlayerAbilityChoiceHandler(List<Ability> abilities);
	public delegate void TextAlertHandler(string message);
	
	private IsMessageProcessed m_messageProcessed;
	private TextAlertHandler m_textHandler;
	
	private UserInputStrategy.ReceiveConditions m_abilityDisplayHandler;
	
	public enum State
	{
		NewEncounter,
		GetAbility,
		WaitingOnAbility,
		NextText,
		Idle
	};
	
	public enum MessageState
	{
		Ready,
		Waiting
	};
	
	private MessageState m_currentMessageState = MessageState.Ready;
	
	private State m_currentState = State.NewEncounter;
	private State m_nextState = State.NewEncounter;
	private string m_status = "";
	
	public BattleSystem(TextAlertHandler textHandler, IsMessageProcessed messageChecker, Random generator = null)
	{
		m_messageProcessed = messageChecker;
		m_textHandler = textHandler;
		
		m_generator = generator;
		if (m_generator == null)
		{
			m_generator = new Random();
		}
	}
	
	
	public void CreatePlayerPokemon(UserInputStrategy.ReceiveConditions abilityDisplayHandler, UserInputStrategy.GetUserInput abilityChoiceHandler)
	{
		UserInputStrategy userStrategy = new UserInputStrategy(abilityDisplayHandler, abilityChoiceHandler);
		//RandomAttackStrategy userStrategy = new RandomAttackStrategy();
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
	
	public void Update()
	{
		// handle messages
		if (m_currentMessageState == MessageState.Ready)
		{
			if (m_messages.Count > 0)
			{
				m_status = m_messages.Dequeue();
				m_currentMessageState = MessageState.Waiting;
				m_textHandler(m_status);
			}
			else
			{
				m_currentState = m_nextState;
			}
		}
		else if (m_currentMessageState == MessageState.Waiting)
		{
			// waiting for the message to be acknowledged
			if (m_messageProcessed())
			{
				m_currentMessageState = MessageState.Ready;
			}
		}
		
		// handle logic
		if (m_currentState == State.NewEncounter)
		{
			m_enemy = generateEnemyPokemon();
			m_enemies.Add(m_enemy);
			m_messages.Enqueue("A Wild " + m_enemy.Name + " appeared!");
			m_messages.Enqueue("Go! " + m_activePokemon.Name + "!");
			//m_currentState = State.GetAbility;
			m_currentState = State.Idle;
			m_nextState = State.GetAbility;
		}
		else if (m_currentState == State.GetAbility)
		{
			m_messages.Enqueue("What will " + m_activePokemon.Name + " do?");
			// Call a callback that will tell us if we're still waiting for if the user has decided on input
			m_activePokemon.UpdateBattleConditions(m_enemies);
			
			//m_abilityHandler(m_activePokemon.getAbilities());
			
			//m_currentState = State.WaitingOnAbility;
			m_currentState = State.Idle;
			m_nextState = State.WaitingOnAbility;
		}
		else if (m_currentState == State.WaitingOnAbility)
		{
			AbilityUse turnInfo = m_activePokemon.getTurn();
			
			// if turnInfo is null, we're still waiting
			if (turnInfo != null)
			{
				m_messages.Enqueue(turnInfo.actor.Name + " used " + turnInfo.ability.Name + "!");
				
				m_enemy.UpdateBattleConditions(m_playerPokemon);
				AbilityUse enemyTurn = m_enemy.getTurn();
				
				m_messages.Enqueue(enemyTurn.actor.Name + " used " + enemyTurn.ability.Name + "!");
				
				//m_currentState = State.GetAbility;
				m_currentState = State.Idle;
				m_nextState = State.GetAbility;
			}
		}
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
