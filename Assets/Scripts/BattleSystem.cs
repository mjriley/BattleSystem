using System;
using System.Linq;
using System.Collections.Generic;


public class BattleSystem
{
	private Player m_player;
	public Player UserPlayer { get { return m_player; } }
	
	private Player m_enemy;
	public Player EnemyPlayer { get { return m_enemy; } }
	
	public List<Character> Enemies { get { return m_enemy.Pokemon; } }
	
	Queue<string> m_messages = new Queue<string>();
	Queue<ITurnAction> m_pendingAbilities = new Queue<ITurnAction>();
	Queue<ITurnAction> m_nextTurnActions = new Queue<ITurnAction>();
	
	Random m_generator = null;
	
	bool m_needPlayerTurn = true;
	bool m_needEnemyTurn = true;
	
	bool m_needPlayerPokemon = false;
	bool m_needEnemyPokemon = false;
	
	public delegate bool IsMessageProcessed();
	public delegate int PlayerAbilityChoiceHandler(List<Ability> abilities);
	public delegate void TextAlertHandler(string message);
	
	private IsMessageProcessed m_messageProcessed;
	private TextAlertHandler m_textHandler;
	
	private UserInputStrategy.ReceiveConditions m_abilityDisplayHandler;
	
	public enum State
	{
		NewEncounter,
		BattleIntro,
		InBattle
	};
	
	public enum InternalState
	{
		NewEncounter,
		GetAbilities,
		WaitingOnPlayerChoice,
		WaitingOnEnemyChoice,
		ExecutingAbilities,
		ProcessTurnEnd,
		WaitingOnNextPokemonSelection,
		NextText,
		Idle
	};
	
	private State m_externalState = State.NewEncounter;
	public State BattleState { get { return m_externalState; } }
	
	public enum MessageState
	{
		Ready,
		Waiting
	};
	
	private MessageState m_currentMessageState = MessageState.Ready;
	
	private InternalState m_currentState = InternalState.NewEncounter;
	public InternalState IntState { get { return m_currentState; } }
	
	private InternalState m_nextState = InternalState.NewEncounter;
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
	
	private void StateChange(State newState)
	{
		if (m_externalState != newState)
		{
			m_externalState = newState;
		}
	}
	
	private bool IsTeamEliminated(List<Character> team)
	{
		foreach (Character pokemon in team)
		{
			if (!pokemon.isDead())
			{
				return false;
			}
		}
		
		return true;
	}
	
	private bool DidPlayerWin()
	{
		return IsTeamEliminated(m_enemy.Pokemon);
	}
	
	private bool DidEnemyWin()
	{
		return IsTeamEliminated(m_player.Pokemon);
	}
	
	private void Restart()
	{
		m_currentState = InternalState.Idle;
		m_nextState = InternalState.NewEncounter;
		m_pendingAbilities.Clear();
	}
	
