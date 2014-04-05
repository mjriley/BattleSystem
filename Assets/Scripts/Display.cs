using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Display : MonoBehaviour {

	public Texture2D baseTexture;
	public Texture2D typeTexture;
	public GUIStyle style;
	
	public GUIStyle abilityNameStyle;
	public GUIStyle typeNameStyle;
	
	bool hasStarted = false;
	
	bool m_waitingForInput = false;
	int m_pressedIndex = 0;


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
		
		m_system = new BattleSystem(this.HandleText, this.GetPlayerChoice);
		m_system.CreatePlayerPokemon();
		
		m_system.Start();
	}
	
	void HandleText(string message)
	{
		// do something
		m_statusText = message;
		
		StartCoroutine(Wait());
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(5);
	}
	
	int GetPlayerChoice(List<Ability> abilities)
	{
		// do something
		
		return 1;
	}
	
//	private IEnumerator DoInit()
//	{
//		yield return new WaitForSeconds(5);
//		m_character.handleTurn(null, m_enemies);
//		hasStarted = !hasStarted;
//	}


	
	int GetUserInput()
	{
		m_waitingForInput = true;
		StartCoroutine(WaitForButtonPress());
		return m_pressedIndex;
	}
	
	IEnumerator WaitForButtonPress()
	{
		while (!m_waitingForInput)
		{
			yield return null;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
//		if (m_waitingForInput)
//		{
//	
//			//GUI.color = Color.yellow;
//	//		GUI.DrawTexture(new Rect(10, 10, 200, 90), baseTexture);
//	//		
//	//		
//	//		GUI.DrawTexture(new Rect(460, 10, -200, 90), baseTexture);
//	//		
//	//		GUI.DrawTexture (new Rect(10, 120, 200, 90), baseTexture);
//	//		
//	//		GUI.DrawTexture (new Rect(460, 120, -200, 90), baseTexture);
//	//		
//	//		GUI.Box (new Rect(10, 10, 100, 90), typeTexture, style);
//	//		
//	//		GUI.Label (new Rect(10, 10, 100, 90), "Me First", abilityNameStyle);
//	//		GUI.Label (new Rect(10, 10, 100, 90), "Normal", typeNameStyle);
//			
//			if (GUI.Button(new Rect(10, 10, 200, 90), baseTexture))
//			{
//				m_pressedIndex = 0;
//				m_waitingForInput = false;
//			}
//			
//			if (GUI.Button(new Rect(260, 10, 200, 90), baseTexture))
//			{
//				m_pressedIndex = 1;
//				m_waitingForInput = false;
//			}
//			
//			if (GUI.Button(new Rect(10, 120, 200, 90), baseTexture))
//			{
//				m_pressedIndex = 2;
//				m_waitingForInput = false;
//			}
//			
//			if (GUI.Button(new Rect(260, 120, 200, 90), baseTexture))
//			{
//				m_pressedIndex = 3;
//				m_waitingForInput = false;
//			}
//		}
		
		//GUI.color = Color.black;
		//GUI.Box(new Rect(150, 10, 100, 90), texture, style);
//		else
//		{
			GUI.Label(new Rect(10, 10, 200, 90), m_statusText);
//		}
	}
}
