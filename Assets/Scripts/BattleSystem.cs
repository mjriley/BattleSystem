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
	
	private enum InternalState
	{
		NewEncounter,
		GetAbilities,
		WaitingOnAbilities,
		ExecutingAbilities,
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
	
	public void CreatePlayerPokemon(UserInputStrategy.ReceiveConditions abilityDisplayHandler, UserInputStrategy.GetUserInput abilityChoiceHandler)
	{
		m_player = new Player("Human");
		
		UserInputStrategy userStrategy = new UserInputStrategy(abilityDisplayHandler, abilityChoiceHandler);
		
		Character pikachu = new Character("Pikachu", Character.Sex.Male, 70, BattleType.Electric, userStrategy);
		pikachu.addAbility(new FlyAbility("Fly", BattleType.Flying, 50, 95, 20));
		pikachu.addAbility(new Ability("Bubble", BattleType.Water, 20, 100, 30));
		pikachu.addAbility(new Ability("Ember", BattleType.Fire, 40, 100, 25));
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
		Player player = new Player("Enemy");
		
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
			
			// only ask the player for an action if he doesn't already have one queued up
			m_messages.Enqueue("What will " + m_player.ActivePokemon.Name + " do?");
			// Call a callback that will tell us if we're still waiting for if the user has decided on input
			m_player.ActivePokemon.UpdateBattleConditions(m_enemy);
			
			m_enemy.ActivePokemon.UpdateBattleConditions(m_player);
			
			m_currentState = InternalState.Idle;
			m_nextState = InternalState.WaitingOnAbilities;
			
			StateChange(State.InBattle);
		}
		else if (m_currentState == InternalState.WaitingOnAbilities)
		{
			ITurnAction playerTurn = m_player.ActivePokemon.getTurn();
			
			// if turnInfo is null, we're still waiting
			if (playerTurn != null)
			{
				// this would have to be moved elsewhere if the enemy was a real player;
				// all abilities could/should be fetched at the same time/asynchronously
				ITurnAction enemyTurn = m_enemy.ActivePokemon.getTurn();
				
				m_pendingAbilities.Enqueue(playerTurn);
				m_pendingAbilities.Enqueue(enemyTurn);
				
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
					
					if (!status.isComplete)
					{
					}
				}
				
				if (status.messages != null)
				{
					foreach (string message in status.messages)
					{
						m_messages.Enqueue(message);
					}
				}
				
				bool victoryCheck = false;
				
				// figure out where to put this logic
				if (m_player.ActivePokemon.isDead())
				{
					m_messages.Enqueue(m_player.ActivePokemon.Name + " fainted!");
					victoryCheck = true;
				}
				
				if (m_enemy.ActivePokemon.isDead())
				{
					m_messages.Enqueue(m_enemy.ActivePokemon.Name + " fainted!");
					victoryCheck = true;
				}

				
				if (victoryCheck)
				{
					if (DidPlayerWin())
					{
						m_messages.Enqueue("Player Won!");
						Restart();
					}
					else if (DidEnemyWin())
					{
						m_messages.Enqueue("Enemy Won!");
						Restart();
					}
					else
					{
						m_currentState = InternalState.Idle;
						m_nextState = InternalState.ExecutingAbilities;
					}
				}
				else
				{
					m_currentState = InternalState.Idle;
					m_nextState = InternalState.ExecutingAbilities;
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
