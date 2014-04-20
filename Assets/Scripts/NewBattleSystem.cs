using System;
using System.Collections.Generic;

public class NewBattleSystem
{
	public enum State
	{
		CombatIntro,
		CombatPrompt,
		FightPrompt,
		ItemPrompt,
		PokemonPrompt,
		ProcessTurn,
		EndTurn
	};
	
	public enum CombatSelection : int
	{
		Fight,
		Item,
		Run,
		Pokemon
	};
	
	private Queue<EventArgs> m_pendingEvents = new Queue<EventArgs>();
	
	public delegate void BattleEventHandler(object sender, EventArgs e);
	
	public event BattleEventHandler BattleProgress;
	
	private State m_currentState = State.CombatIntro;
	private State m_nextState = State.CombatIntro;
	public State CurrentState { get { return m_currentState; } }
	
	private Player m_userPlayer;
	public Player UserPlayer { get { return m_userPlayer; } }
	
	private Player m_enemyPlayer; 
	public Player EnemyPlayer { get { return m_enemyPlayer; } }
	
	private int m_userChoice = -1;
	
	private Queue<ITurnAction> m_actionQueue = new Queue<ITurnAction>();
	
	private Random m_generator = new Random();
	private RandomAttackStrategy m_enemyStrategy;
	
	private bool m_waitingForInput = false;
	
	public NewBattleSystem()
	{
		m_enemyStrategy = new RandomAttackStrategy(m_generator);
	}
	
	public void ProcessUserChoice(int choice)
	{
		m_userChoice = choice;
	}
	
	bool ValidateInput()
	{
		if (m_userChoice != -1)
		{
			m_waitingForInput = false;
			return true;
		}
		
		return false;
	}
	
	private void AddStatusMessage(string status)
	{
		m_pendingEvents.Enqueue(new StatusUpdateEventArgs(status));
	}
	