	public void CreatePlayerPokemon(UserInputStrategy.ReceiveConditions abilityDisplayHandler, 
		UserInputStrategy.GetUserInput abilityChoiceHandler, 
		InputPokemonStrategy.GetNextPokemon nextPokemonHandler)
	{
		InputPokemonStrategy nextPokemonStrategy = new InputPokemonStrategy(nextPokemonHandler);
		m_player = new Player("Human", nextPokemonStrategy);
		
		UserInputStrategy userStrategy = new UserInputStrategy(abilityDisplayHandler, abilityChoiceHandler);
		
		Character pikachu = new Character("Pikachu", Character.Sex.Male, 70, BattleType.Electric, userStrategy);
		pikachu.addAbility(new FlyAbility("Fly", BattleType.Flying, 50, 95, 20));
		pikachu.addAbility(new Ability("Bubble", BattleType.Water, 20, 100, 30));
		pikachu.addAbility(new Ability("Ember", BattleType.Fire, 10, 100, 25, new AbilityEffect(AbilityEffect.EffectType.Burn, 0.10f)));
		pikachu.addAbility(new Ability("Vine Whip", BattleType.Grass, 35, 100, 10));
		
		Character chespin = new Character("Chespin", Character.Sex.Male, 70, BattleType.Grass, userStrategy);
		chespin.addAbility(new Ability("Ability 0", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 1", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 2", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 3", BattleType.Grass, 20, 100, 20));
		
		Character squirtle = new Character("Squirtle", Character.Sex.Male, 70, BattleType.Water, userStrategy);
		squirtle.addAbility(new Ability("Ability 0", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 1", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 2", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 3", BattleType.Water, 20, 100, 20));
		
		Character charmander = new Character("Charmander", Character.Sex.Male, 70, BattleType.Fire, userStrategy);
		charmander.addAbility(new Ability("Ability 0", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 1", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 2", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 3", BattleType.Fire, 20, 100, 20));
		
		m_player.AddPokemon(pikachu);
		m_player.AddPokemon(chespin);
		m_player.AddPokemon(squirtle);
		m_player.AddPokemon(charmander);
	}
	
	public Player generateEnemy()
	{
		Player player = new Player("Enemy", new SequentialPokemonStrategy());
		
		for (int i = 0; i < 3; ++i)
		{
			int index = m_generator.Next(0, Pokemon.Names.Length);
			string name = Pokemon.Names[index];
			
			RandomAttackStrategy enemy_strategy = new RandomAttackStrategy();
			
			Character enemy = new Character(name, Character.Sex.Female, 70, BattleType.Water, enemy_strategy);
			Ability ability0 = new Ability("Enemy Ability 0", BattleType.Normal, 20, 100, 20);
			Ability ability1 = new Ability("Enemy Ability 1", BattleType.Normal, 20, 100, 20);
			Ability ability2 = new Ability("Enemy Ability 2", BattleType.Normal, 20, 100, 20);
			Ability ability3 = new Ability("Enemy Ability 3", BattleType.Normal, 20, 100, 20);
			
			enemy.addAbility(ability0);
			enemy.addAbility(ability1);
			enemy.addAbility(ability2);
			enemy.addAbility(ability3);
			
			player.AddPokemon(enemy);
		}
		
		return player;
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
		if (m_currentState == InternalState.NewEncounter)
		{
			m_enemy = generateEnemy();
			m_messages.Enqueue("A Wild " + m_enemy.Name + " appeared!");
			m_messages.Enqueue("Go! " + m_player.ActivePokemon.Name + "!");
			m_currentState = InternalState.Idle;
			m_nextState = InternalState.GetAbilities;
			
			StateChange(State.BattleIntro);
		}
		else if (m_currentState == InternalState.GetAbilities)
		{
			// roll over the previous queue to the current queue
			m_pendingAbilities = m_nextTurnActions;
			m_nextTurnActions = new Queue<ITurnAction>();
			
			m_needPlayerTurn = !m_pendingAbilities.Any(p => p.Subject.Equals(m_player.ActivePokemon) );
			m_needEnemyTurn = !m_pendingAbilities.Any(p => p.Subject.Equals(m_enemy.ActivePokemon) );
			
			// only ask the player for an action if he doesn't already have one queued up
			if (m_needPlayerTurn)
			{
				m_messages.Enqueue("What will " + m_player.ActivePokemon.Name + " do?");
				// Call a callback that will tell us if we're still waiting for if the user has decided on input
				m_player.ActivePokemon.UpdateBattleConditions(m_enemy);
			}
			
			if (m_needEnemyTurn)
			{
				m_enemy.ActivePokemon.UpdateBattleConditions(m_player);
			}
			
			m_currentState = InternalState.Idle;
			m_nextState = InternalState.WaitingOnPlayerChoice;
			
			StateChange(State.InBattle);
		}
		else if (m_currentState == InternalState.WaitingOnPlayerChoice)
		{
			if (m_needPlayerTurn)
			{
				ITurnAction playerTurn = m_player.ActivePokemon.getTurn();
				
				if (playerTurn != null)
				{
					m_pendingAbilities.Enqueue(playerTurn);
					
					m_currentState = InternalState.Idle;
					m_nextState = InternalState.WaitingOnEnemyChoice;
				}
			}
			else
			{
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.WaitingOnEnemyChoice;
			}
		}
		else if (m_currentState == InternalState.WaitingOnEnemyChoice)
		{
			if (m_needEnemyTurn)
			{
				ITurnAction enemyTurn = m_enemy.ActivePokemon.getTurn();
				m_pendingAbilities.Enqueue(enemyTurn);
				
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.ExecutingAbilities;
			}
			else
			{
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.ExecutingAbilities;
			}
		}
		else if (m_currentState == InternalState.ExecutingAbilities)
		{
			if (m_pendingAbilities.Count > 0)
			{
				ITurnAction turnAction = m_pendingAbilities.Peek();
				ActionStatus status = turnAction.Execute(); 
				
				if (status.isComplete || status.turnComplete)
				{
					m_pendingAbilities.Dequeue();
					List<string> endTurnMessages = turnAction.Subject.CompleteTurn();
					status.messages.AddRange(endTurnMessages);
					
					if (!status.isComplete)
					{
						m_nextTurnActions.Enqueue(turnAction);
					}
				}
				
				if (status.messages != null)
				{
					foreach (string message in status.messages)
					{
						m_messages.Enqueue(message);
					}
					
				}
				
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.ExecutingAbilities;
			}
			else
			{
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.ProcessTurnEnd;
			}
		}
		else if (m_currentState == InternalState.WaitingOnNextPokemonSelection)
		{
			if (m_needPlayerPokemon)
			{
				UnityEngine.Debug.Log("Need Player Pokemon");
				int nextPokemon = m_player.GetNextPokemon(m_enemy);
				// Go for it! <X>!
				if (nextPokemon != -1)
				{
					m_player.setActivePokemon(nextPokemon);
					m_messages.Enqueue("Go for it! " + m_player.ActivePokemon.Name + "!");
					m_needPlayerPokemon = false;
				}
			}
			
			if (m_needEnemyPokemon)
			{
				UnityEngine.Debug.Log("Need Enemy Pokemon");
				int nextPokemon = m_enemy.GetNextPokemon(m_player);
				if (nextPokemon != -1)
				{
					m_enemy.setActivePokemon(nextPokemon);
					m_messages.Enqueue(m_enemy.Name + " is about to send in " + m_enemy.ActivePokemon.Name + ". Will you switch your Pokemon?");
					m_needEnemyPokemon = false;
				}
			}
			
			if (!m_needPlayerPokemon && !m_needEnemyPokemon)
			{
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.GetAbilities;
			}
		}
		else if (m_currentState == InternalState.ProcessTurnEnd)
		{
			bool checkPlayerVictory = false;
			bool checkEnemyVictory = false;
			
			if (m_player.ActivePokemon.isDead())
			{
				m_messages.Enqueue(m_player.ActivePokemon.Name + " fainted!");
				
				checkEnemyVictory = true;
				
				m_messages.Enqueue("Battle using which Pokemon?");
			}
			
			if (m_enemy.ActivePokemon.isDead())
			{
				m_messages.Enqueue(m_enemy.ActivePokemon.Name + " fainted!");
				
				checkPlayerVictory = true;
			}
			
			if (checkEnemyVictory)
			{
				if (DidEnemyWin())
				{
					m_messages.Enqueue("Enemy Won!");
					Restart();
				}
				else
				{
					// get next enemy pokemon
					m_needPlayerPokemon = true;
					m_currentState = InternalState.Idle;
					m_nextState = InternalState.WaitingOnNextPokemonSelection;
				}
			}
			else if (checkPlayerVictory)
			{
				if (DidPlayerWin())
				{
					m_messages.Enqueue("Player Won!");
					Restart();
				}
				else
				{
					m_needEnemyPokemon = true;
					m_currentState = InternalState.Idle;
					m_nextState = InternalState.WaitingOnNextPokemonSelection;
				}
			}
			else
			{
				m_currentState = InternalState.Idle;
				m_nextState = InternalState.GetAbilities;
			}
		}
	}
	
}
