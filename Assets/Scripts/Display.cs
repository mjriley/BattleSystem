using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Display : MonoBehaviour {

	public Texture2D baseTexture;
	public Texture2D typeTexture;
	
	
	public GUIStyle style;
	
	public GUIStyle abilityNameStyle;
	public GUIStyle abilityDetailsStyle;
	public GUIStyle typeNameStyle;
	public GUIStyle statusStyle;
	public GUIStyle buttonStyle;
	
	public int tagOffset = 10;
	
	bool m_waitingForInput = false;
	int m_pressedIndex = 0;
	
	private List<Ability> m_abilities;
	
	bool m_handledCurrentText = true;


	private Character m_character;
	private List<Character> m_enemies;
	private UserInputStrategy m_strategy;
	
	private BattleSystem m_system;
	
	private string m_statusText = "";

	// Use this for initialization
	void Start()
	{
		m_system = new BattleSystem(this.HandleText, this.CurrentMessageProcessed);
		m_system.CreatePlayerPokemon(this.HandleAbilities, this.GetAbilityChoice);
		m_character = m_system.ActivePokemon;
		
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
		m_enemies = m_system.Enemies;
		m_abilities = abilities;
		m_waitingForInput = true;
	}
	
	int GetAbilityChoice()
	{
		if (!m_waitingForInput)
		{
			return m_pressedIndex;
		}
		
		return -1;
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
		StopCoroutine ("Wait");
		m_handledCurrentText = true;
	}
	
	void processButtonClick(int index)
	{
		m_pressedIndex = index;
		m_waitingForInput = false;
		DoneWithText();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown("space") && !m_waitingForInput)
		{
			DoneWithText();
		}
		
		if (m_waitingForInput)
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
		}
		m_system.Update();
	}
	
	public int pokemonNameHeight = 20;
	public int pokemonNameWidth = 70;
	public int statusHeight = 50;
	
	public int healthDisplayWidth = 439;
	public int healthDisplayHeight = 77;
	
	public GUIStyle m_playerNameStyle;	
	
	void OnGUI()
	{
		int buttonHeight = 90;
		int padding = 50;
		
		int buttonWidth = (Screen.width - padding) / 2;
		
		if (m_waitingForInput)
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
		
		if (m_system.BattleState == BattleSystem.State.InBattle)
		{
			Vector2 playerSizeInfo = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
			Rect playerRect = new Rect(0, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);	
			//PlayerStatusDisplay.Display(playerRect, "Goober", Character.Sex.Male, m_playerNameStyle);
			PlayerStatusDisplay.Display(playerRect, m_character, m_playerNameStyle);
			
			Rect aiRect = new Rect(Screen.width - playerSizeInfo.x, Screen.height - statusHeight - playerSizeInfo.y, playerSizeInfo.x, playerSizeInfo.y);
			//PlayerStatusDisplay.Display(aiRect, "AI Pokemon", Character.Sex.Female, m_playerNameStyle);
			PlayerStatusDisplay.Display(aiRect, m_enemies[0], m_playerNameStyle);
		}
		
		GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
		GUI.Box(new Rect(0, Screen.height - statusHeight, Screen.width, statusHeight), m_statusText, statusStyle);
	}
	
}
