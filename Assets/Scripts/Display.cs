//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class Display : MonoBehaviour {
//	private enum State
//	{
//		Default,
//		DisplayAbilities,
//		SubmitAbilityChoice,
//		DisplayPokemon,
//		SubmitPokemonChoice
//	}
//	
//	private State m_currentState = State.Default;
//
//	public Texture2D baseTexture;
//	public Texture2D typeTexture;
//	
//	public GUIStyle style;
//	
//	public GUIStyle abilityNameStyle;
//	public GUIStyle abilityDetailsStyle;
//	public GUIStyle typeNameStyle;
//	public GUIStyle statusStyle;
//	public GUIStyle buttonStyle;
//	
//	public int tagOffset = 10;
//	
//	ITurnAction m_selectedAction;
//	
//	private List<Ability> m_abilities;
//	
//	bool m_handledCurrentText = true;
//	int m_selectedPokemon;
//
//
//	private Player m_userPlayer;
//	private Player m_enemyPlayer;
//	private UserInputStrategy m_strategy;
//	
//	private BattleSystem m_system;
//	
//	private string m_statusText = "";
//	
//	SpriteRenderer m_playerDisplay;
//	SpriteRenderer m_enemyDisplay;
//	
//	Animator m_playerAnimator;
//	Animator m_enemyAnimator;
//
//	// Use this for initialization
//	void Start()
//	{
//		m_system = new BattleSystem(this.HandleText, this.CurrentMessageProcessed);
//		m_system.CreatePlayerPokemon(this.HandleAbilities, this.GetTurnAction, this.GetNextPokemon);
//		m_userPlayer = m_system.UserPlayer;
//		
//		GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerDisplay");
//		m_playerDisplay = playerObject.GetComponent<SpriteRenderer>();
//		m_playerAnimator = m_playerDisplay.GetComponent<Animator>();
//		
//		
//		GameObject enemyObject = GameObject.FindGameObjectWithTag("EnemyDisplay");
//		m_enemyDisplay = enemyObject.GetComponent<SpriteRenderer>();
//		m_enemyAnimator = m_enemyDisplay.GetComponent<Animator>();
//		
//		m_playerDisplay.GetComponent<SpriteRenderer>().enabled = true;
//		m_enemyDisplay.GetComponent<SpriteRenderer>().enabled = false;
//	}
//	
//	void HandleText(string message)
//	{
//		// do something
//		m_statusText = message;
//		
//		m_handledCurrentText = false;
//		
//		StartCoroutine("Wait");
//	}
//	
//	void HandleAbilities(List<Ability> abilities)
//	{
//		m_enemyPlayer = m_system.EnemyPlayer;
//		m_abilities = abilities;
//		
//		m_currentState = State.DisplayAbilities;
//	}
//	
//	ITurnAction GetTurnAction()
//	{
//		if (m_currentState == State.SubmitAbilityChoice)
//		{
//			m_currentState = State.Default;
//			return m_selectedAction;
//		}
//		
//		return null;
//	}
//	
//	int GetNextPokemon()
//	{
//		if (m_currentState != State.DisplayPokemon)
//		{
//			m_currentState = State.DisplayPokemon;
//			m_selectedPokemon = -1;
//		}
//		
//		if (m_selectedPokemon != -1)
//		{
//			m_currentState = State.SubmitPokemonChoice;
//			return m_selectedPokemon;
//		}
//		
//		return -1;
//	}
//	
//	bool CurrentMessageProcessed()
//	{
//		return m_handledCurrentText;
//	}
//	
//	IEnumerator Wait()
//	{
//		yield return new WaitForSeconds(4);
//		m_statusText = "";
//		m_handledCurrentText = true;
//	}
//	
//	void DoneWithText()
//	{
//		StopCoroutine("Wait");
//		m_handledCurrentText = true;
//	}
//	
//	void processButtonClick(int index)
//	{
//		m_selectedAction = new AbilityUse(m_userPlayer.ActivePokemon, m_enemyPlayer, m_abilities[index]);
//		DoneWithText();
//		
//		m_currentState = State.SubmitAbilityChoice;
//	}
//	
//	void processPokemonClick(int index)
//	{
//		if (m_system.IntState == BattleSystem.InternalState.WaitingOnPlayerChoice)
//		{
//			m_selectedAction = new SwapAbility(m_userPlayer, index);
//			DoneWithText();
//			m_currentState = State.SubmitAbilityChoice;
//		}
//		else if (m_system.IntState == BattleSystem.InternalState.WaitingOnNextPokemonSelection)
//		{
//			m_selectedPokemon = index;
//			DoneWithText();
//			m_currentState = State.SubmitPokemonChoice;
//		}
//	}
//	
//	void UpdatePokemonAnimations()
//	{
//		string friendlyPokemon = m_userPlayer.ActivePokemon.Species.ToString();
//		if (m_playerAnimator.runtimeAnimatorController.ToString() != friendlyPokemon)
//		{
//			m_playerAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + friendlyPokemon);
//		}
//		
//		if (m_enemyPlayer != null)
//		{
//			string enemyPokemon = m_enemyPlayer.ActivePokemon.Species.ToString();
//			
//			if (m_enemyAnimator.runtimeAnimatorController.ToString() != enemyPokemon)
//			{
//				m_enemyAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + enemyPokemon);
//			}
//			
//			if (!m_enemyDisplay.enabled)
//			{
//				m_enemyDisplay.enabled = true;
//			}
//		}
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		UpdatePokemonAnimations();
//		
//		if (Input.GetKeyDown("space"))
//		{
//			DoneWithText();
//		}
//		
//		if (m_currentState == State.DisplayAbilities)
//		{
//			if (Input.GetKeyDown("1"))
//			{
//				processButtonClick(0);
//			}
//			else if (Input.GetKeyDown("2"))
//			{
//				processButtonClick(1);
//			}
//			else if (Input.GetKeyDown("3"))
//			{
//				processButtonClick(2);
//			}
//			else if (Input.GetKeyDown("4"))
//			{
//				processButtonClick(3);
//			}
//			else if (Input.GetKeyDown("p"))
//			{
//				m_currentState = State.DisplayPokemon;
//			}
//		}
//		else if (m_currentState == State.DisplayPokemon)
//		{
//			if (Input.GetKeyDown("escape"))
//			{
//				m_currentState = State.DisplayAbilities;
//			}
//		}
//		
//		m_system.Update();
//	}
//	
//	public int pokemonNameHeight = 20;
//	public int pokemonNameWidth = 70;
//	public int statusHeight = 50;
//	
//	public int healthDisplayWidth = 439;
//	public int healthDisplayHeight = 77;
//	
//	public GUIStyle m_playerNameStyle;	
//	
//	public Texture2D pokemonTagTexture;
//	public int pokemonHeight = 65;
//	public int pokemonHPadding = 50;
//	public int pokemonVPadding = 7;
//	
//	void OnGUI()
//	{
//		int buttonHeight = 90;
//		int padding = 50;
//		
//		int buttonWidth = (Screen.width - padding) / 2;
//		
//		if (m_currentState == State.DisplayAbilities)
//		{
//			Rect buttonBounds = new Rect(0, 0, buttonWidth, buttonHeight);
//			if (AbilityButton.Display(buttonBounds, m_abilities[0], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
//			{
//				processButtonClick(0);
//			}
//			
//			buttonBounds = new Rect((Screen.width + padding) / 2, 0, buttonWidth, buttonHeight);
//			if (AbilityButton.Display(buttonBounds, m_abilities[1], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
//			{
//				processButtonClick(1);
//			}
//			
//			buttonBounds = new Rect(0, 120, buttonWidth, buttonHeight);
//			if (AbilityButton.Display(buttonBounds, m_abilities[2], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
//			{
//				processButtonClick(2);
//			}
//			
//			buttonBounds = new Rect((Screen.width + padding) / 2, 120, buttonWidth, buttonHeight);
//			if (AbilityButton.Display(buttonBounds, m_abilities[3], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
//			{
//				processButtonClick(3);
//			}
//		}
//		else if (m_currentState == State.DisplayPokemon)
//		{
//			int pokemonWidth = (Screen.width - pokemonHPadding) / 2;
//			
//			for (int i = 0, pokemonIndex = 0; i < 3; ++i, ++pokemonIndex)
//			{
//				Character leftPokemon = null;
//				if (pokemonIndex < m_userPlayer.Pokemon.Count)
//				{
//					leftPokemon = m_userPlayer.Pokemon[pokemonIndex];
//				}
//				//if (PokemonTagDisplay.Button(new Rect(0, i * (pokemonHeight + pokemonVPadding), pokemonWidth, pokemonHeight), leftPokemon, false))
//				if (PokemonTagDisplay.Button(new Rect(0, i * (pokemonHeight + pokemonVPadding), pokemonWidth, pokemonHeight), leftPokemon, false, 
//					tagNameStyle, tagOffsetX, tagOffsetY, statOffsetX, statOffsetY, statStyle))
//				{
//					processPokemonClick(pokemonIndex);
//				}
//				++pokemonIndex;
//				
//				Character rightPokemon = null;
//				if (pokemonIndex < m_userPlayer.Pokemon.Count)
//				{
//					rightPokemon = m_userPlayer.Pokemon[pokemonIndex];
//				}
//				//if (PokemonTagDisplay.Button(new Rect((Screen.width + pokemonHPadding) / 2, i * (pokemonHeight + pokemonVPadding) + pokemonVPadding, pokemonWidth, pokemonHeight), rightPokemon, true))
//				if (PokemonTagDisplay.Button(new Rect((Screen.width + pokemonHPadding) / 2, i * (pokemonHeight + pokemonVPadding) + pokemonVPadding, pokemonWidth, pokemonHeight), rightPokemon, true, 
//					tagNameStyle, tagOffsetX, tagOffsetY, statOffsetX, statOffsetY, statStyle))
//				{
//					processPokemonClick(pokemonIndex);
//				}
//			}
//		}
//		
//		if (m_system.BattleState == BattleSystem.State.InBattle)
//		{
//			Vector2 playerSizeInfo = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
//			Rect playerRect = new Rect(0, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);	
//			PlayerStatusDisplay.Display(playerRect, m_userPlayer.ActivePokemon, m_userPlayer, m_playerNameStyle);
//			
//			Rect aiRect = new Rect(Screen.width - playerSizeInfo.x, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);
//			PlayerStatusDisplay.Display(aiRect, m_enemyPlayer.ActivePokemon, m_enemyPlayer, m_playerNameStyle);
//		}
//		
//		GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
//		GUI.Box(new Rect(0, Screen.height - statusHeight, Screen.width, statusHeight), m_statusText, statusStyle);
//	}
//	
//	public int tagOffsetX = 65;
//	public int tagOffsetY = 13;
//	public int statOffsetX = 0;
//	public int statOffsetY = 0;
//	public GUIStyle tagNameStyle;
//	public GUIStyle statStyle;
//	
//}
