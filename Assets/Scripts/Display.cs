using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Display : MonoBehaviour {
	private enum State
	{
		Default,
		DisplayAbilities,
		SubmitAbilityChoice,
		DisplayPokemon
	}
	
	private State m_currentState = State.Default;

	public Texture2D baseTexture;
	public Texture2D typeTexture;
	
	public GUIStyle style;
	
	public GUIStyle abilityNameStyle;
	public GUIStyle abilityDetailsStyle;
	public GUIStyle typeNameStyle;
	public GUIStyle statusStyle;
	public GUIStyle buttonStyle;
	
	public int tagOffset = 10;
	
	//bool m_waitingForInput = false;
	//int m_pressedIndex = 0;
	//IAbility m_selectedAbility;
	ITurnAction m_selectedAction;
	
	private List<Ability> m_abilities;
	
	bool m_handledCurrentText = true;


	private Player m_userPlayer;
	private Player m_enemyPlayer;
	//private Character m_character;
	//private List<Character> m_enemies;
	private UserInputStrategy m_strategy;
	
	private BattleSystem m_system;
	
	private string m_statusText = "";

	// Use this for initialization
	void Start()
	{
		m_system = new BattleSystem(this.HandleText, this.CurrentMessageProcessed);
		m_system.CreatePlayerPokemon(this.HandleAbilities, this.GetTurnAction);
		m_userPlayer = m_system.UserPlayer;
		//m_character = m_system.ActivePokemon;
		
		GameObject playerDisplay = GameObject.FindGameObjectWithTag("PlayerDisplay");
		GameObject enemyDisplay = GameObject.FindGameObjectWithTag("EnemyDisplay");
		
		playerDisplay.GetComponent<SpriteRenderer>().enabled = true;
		enemyDisplay.GetComponent<SpriteRenderer>().enabled = true;
	}
	
	void HandleText(string message)
	{
		// do something
		m_statusText = message;
		
		m_handledCurrentText = false;
		
		StartCoroutine("Wait");
	}
	
	void HandleAbilities(List<Ability> abilities)
	{
		m_enemyPlayer = m_system.EnemyPlayer;
		//m_enemies = m_system.Enemies;
		m_abilities = abilities;
		//m_waitingForInput = true;
		
		m_currentState = State.DisplayAbilities;
	}
	
	ITurnAction GetTurnAction()
	{
		//if (!m_waitingForInput)
		if (m_currentState == State.SubmitAbilityChoice)
		{
			m_currentState = State.Default;
			//return m_pressedIndex;
			//return m_selectedAbility;
			return m_selectedAction;
		}
		
		//return -1;
		return null;
	}
	
	bool CurrentMessageProcessed()
	{
		return m_handledCurrentText;
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(4);
		m_statusText = "";
		m_handledCurrentText = true;
	}
	
	void DoneWithText()
	{
		StopCoroutine("Wait");
		m_handledCurrentText = true;
	}
	
	void processButtonClick(int index)
	{
		//m_pressedIndex = index;
		m_selectedAction = new AbilityUse(m_userPlayer.ActivePokemon, m_enemyPlayer, m_abilities[index]);
		//m_selectedAbility = m_abilities[index];
		//m_waitingForInput = false;
		DoneWithText();
		
		m_currentState = State.SubmitAbilityChoice;
	}
	
	void SubmitSwapAbility(int index)
	{
		m_selectedAction = new SwapAbility(m_userPlayer, index);
		
		DoneWithText();
		
		m_currentState = State.SubmitAbilityChoice;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		if (Input.GetKeyDown("space") && !m_waitingForInput)
		if (Input.GetKeyDown("space"))
		{
			DoneWithText();
		}
		
		//if (m_waitingForInput)
		if (m_currentState == State.DisplayAbilities)
		{
			if (Input.GetKeyDown("1"))
			{
				processButtonClick(0);
			}
			else if (Input.GetKeyDown("2"))
			{
				processButtonClick(1);
			}
			else if (Input.GetKeyDown("3"))
			{
				processButtonClick(2);
			}
			else if (Input.GetKeyDown("4"))
			{
				processButtonClick(3);
			}
			else if (Input.GetKeyDown("p"))
			{
				m_currentState = State.DisplayPokemon;
			}
		}
		else if (m_currentState == State.DisplayPokemon)
		{
			if (Input.GetKeyDown("escape"))
			{
				m_currentState = State.DisplayAbilities;
			}
		}
		
		m_system.Update();
	}
	
	public int pokemonNameHeight = 20;
	public int pokemonNameWidth = 70;
	public int statusHeight = 50;
	
	public int healthDisplayWidth = 439;
	public int healthDisplayHeight = 77;
	
	public GUIStyle m_playerNameStyle;	
	
	public Texture2D pokemonTagTexture;
	public int pokemonHeight = 65;
	public int pokemonHPadding = 50;
	public int pokemonVPadding = 7;
	
	void OnGUI()
	{
		int buttonHeight = 90;
		int padding = 50;
		
		int buttonWidth = (Screen.width - padding) / 2;
		
		
		//if (m_waitingForInput)
		if (m_currentState == State.DisplayAbilities)
		{
			Rect buttonBounds = new Rect(0, 0, buttonWidth, buttonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[0], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
			{
				processButtonClick(0);
			}
			
			buttonBounds = new Rect((Screen.width + padding) / 2, 0, buttonWidth, buttonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[1], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
			{
				processButtonClick(1);
			}
			
			buttonBounds = new Rect(0, 120, buttonWidth, buttonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[2], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
			{
				processButtonClick(2);
			}
			
			buttonBounds = new Rect((Screen.width + padding) / 2, 120, buttonWidth, buttonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[3], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, buttonStyle))
			{
				processButtonClick(3);
			}
		}
		else if (m_currentState == State.DisplayPokemon)
		{
			int pokemonWidth = (Screen.width - pokemonHPadding) / 2;
			
			for (int i = 0, pokemonIndex = 0; i < 3; ++i, ++pokemonIndex)
			{
				Character leftPokemon = null;
				if (pokemonIndex < m_userPlayer.Pokemon.Count)
				{
					leftPokemon = m_userPlayer.Pokemon[pokemonIndex];
				}
				if (PokemonTagDisplay.Button(new Rect(0, i * (pokemonHeight + pokemonVPadding), pokemonWidth, pokemonHeight), leftPokemon, false))
				{
					SubmitSwapAbility(pokemonIndex);
				}
				++pokemonIndex;
				
				Character rightPokemon = null;
				if (pokemonIndex < m_userPlayer.Pokemon.Count)
				{
					rightPokemon = m_userPlayer.Pokemon[pokemonIndex];
				}
				if (PokemonTagDisplay.Button(new Rect((Screen.width + pokemonHPadding) / 2, i * (pokemonHeight + pokemonVPadding) + pokemonVPadding, pokemonWidth, pokemonHeight), rightPokemon, true))
				{
					SubmitSwapAbility(pokemonIndex);
				}
				//GUI.DrawTexture(new Rect(0, i * (pokemonHeight + pokemonVPadding), pokemonWidth, pokemonHeight), pokemonTagTexture);
				//GUI.DrawTexture(new Rect((Screen.width + pokemonHPadding) / 2, i * (pokemonHeight + pokemonVPadding), pokemonWidth, pokemonHeight), pokemonTagTexture);
			}
		}
		
		if (m_system.BattleState == BattleSystem.State.InBattle)
		{
			Vector2 playerSizeInfo = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
			Rect playerRect = new Rect(0, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);	
			//PlayerStatusDisplay.Display(playerRect, "Goober", Character.Sex.Male, m_playerNameStyle);
			PlayerStatusDisplay.Display(playerRect, m_userPlayer.ActivePokemon, m_userPlayer, m_playerNameStyle);
			
			Rect aiRect = new Rect(Screen.width - playerSizeInfo.x, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);
			//PlayerStatusDisplay.Display(aiRect, "AI Pokemon", Character.Sex.Female, m_playerNameStyle);
			//PlayerStatusDisplay.Display(aiRect, m_enemies[0], m_enemyPlayer, m_playerNameStyle);
			PlayerStatusDisplay.Display(aiRect, m_enemyPlayer.ActivePokemon, m_enemyPlayer, m_playerNameStyle);
		}
		
		GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
		GUI.Box(new Rect(0, Screen.height - statusHeight, Screen.width, statusHeight), m_statusText, statusStyle);
		
	}
	
}
