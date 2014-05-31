using System;
using System.Linq;
using System.Collections.Generic;

using Abilities;

public class NewBattleSystem
{
	public enum State
	{
		Start,
		Splash,
		CombatIntro,
		BeginTurn,
		CombatPrompt,
		FightPrompt,
		ItemPrompt,
		PokemonPrompt,
		EnemyAction,
		ProcessTurn,
		EndTurn,
		FriendlyPokemonDefeated,
		EnemyPokemonDefeated,
		Victory,
		ReplacePokemon,
		ReplaceEnemyPokemon
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
	public delegate void StateChangeHandler(object sender, StateChangeArgs e);
	
	public event BattleEventHandler BattleProgress;
	public event StateChangeHandler LeaveState;
	public event StateChangeHandler EnterState;
	
	private State m_currentState = State.Start;
	private State m_nextState = State.Splash;
	public State CurrentState { get { return m_currentState; } }
	
	private Player m_userPlayer;
	public Player UserPlayer { get { return m_userPlayer; } }
	
	private Player m_enemyPlayer; 
	Trainer m_enemyTrainer;
	public Player EnemyPlayer { get { return m_enemyPlayer; } }
	
	private int m_userChoice = -1;
	
	private Queue<ITurnAction> m_actionQueue = new Queue<ITurnAction>();
	private List<ITurnAction> m_turnActions = new List<ITurnAction>();
	//private Queue<ITurnAction> m_nextTurnQueue = new Queue<ITurnAction>();
	
	private Random m_generator = new Random();
	private RandomAttackStrategy m_enemyStrategy;
	
	private bool m_waitingForInput = false;
	
	bool isQueueSorted = false;
	
	int m_consecutiveVictories;
	public int Victories { get { return m_consecutiveVictories; } }
	
	public NewBattleSystem()
	{
		m_enemyStrategy = new RandomAttackStrategy(m_generator);
		
		Reset();
	}
	
	public void InitializePlayer(PokemonPrototype[] prototypes)
	{
		m_userPlayer = new Player("Human", null);
		
		UserInputStrategy input = new UserInputStrategy(null, this.GetUserAbility);
		
		foreach (PokemonPrototype prototype in prototypes)
		{
			Character pokemon = PokemonFactory.CreatePokemon(prototype.Species, prototype.Level, "", prototype.Gender, input);
			m_userPlayer.AddPokemon(pokemon);
		}
	}
	
	public void ProcessUserChoice(int choice)
	{
		m_userChoice = choice;
	}
	