	public void Update()
	{
		if (m_pendingEvents.Count != 0)
		{
			EventArgs e = m_pendingEvents.Dequeue();
			OnBattleProgress(e);
			return;
		}
		
		// all status events must be processed before we can handle state logic
		m_currentState = m_nextState;
		
		switch (m_currentState)
		{
			case State.CombatIntro:
			{
				m_enemyPlayer = generateEnemy();
				AddStatusMessage("A Wild " + m_enemyPlayer.ActivePokemon.Species.ToString() + " appeared!");
				m_pendingEvents.Enqueue(new DeployEventArgs(false));
				
				AddStatusMessage("Go! " + m_userPlayer.ActivePokemon.Name + "!");
				m_pendingEvents.Enqueue(new DeployEventArgs(true));
				m_nextState = State.CombatPrompt;
				break;
			}
			case State.CombatPrompt:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					
					// update the players with the battle conditions
					m_userPlayer.UpdateConditions(m_enemyPlayer);
					m_enemyPlayer.UpdateConditions(m_userPlayer);
					
					AddStatusMessage("What will " + m_userPlayer.ActivePokemon.Name + " do?");
					m_waitingForInput = true;
				}
				else if (ValidateInput())
				{
					switch (m_userChoice)
					{
						case (int)CombatSelection.Fight:
						{
							m_nextState = State.FightPrompt;
							break;
						}
						case (int)CombatSelection.Item:
						{
							m_nextState = State.ItemPrompt;
							break;
						}
						case (int)CombatSelection.Run:
						{
							// do nothing
							break;
						}
						case (int)CombatSelection.Pokemon:
						{
							m_nextState = State.PokemonPrompt;
							break;
						}
					}
				}
				break;
			}
			case State.FightPrompt:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					AddStatusMessage("Chose to Fight!");
					m_waitingForInput = true;
				}
				else if (ValidateInput())
				{
					if (m_userChoice == -2)
					{
						m_nextState = State.CombatPrompt;
					}
					else
					{
						//Ability userAbility = m_userPlayer.ActivePokemon.getAbilities()[m_userChoice];
						ITurnAction userAction = m_userPlayer.GetTurnAction();
						m_actionQueue.Enqueue(userAction);
						
						ITurnAction enemyAction = m_enemyPlayer.GetTurnAction();
						UnityEngine.Debug.Log("Enemy action is: " + enemyAction.Subject.Name);
						m_actionQueue.Enqueue(enemyAction);
						//AddStatusMessage("User submitted ability " + m_userPlayer.ActivePokemon.getAbilities()[m_userChoice].Name);
						m_nextState = State.ProcessTurn;
					}
				}
				break;
			}
			case State.ItemPrompt:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					AddStatusMessage("Chose to use an item!");
					m_waitingForInput = true;
				}
				else if (ValidateInput())
				{
					if (m_userChoice == -2)
					{
						m_nextState = State.CombatPrompt;
					}
				}
				break;
			}
			case State.PokemonPrompt:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					AddStatusMessage("Chose to switch pokemon!");
					m_waitingForInput = true;
				}
				else if (ValidateInput())
				{
					if (m_userChoice == -2)
					{
						m_nextState = State.CombatPrompt;
					}
				}
				break;
			}
			case State.ProcessTurn:
			{
				UnityEngine.Debug.Log("Processing Turn");
				if (m_actionQueue.Count > 0)
				{
					ITurnAction action = m_actionQueue.Peek();
					ActionStatus status = action.Execute();
					UnityEngine.Debug.Log("Processing " + action.Subject.Name);
					
					if (status.turnComplete)
					{
						foreach (string message in status.messages)
						{
							AddStatusMessage(message);
						}
					}
					
					if (status.isComplete)
					{
						m_actionQueue.Dequeue();
					}
				}
				else
				{
					m_nextState = State.EndTurn;
				}
				
				//AddStatusMessage("Processing Turn...");
				//m_nextState = State.EndTurn;
				break;
			}
			case State.EndTurn:
			{
				AddStatusMessage("Ending turn");
				m_nextState = State.CombatPrompt;
				break;
			}
		}
	}
	
	private void OnBattleProgress(EventArgs e)
	{
		if (BattleProgress != null)
		{
			BattleProgress(this, e);
		}
	}
	
	
	public void CreatePlayerPokemon()
	{
		m_userPlayer = new Player("Human", null);
		
		UserInputStrategy input = new UserInputStrategy(null, this.GetUserAbility);
		
		Character pikachu = new Character("Pikachu", Pokemon.Species.Pikachu, Character.Sex.Male, 70, 35, BattleType.Electric, input);
		pikachu.addAbility(new FlyAbility("Fly", BattleType.Flying, 50, 95, 20));
		pikachu.addAbility(new Ability("Bubble", BattleType.Water, 20, 100, 30));
		pikachu.addAbility(new Ability("Ember", BattleType.Fire, 10, 100, 25, new AbilityEffect(AbilityEffect.EffectType.Burn, 0.10f)));
		pikachu.addAbility(new Ability("Vine Whip", BattleType.Grass, 35, 100, 10));
		
		Character chespin = new Character("Chespin", Pokemon.Species.Chespin, Character.Sex.Female, 70, 1, BattleType.Grass, input);
		chespin.addAbility(new Ability("Ability 0", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 1", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 2", BattleType.Grass, 20, 100, 20));
		chespin.addAbility(new Ability("Ability 3", BattleType.Grass, 20, 100, 20));
		
		Character squirtle = new Character("Squirtle", Pokemon.Species.Squirtle, Character.Sex.Female, 70, 20, BattleType.Water, input);
		squirtle.addAbility(new Ability("Ability 0", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 1", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 2", BattleType.Water, 20, 100, 20));
		squirtle.addAbility(new Ability("Ability 3", BattleType.Water, 20, 100, 20));
		
		Character charmander = new Character("Charmander", Pokemon.Species.Charmander, Character.Sex.Male, 70, 100, BattleType.Fire, input);
		charmander.addAbility(new Ability("Ability 0", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 1", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 2", BattleType.Fire, 20, 100, 20));
		charmander.addAbility(new Ability("Ability 3", BattleType.Fire, 20, 100, 20));
		
		Character magikarp = new Character("Magikarp", Pokemon.Species.Magikarp, Character.Sex.Male, 70, 100, BattleType.Normal, input);
		magikarp.addAbility(new Ability("Ability 0", BattleType.Normal, 20, 100, 20));
		magikarp.addAbility(new Ability("Ability 1", BattleType.Normal, 20, 100, 20));
		magikarp.addAbility(new Ability("Ability 2", BattleType.Normal, 20, 100, 20));
		magikarp.addAbility(new Ability("Ability 3", BattleType.Normal, 20, 100, 20));
		
		Character bulbasaur = new Character("Bulbasaur", Pokemon.Species.Bulbasaur, Character.Sex.Female, 70, 10, BattleType.Poison, input);
		bulbasaur.addAbility(new Ability("Ability 0", BattleType.Poison, 20, 100, 20));
		bulbasaur.addAbility(new Ability("Ability 1", BattleType.Poison, 20, 100, 20));
		bulbasaur.addAbility(new Ability("Ability 2", BattleType.Poison, 20, 100, 20));
		bulbasaur.addAbility(new Ability("Ability 3", BattleType.Poison, 20, 100, 20));
		
		m_userPlayer.AddPokemon(pikachu);
		m_userPlayer.AddPokemon(chespin);
		m_userPlayer.AddPokemon(squirtle);
		m_userPlayer.AddPokemon(charmander);
		m_userPlayer.AddPokemon(magikarp);
		m_userPlayer.AddPokemon(bulbasaur);
	}
	
	public Player generateEnemy()
	{
		Player player = new Player("Enemy", new SequentialPokemonStrategy());
		
		string[] pokemonNames = Enum.GetNames(typeof(Pokemon.Species));
		for (int i = 0; i < 3; ++i)
		{
			int index = m_generator.Next(0, pokemonNames.Length);
			string name = pokemonNames[index];
			
			//RandomAttackStrategy enemy_strategy = new RandomAttackStrategy();
			
			Pokemon.Species species = (Pokemon.Species)Enum.Parse(typeof(Pokemon.Species), name);
			
			Character enemy = new Character(name, species, Character.Sex.Female, 70, 50, BattleType.Water, m_enemyStrategy);
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
	
	private ITurnAction GetUserAbility()
	{
		if (m_userChoice == -1)
		{
			return null;
		}
		
		Ability selectedAbility = m_userPlayer.ActivePokemon.getAbilities()[m_userChoice];
		
		return new AbilityUse(m_userPlayer.ActivePokemon, m_enemyPlayer, selectedAbility);
	}
}

public class StatusUpdateEventArgs : EventArgs
{
	public string Status { get; set; }
	
	public StatusUpdateEventArgs(string status)
	{
		Status = status;
	}
}

public class DeployEventArgs : EventArgs
{
	public bool Friendly { get; set; }	
	
	public DeployEventArgs(bool friendly)
	{
		Friendly = friendly;
	}
}
