using System;
using System.Linq;
using System.Collections.Generic;

using Abilities;
using Pokemon;
using Items;

namespace PokeCore {

public class NewBattleSystem
{
	public enum State
	{
		Start,
		Splash,
		CombatIntro,
		BeginTurn,
		GetTurnActions,
		ProcessTurn,
		EndTurn,
		Victory,
		ReplaceDeadPokemon
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
	public delegate bool ProcessAction(ITurnAction action);
	
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
	
	private Random m_generator = new Random();
	
	private bool m_waitingForInput = false;
	
	bool isQueueSorted = false;
	bool m_needPlayerPokemon = false;
	bool m_needEnemyPokemon = false;
	
	int m_consecutiveVictories;
	public int Victories { get { return m_consecutiveVictories; } }
	
	bool m_hasCounterReplace;
	
	public NewBattleSystem(bool hasCounterReplace=true)
	{
		m_hasCounterReplace = hasCounterReplace;
		Reset();
	}
	
	public void InitializePlayer(PokemonPrototype[] prototypes, IActionRequest turnRequest, IActionRequest replaceRequest, IActionRequest counterReplaceRequest)
	{
		m_userPlayer = new Player("Human", turnRequest, replaceRequest, counterReplaceRequest);
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Potion"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Super Potion"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Ether"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Max Elixir"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Antidote"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Poke Ball"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("Master Ball"));
		m_userPlayer.Inventory.AddItem(ItemFactory.GetItem("X Attack"), 10);
		
		foreach (PokemonPrototype prototype in prototypes)
		{
			Character pokemon = PokemonFactory.CreatePokemon(prototype.Species, prototype.Level, "", prototype.Gender);
			m_userPlayer.AddPokemon(pokemon);
		}
	}
	
	public void ProcessUserChoice(int choice)
	{
		m_userChoice = choice;
	}
	
	bool ActionHandler(ITurnAction action)
	{
		List<string> messages;
		
		if (!action.Verify(out messages))
		{
			foreach (string message in messages)
			{
				m_pendingEvents.Enqueue(new StatusUpdateEventArgs(message));
			}
			return false;
		}
		
		m_turnActions.Add(action);
		
		return true;
	}
	
	bool ReplacementHandler(ITurnAction action)
	{
		if (action.Subject.Owner == m_userPlayer)
		{
			m_needPlayerPokemon = false;
			
			if (m_hasCounterReplace && !m_needEnemyPokemon)
			{
				m_needEnemyPokemon = true;
				m_enemyPlayer.GetCounterPokemon(m_userPlayer, this.CounterReplaceHandler);
			}
		}
		else if (action.Subject.Owner == m_enemyPlayer)
		{
			m_needEnemyPokemon = false;
			
			if (m_hasCounterReplace && !m_needPlayerPokemon)
			{
				string format = L18N.Get("PROMPT_COUNTER_REPLACE"); // <X> is about to send in <Y>. Will you switch your Pokémon?
				string message = String.Format(format, m_enemyPlayer.Name, action.Subject.Name);
				m_pendingEvents.Enqueue(new StatusUpdateEventArgs(message));
				m_needPlayerPokemon = true;
				m_userPlayer.GetCounterPokemon(m_enemyPlayer, this.CounterReplaceHandler);
			}
		}
		
		m_actionQueue.Enqueue(action);
		
		return true;
	}
	
	bool CounterReplaceHandler(ITurnAction action)
	{
		if (action.Subject.Owner == m_userPlayer)
		{
			m_needPlayerPokemon = false;
		}
		else if (action.Subject.Owner == m_enemyPlayer)
		{
			m_needEnemyPokemon = false;
		}
		
		m_actionQueue.Enqueue(action);
		
		return true;
	}
	
