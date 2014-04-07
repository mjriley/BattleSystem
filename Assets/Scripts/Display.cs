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
	
	public int tagOffset = 10;
	
	bool hasStarted = false;
	
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
	void Start () {
//		m_strategy = new UserInputStrategy(new UserInputStrategy.GetUserInput(GetUserInput));
//		m_character = new Character(70, m_strategy);
//		Ability ability1 = new Ability("Ability 1", "Normal", 20, 20);
//		Ability ability2 = new Ability("Ability 2", "Normal", 20, 20);
//		Ability ability3 = new Ability("Ability 3", "Normal", 20, 20);
//		Ability ability4 = new Ability("Ability 4", "Normal", 20, 20);
//		m_character.addAbility(ability1);
//		m_character.addAbility(ability2);
//		m_character.addAbility(ability3);
//		m_character.addAbility(ability4);
//		
//		Character enemy = new Character(70);
//		m_enemies = new List<Character>();
//		m_enemies.Add(enemy);
		
		//yield return StartCoroutine(DoInit());
		
		m_system = new BattleSystem(this.HandleText, this.CurrentMessageProcessed);
		m_system.CreatePlayerPokemon(this.HandleAbilities, this.GetAbilityChoice);
		
		//m_system.Start();
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
	
	void OnGUI()
	{
		int buttonHeight = 90;
		int padding = 50;
		
		int buttonWidth = (Screen.width - padding) / 2;
		
		if (m_waitingForInput)
		{
			GUI.DrawTexture(new Rect(0, 0, buttonWidth, buttonHeight), baseTexture);
			GUI.Box(new Rect(tagOffset, buttonHeight / 2 + 10, 70, 25), "Normal", typeNameStyle);
			GUI.Label(new Rect(0, 0, buttonWidth, buttonHeight / 2), m_abilities[0].Name, abilityNameStyle);
			GUI.Label(new Rect(0, buttonHeight / 2, buttonWidth, buttonHeight / 2), "PP 23/25", abilityDetailsStyle);
			if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), ""))
			{
				processButtonClick(0);
			}
			
			GUI.DrawTexture(new Rect(Screen.width, 0, -buttonWidth, buttonHeight), baseTexture);
			GUI.Box(new Rect((Screen.width + padding) / 2 + tagOffset, buttonHeight / 2 + 10, 70, 25), "Normal", typeNameStyle);
			GUI.Label(new Rect((Screen.width + padding) / 2, 0, buttonWidth, buttonHeight / 2), m_abilities[1].Name, abilityNameStyle);
			GUI.Label(new Rect((Screen.width + padding) / 2, buttonHeight / 2, buttonWidth, buttonHeight / 2), "PP 23/25", abilityDetailsStyle);
			if (GUI.Button(new Rect((Screen.width + padding) / 2, 0, buttonWidth, buttonHeight), ""))
			{
				processButtonClick(1);
			}
			
			GUI.DrawTexture(new Rect(0, 120, buttonWidth, buttonHeight), baseTexture);
			GUI.Box(new Rect(tagOffset, 120 + buttonHeight / 2 + 10, 70, 25), "Normal", typeNameStyle);
			GUI.Label(new Rect(0, 120, buttonWidth, buttonHeight / 2), m_abilities[2].Name, abilityNameStyle);
			GUI.Label(new Rect(0, 120 + buttonHeight / 2, buttonWidth, buttonHeight / 2), "PP 23/25", abilityDetailsStyle);
			if (GUI.Button(new Rect(0, 120, buttonWidth, 90), ""))
			{
				processButtonClick(2);
			}
			
			GUI.DrawTexture (new Rect(Screen.width, 120, -buttonWidth, buttonHeight), baseTexture);
			GUI.Box(new Rect((Screen.width + padding) / 2 + tagOffset, 120 + buttonHeight / 2 + 10, 70, 25), "Normal", typeNameStyle);
			GUI.Label(new Rect((Screen.width + padding) / 2, 120, buttonWidth, buttonHeight / 2), m_abilities[3].Name, abilityNameStyle);
			GUI.Label(new Rect((Screen.width + padding) / 2, 120 + buttonHeight / 2, buttonWidth, buttonHeight / 2), "PP 23/25", abilityDetailsStyle);
			if (GUI.Button(new Rect((Screen.width + padding) / 2, 120, buttonWidth, buttonHeight), ""))
			{
				processButtonClick(3);
			}
		}
		
		//GUI.color = Color.black;
		//GUI.Box(new Rect(150, 10, 100, 90), texture, style);
//		else
//		{
			GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
			GUI.Box(new Rect(0, Screen.height - 50, Screen.width, 50), m_statusText, statusStyle);
//		}
	}
}