	bool NeedPlayerAction(Player player)
	{
		//return !m_actionQueue.Any(p => (p.Subject == player.ActivePokemon));
		return !m_turnActions.Any(p => (p.Subject == player.ActivePokemon));
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
	
	public void Reset()
	{
		m_pendingEvents = new Queue<EventArgs>();
		m_currentState = State.Start;
		m_nextState = State.Splash;
		
		m_userChoice = -1;
		m_waitingForInput = false;
		
		m_enemyTrainer = null;
		
		m_consecutiveVictories = 0;
	}
	
	void ProcessVictory()
	{
		m_consecutiveVictories += 1;
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
		ChangeState(m_nextState);
		//m_currentState = m_nextState;
		
		switch (m_currentState)
		{
			// You are challenged by Duchess Ione!
			// -> Shift out trainer portrait
			// Duchess Ione sent out <X>!
			// Go! <Y>!
			case State.Splash:
			{
				m_userPlayer.RestorePokemon();
				m_enemyPlayer = generateEnemy();
				m_pendingEvents.Enqueue(new NewEncounterEventArgs(m_enemyTrainer));
				string fullName = m_enemyTrainer.Title + " " + m_enemyTrainer.Name;
				m_pendingEvents.Enqueue(new StatusUpdateEventArgs("You have been challenged by " + fullName + "!", false));
				m_nextState = State.CombatIntro;
				break;
			}
			case State.CombatIntro:
			{
				AddStatusMessage(m_enemyPlayer.Name + " sent out " + m_enemyPlayer.ActivePokemon.Species.ToString() + "!");
				m_pendingEvents.Enqueue(new DeployEventArgs(m_enemyPlayer.ActivePokemon));
				
				AddStatusMessage("Go! " + m_userPlayer.ActivePokemon.Name + "!");
				m_pendingEvents.Enqueue(new DeployEventArgs(m_userPlayer.ActivePokemon));
				m_nextState = State.BeginTurn;
				break;
			}
			case State.BeginTurn:
			{
				isQueueSorted = false;
				
				if (NeedPlayerAction(m_userPlayer))
				{
					m_nextState = State.CombatPrompt;
				}
				else if (NeedPlayerAction(m_enemyPlayer))
				{
					m_nextState = State.EnemyAction;
				}
				else
				{
					m_nextState = State.ProcessTurn;
				}
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
						//m_actionQueue.Enqueue(m_userPlayer.GetTurnAction());
						m_turnActions.Add(m_userPlayer.GetTurnAction());
						
						m_nextState = State.EnemyAction;
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
					AddStatusMessage("Choose a pokemon");
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
						// Assemble a recall action
						SwapAbility swap = new SwapAbility(m_userPlayer, m_userChoice);
						m_actionQueue.Enqueue(swap);
						m_nextState = State.EnemyAction;
					}
				}
				break;
			}
			case State.EnemyAction:
			{
				if (NeedPlayerAction(m_enemyPlayer))
				{
					ITurnAction enemyAction = m_enemyPlayer.GetTurnAction();
					//m_actionQueue.Enqueue(enemyAction);
					m_turnActions.Add(enemyAction);
				}
				
				m_nextState = State.ProcessTurn;
				break;
			}
			case State.ProcessTurn:
			{
				if (!isQueueSorted)
				{
					m_turnActions.Sort(delegate(ITurnAction x, ITurnAction y) { return ActionSort.ByPriority(y, x); });
					
					foreach (ITurnAction action in m_turnActions)
					{
						m_actionQueue.Enqueue(action);
					}
					
					m_turnActions = new List<ITurnAction>();
					isQueueSorted = true;
				}
				
				if (m_actionQueue.Count > 0)
				{
					// Sort the action queue
					ITurnAction action = m_actionQueue.Peek();
					ActionStatus status = action.Execute();
					
					foreach (EventArgs eventArgs in status.events)
					{
						m_pendingEvents.Enqueue(eventArgs);
					}
					
					if (status.turnComplete)
					{
						m_actionQueue.Dequeue();
						
						if (!status.isComplete)
						{
							//m_nextTurnQueue.Enqueue(action);
							m_turnActions.Add(action);
						}
					}
				}
				else
				{
					m_nextState = State.EndTurn;
				}
				break;
			}
			case State.EndTurn:
			{
				// TODO: Likely need to move this to its own state, and make damage events delayed
				ActionStatus status = new ActionStatus();
				m_userPlayer.ActivePokemon.CompleteTurn(m_enemyPlayer.ActivePokemon, ref status);
					
				m_enemyPlayer.ActivePokemon.CompleteTurn(m_userPlayer.ActivePokemon, ref status);
				
				foreach (EventArgs eventArgs in status.events)
				{
					m_pendingEvents.Enqueue(eventArgs);
				}
				
				if (m_userPlayer.ActivePokemon.isDead())
				{
					if (m_userPlayer.IsDefeated())
					{
						AddStatusMessage("You've lost!");
						m_nextState = State.Splash;
					}
					else
					{
						m_nextState = State.FriendlyPokemonDefeated;
					}
				}
				else if (m_enemyPlayer.ActivePokemon.isDead())
				{
					if (m_enemyPlayer.IsDefeated())
					{
						m_nextState = State.Victory;
					}
					else
					{
						m_nextState = State.EnemyPokemonDefeated;
					}
				}
				else
				{
					// swap queues -- make the next turn the current one
//					Queue<ITurnAction> temp = m_actionQueue;
//					m_actionQueue = m_nextTurnQueue;
//					m_nextTurnQueue = temp;
					
					m_nextState = State.BeginTurn;
				}
				break;
			}
			case State.Victory:
			{
				if (!m_waitingForInput)
				{
					ProcessVictory();
					m_userChoice = -1;
					m_waitingForInput = true;
				}
				else
				{
					if (m_userChoice != -1)
					{
						VictoryDisplay.Options selectedOption = (VictoryDisplay.Options)m_userChoice;
						if (selectedOption == VictoryDisplay.Options.Continue)
						{
							m_waitingForInput = false;
							m_nextState = State.Splash;
						}
						else if (selectedOption == VictoryDisplay.Options.Quit)
						{
							HighScores highScores = new HighScores();
							highScores.Load();
							
							if (highScores.IsHighScore(m_consecutiveVictories))
							{
								highScores.InsertScore(HighScore.CreateFromPlayer(m_userPlayer, m_consecutiveVictories));
								highScores.Save();
							}
							
							// TODO: Figure out some way to not include this in this file
							UnityEngine.Application.LoadLevel("HighScores");
						}
					}
				}
				break;
			}
			case State.FriendlyPokemonDefeated:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					AddStatusMessage("Battle using which Pokemon?");
					m_waitingForInput = true;
				}
				else
				{
					if (m_userChoice != -1)
					{
						m_waitingForInput = false;
						m_actionQueue.Enqueue(new DeployAbility(m_userPlayer, m_userChoice, true));
						m_nextState = State.ProcessTurn;
					}
				}
				break;
			}
			case State.EnemyPokemonDefeated:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					int pokemonIndex = m_enemyPlayer.GetNextPokemon(m_userPlayer);
					