	bool NeedPlayerAction(Player player)
	{
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
				string format = L18N.Get("MSG_CHALLENGE"); // You have been challenged by <X>!
				string message = String.Format(format, fullName);
				m_pendingEvents.Enqueue(new StatusUpdateEventArgs(message, false));
				m_nextState = State.CombatIntro;
				break;
			}
			case State.CombatIntro:
			{
				string deployFormat = L18N.Get("MSG_DEPLOY"); // <X> sent out <Y>!
				string deployMessage = String.Format(deployFormat, m_enemyPlayer.Name, m_enemyPlayer.ActivePokemon.Species.ToString());
				AddStatusMessage(deployMessage);
				m_pendingEvents.Enqueue(new DeployEventArgs(m_enemyPlayer.ActivePokemon));
				
				string goFormat = L18N.Get("MSG_GO"); // Go! <X>!
				string goMessage = String.Format(goFormat, m_userPlayer.ActivePokemon.Name);
				AddStatusMessage(goMessage);
				m_pendingEvents.Enqueue(new DeployEventArgs(m_userPlayer.ActivePokemon));
				m_nextState = State.BeginTurn;
				break;
			}
			case State.BeginTurn:
			{
				isQueueSorted = false;
				
				if (NeedPlayerAction(m_userPlayer) || NeedPlayerAction(m_enemyPlayer))
				{
					m_nextState = State.GetTurnActions;
				}
				else
				{
					m_nextState = State.ProcessTurn;
				}
				break;
			}
			case State.GetTurnActions:
			{
				if (!m_waitingForInput)
				{
					m_userChoice = -1;
					
					if (NeedPlayerAction(m_userPlayer))
					{
						m_userPlayer.GetTurnAction(m_enemyPlayer, this.ActionHandler);
						string format = L18N.Get("PROMPT_TURN"); // What will <X> do?
						string message = String.Format(format, m_userPlayer.ActivePokemon.Name);
						AddStatusMessage(message);
					}
					
					if (NeedPlayerAction(m_enemyPlayer))
					{
						m_enemyPlayer.GetTurnAction(m_userPlayer, this.ActionHandler);
					}
					
					m_waitingForInput = true;
				}
				else
				{
					if (!NeedPlayerAction(m_userPlayer) && (!NeedPlayerAction(m_enemyPlayer)))
					{
						m_waitingForInput = false;
						m_nextState = State.ProcessTurn;
					}
				}
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
				
				if (m_userPlayer.ActivePokemon.isDead() || m_enemyPlayer.ActivePokemon.isDead())
				{
					m_nextState = State.ReplaceDeadPokemon;
				}
				else
				{
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
			case State.ReplaceDeadPokemon:
			{
				if (!m_waitingForInput)
				{
					if (m_userPlayer.ActivePokemon.isDead())
					{
						if (m_userPlayer.IsDefeated())
						{
							AddStatusMessage(L18N.Get("MSG_LOSE"));
							m_nextState = State.Splash;
						}
						else
						{
							m_needPlayerPokemon = true;
							m_userPlayer.GetNextPokemon(m_enemyPlayer, this.ReplacementHandler);
						}
					}
					
					if (m_enemyPlayer.ActivePokemon.isDead())
					{
						if (m_enemyPlayer.IsDefeated())
						{
							m_nextState = State.Victory;
						}
						else
						{
							m_needEnemyPokemon = true;
							m_enemyPlayer.GetNextPokemon(m_userPlayer, this.ReplacementHandler);
						}
					}
					
					m_waitingForInput = true;
				}
				else
				{
					if (!m_needPlayerPokemon && !m_needEnemyPokemon)
					{
						m_waitingForInput = false;
						m_nextState = State.ProcessTurn;
					}
				}
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
		
		RandomMoveRequest moveRequest = new RandomMoveRequest();
		SequentialPokemonRequest nextPokemonRequest = new SequentialPokemonRequest();
		NoCounterRequest counterRequest = new NoCounterRequest();
		Player player = new Player(trainerName, moveRequest, nextPokemonRequest, counterRequest);
		
		Pokemon.Species[] speciesOptions = (Pokemon.Species[])Enum.GetValues(typeof(Pokemon.Species));
		speciesOptions = speciesOptions.Where(x => x != Pokemon.Species.None).ToArray();
		for (int i = 0; i < 3; ++i)
		//for (int i = 0; i < 1; ++i)
		{
			int index = m_generator.Next(0, speciesOptions.Length);
			
			Pokemon.Species species = speciesOptions[index];
			
			Character enemy = PokemonFactory.CreatePokemon(species, 50, "", Pokemon.Gender.Random);
			
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

}