					AddStatusMessage(m_enemyPlayer.Name + " is about to send in " + m_enemyPlayer.Pokemon[pokemonIndex].Name + ".\n Will you switch your Pokemon?");
					
					m_waitingForInput = true;
				}
				else
				{
					if (m_userChoice != -1)
					{
						m_waitingForInput = false;
						if (m_userChoice == 1)
						{	
							m_nextState = State.ReplacePokemon;
						}
						else
						{
							m_nextState = State.ReplaceEnemyPokemon;
						}
					}
				}
				break;
			}
			case State.ReplacePokemon:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					AddStatusMessage("Choose a Pokemon");
					m_waitingForInput = true;
				}
				else
				{
					if (m_userChoice != -1)
					{
						m_waitingForInput = false;
						if (m_userChoice != -2)
						{
							SwapAbility ability = new SwapAbility(m_userPlayer, m_userChoice);
							m_actionQueue.Enqueue(ability);
						}
						m_nextState = State.ReplaceEnemyPokemon;
					}
				}
				break;
			}
			case State.ReplaceEnemyPokemon:
			{
				int pokemonIndex = m_enemyPlayer.GetNextPokemon(m_userPlayer);
				m_actionQueue.Enqueue(new DeployAbility(m_enemyPlayer, pokemonIndex, false));
				
				m_nextState = State.ProcessTurn;
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
	
	private void ChangeState(State newState)
	{
		if (LeaveState != null)
		{
			StateChangeArgs e = new StateChangeArgs(m_currentState);
			LeaveState(this, e);
		}
		
		m_currentState = newState;
		
		if (EnterState != null)
		{
			StateChangeArgs e = new StateChangeArgs(m_currentState);
			EnterState(this, e);
		}
	}
	
	public Player generateEnemy()
	{
		m_enemyTrainer = new Trainer(TrainerDefinition.TrainerType.RandomClass, Pokemon.Gender.Random, -1);
		string trainerName = m_enemyTrainer.Title + " " + m_enemyTrainer.Name;
		
		Player player = new Player(trainerName, new SequentialPokemonStrategy());
		
		Pokemon.Species[] speciesOptions = (Pokemon.Species[])Enum.GetValues(typeof(Pokemon.Species));
		speciesOptions = speciesOptions.Where(x => x != Pokemon.Species.None).ToArray();
		//for (int i = 0; i < 3; ++i)
		for (int i = 0; i < 1; ++i)
		{
			int index = m_generator.Next(0, speciesOptions.Length);
			
			Pokemon.Species species = speciesOptions[index];
			
			Character enemy = PokemonFactory.CreatePokemon(species, 50, "", Pokemon.Gender.Random, m_enemyStrategy);
			
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
		
		//AbstractAbility selectedAbility = m_userPlayer.ActivePokemon.getAbilities()[m_userChoice];
		Ability selectedAbility = m_userPlayer.ActivePokemon.getAbilities()[m_userChoice];
		
		return new AbilityUse(m_userPlayer.ActivePokemon, m_enemyPlayer, selectedAbility);
	}
}

public class StateChangeArgs : EventArgs
{
	public NewBattleSystem.State State { get; set; }
	
	public StateChangeArgs(NewBattleSystem.State state)
	{
		this.State = state;
	}
}

public class NewEncounterEventArgs : EventArgs
{
	public Trainer Trainer { get; set; }
		
	public NewEncounterEventArgs(Trainer trainer)
	{
		Trainer = trainer;
	} 
}

public class StatusUpdateEventArgs : EventArgs
{
	public string Status { get; set; }
	public bool Expires { get; set; }
	
	public StatusUpdateEventArgs(string status, bool expires=true)
	{
		Status = status;
		Expires = expires;
	}
}

public class DeployEventArgs : EventArgs
{
	public Character Pokemon { get; set; }
	
	public DeployEventArgs(Character pokemon)
	{
		Pokemon = pokemon;
	}
}

public class WithdrawEventArgs : EventArgs
{
	public Character Pokemon { get; set; }
	
	public WithdrawEventArgs(Character pokemon)
	{
		Pokemon = pokemon;
	}
}

public class DamageEventArgs : EventArgs
{
	public int Amount { get; set; }
	public Player Player { get; set; }
	
	public DamageEventArgs(Player player, int amount)
	{
		Player = player;
		Amount = amount;
	}
}
