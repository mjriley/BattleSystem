using UnityEngine;
using System;
using System.Collections;

public class BattleDisplay : MonoBehaviour
{
	private NewBattleSystem m_system = new NewBattleSystem();
	
	SpriteRenderer m_playerDisplay;
	SpriteRenderer m_enemyDisplay;
	
	Animator m_playerAnimator;
	Animator m_enemyAnimator;
	
	bool m_ready = true;
	string m_statusText = "";
	
	Texture2D m_fightButtonTexture;
	Texture2D m_fightButtonDownTexture;
	GUIStyle m_fightButtonStyle;
	
	Texture2D m_bagButtonTexture;
	Texture2D m_bagButtonDownTexture;
	GUIStyle m_bagButtonStyle;
	
	Texture2D m_pokemonButtonTexture;
	Texture2D m_pokemonButtonDownTexture;
	GUIStyle m_pokemonButtonStyle;
	
	Texture2D m_runButtonTexture;
	Texture2D m_runButtonDownTexture;
	GUIStyle m_runButtonStyle;
	
	Texture2D m_backButtonTexture;
	Texture2D m_backButtonDownTexture;
	GUIStyle m_backButtonStyle;
	
	public GUIStyle typeNameStyle;
	public GUIStyle abilityNameStyle;
	public GUIStyle abilityDetailsStyle;
	public GUIStyle buttonStyle;
	
	Rect m_screenArea;
	
	Ability m_ability;
	
	public delegate void AnimationCallback(MonoBehaviour script);
	
	void HandleAnimationComplete(MonoBehaviour script)
	{
		script.gameObject.GetComponent<Animator>().enabled = true;
	}
	
	
	/** GUI variables ******/
	public int statusHeight = 10;
	public GUIStyle statusStyle;
	
	/***************/
	
	public void Start()
	{
		m_ability = new Ability("Test", BattleType.Fire, 20, 100, 20);
	
		m_system.CreatePlayerPokemon();
		GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerDisplay");
		m_playerDisplay = playerObject.GetComponent<SpriteRenderer>();
		m_playerAnimator = m_playerDisplay.GetComponent<Animator>();
		
		
		GameObject enemyObject = GameObject.FindGameObjectWithTag("EnemyDisplay");
		m_enemyDisplay = enemyObject.GetComponent<SpriteRenderer>();
		m_enemyAnimator = m_enemyDisplay.GetComponent<Animator>();
		
		m_playerDisplay.enabled = false;
		m_playerAnimator.enabled = false;
		
		m_enemyDisplay.enabled = false;
		m_enemyAnimator.enabled = false;
		
		m_system.BattleProgress += HandleBattleEvents;
		
		m_fightButtonTexture = Resources.Load<Texture2D>("Textures/FightButton");
		m_fightButtonDownTexture = Resources.Load<Texture2D>("Textures/FightButtonDown");
		m_fightButtonStyle = new GUIStyle();
		m_fightButtonStyle.normal.background = m_fightButtonTexture;
		m_fightButtonStyle.active.background = m_fightButtonDownTexture;
		
		m_bagButtonTexture = Resources.Load<Texture2D>("Textures/BagButton");
		m_bagButtonDownTexture = Resources.Load<Texture2D>("Textures/BagButtonDown");
		m_bagButtonStyle = new GUIStyle();
		m_bagButtonStyle.normal.background = m_bagButtonTexture;
		m_bagButtonStyle.active.background = m_bagButtonDownTexture;
		
		m_pokemonButtonTexture = Resources.Load<Texture2D>("Textures/PokemonButton");
		m_pokemonButtonDownTexture = Resources.Load<Texture2D>("Textures/PokemonButtonDown");
		m_pokemonButtonStyle = new GUIStyle();
		m_pokemonButtonStyle.normal.background = m_pokemonButtonTexture;
		m_pokemonButtonStyle.active.background = m_pokemonButtonDownTexture;
		
		m_runButtonTexture = Resources.Load<Texture2D>("Textures/RunButton");
		m_runButtonDownTexture = Resources.Load<Texture2D>("Textures/RunButtonDown");
		m_runButtonStyle = new GUIStyle();
		m_runButtonStyle.normal.background = m_runButtonTexture;
		m_runButtonStyle.active.background = m_runButtonDownTexture;
		
		m_backButtonTexture = Resources.Load<Texture2D>("Textures/BackButton");
		m_backButtonDownTexture = Resources.Load<Texture2D>("Textures/BackButtonDown");
		m_backButtonStyle = new GUIStyle();
		m_backButtonStyle.normal.background = m_backButtonTexture;
		m_backButtonStyle.active.background = m_backButtonDownTexture;
		
		m_screenArea = new Rect(0, areaY, Screen.width, Screen.height - areaY - statusHeight);
	}
	
	void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			DoneWithText();
		}
		
		if (Input.GetKeyDown("a"))
		{
			m_playerAnimator.Play("FadeIn");
		}
		
		if (m_ready)
		{
			m_system.Update();
		}
	}
	
	public int fightButtonWidth = 100;
	public int fightButtonHeight = 100;
	
	public int sideButtonWidth = 100;
	public int sideButtonHeight = 100;
	
	public int midButtonWidth = 100;
	public int midButtonHeight = 100;
	
	public int backButtonWidth = 100;
	public int backButtonHeight = 100;
	
	public int areaY = 100;
	
	public int abilityButtonGapX = 10;
	public int abilityButtonGapY = 10;
	public int abilityButtonHeight = 90;
	
	public GUIStyle m_playerNameStyle;
	
	void OnGUI()
	{
	
		Vector2 playerDisplayCoords = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
		Rect playerStatusRect = new Rect(0, 0, playerDisplayCoords.x, playerDisplayCoords.y);
		PlayerStatusDisplay.Display(playerStatusRect, m_system.UserPlayer.ActivePokemon, m_system.UserPlayer, m_playerNameStyle);
		
		Vector2 enemyDisplayCoords = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
		Rect enemyStatusRect = new Rect(Screen.width - enemyDisplayCoords.x, 0, enemyDisplayCoords.x, enemyDisplayCoords.y);
		PlayerStatusDisplay.Display(enemyStatusRect, m_system.EnemyPlayer.ActivePokemon, m_system.EnemyPlayer, m_playerNameStyle);
		
		if (m_system.CurrentState == NewBattleSystem.State.CombatPrompt)
		{
			GUI.BeginGroup(m_screenArea);
				if (GUI.Button(new Rect((m_screenArea.width - fightButtonWidth) / 2.0f, 0.0f, fightButtonWidth, fightButtonHeight), "", m_fightButtonStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Fight);
					DoneWithText();
				}
				
				if (GUI.Button(new Rect(0, m_screenArea.height - sideButtonHeight, sideButtonWidth, sideButtonHeight), "", m_bagButtonStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Item);
					DoneWithText();
				}
				
				if (GUI.Button(new Rect((m_screenArea.width - midButtonWidth) / 2.0f, m_screenArea.height - midButtonHeight, midButtonWidth, midButtonHeight), "", m_runButtonStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Run);
					DoneWithText();
				}
				
				if (GUI.Button(new Rect(m_screenArea.width - sideButtonWidth, m_screenArea.height - sideButtonHeight, sideButtonWidth, sideButtonHeight), "", m_pokemonButtonStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Pokemon);
					DoneWithText();
				}
			GUI.EndGroup();
		}
		else if (m_system.CurrentState == NewBattleSystem.State.FightPrompt ||
			m_system.CurrentState == NewBattleSystem.State.ItemPrompt ||
			m_system.CurrentState == NewBattleSystem.State.PokemonPrompt)
		{
			GUI.BeginGroup(m_screenArea);
				if (m_system.CurrentState == NewBattleSystem.State.FightPrompt)
				{
					Character pokemon = m_system.UserPlayer.ActivePokemon;
					
					Rect buttonBounds = new Rect(0, 0, (m_screenArea.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
					if (GUI.Button(buttonBounds, pokemon.getAbilities()[0].Name))
					{
						DoneWithText();
						m_system.ProcessUserChoice(0);
					}
					
					if (GUI.Button(new Rect((m_screenArea.width + abilityButtonGapX) / 2.0f, 0, (m_screenArea.width - abilityButtonGapX) / 2.0f, abilityButtonHeight), pokemon.getAbilities()[1].Name))
					{
						DoneWithText();
						m_system.ProcessUserChoice(1);
					}
					
					if (GUI.Button(new Rect(0, abilityButtonHeight + abilityButtonGapY, (m_screenArea.width - abilityButtonGapX) / 2.0f, abilityButtonHeight), pokemon.getAbilities()[2].Name))
					{
						DoneWithText();
						m_system.ProcessUserChoice(2);
					}
					
					if (GUI.Button(new Rect((m_screenArea.width + abilityButtonGapX) / 2.0f, abilityButtonHeight + abilityButtonGapY, (m_screenArea.width - abilityButtonGapX) / 2.0f, abilityButtonHeight),
						pokemon.getAbilities()[3].Name))
					{
						DoneWithText();
						m_system.ProcessUserChoice(3);
					}
				}
				
				if (GUI.Button(new Rect(m_screenArea.width - m_backButtonTexture.width, m_screenArea.height - m_backButtonTexture.height, m_backButtonTexture.width, m_backButtonTexture.height), "", m_backButtonStyle))
				{
					m_system.ProcessUserChoice(-2);
					DoneWithText();
				}
			GUI.EndGroup();
		}
	
		GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
		GUI.Box(new Rect(0, Screen.height - statusHeight, Screen.width, statusHeight), m_statusText, statusStyle);
	}
	
	private void HandleBattleEvents(object sender, EventArgs e)
	{
		if (e is StatusUpdateEventArgs)
		{
			StatusUpdateEventArgs args = (StatusUpdateEventArgs)e;
			m_statusText = args.Status; 
			m_ready = false;
			StartCoroutine("Wait");
		}
		else if (e is DeployEventArgs)
		{
			DeployEventArgs args = (DeployEventArgs)e;
			if (args.Friendly)
			{
				BallZoneIn animation = m_playerDisplay.gameObject.AddComponent("BallZoneIn") as BallZoneIn;
				animation.sprite = Resources.Load<Sprite>("Textures/circle");
				animation.callback = this.HandleAnimationComplete;
				m_playerDisplay.enabled = true;
				
			}
			else
			{
				BallZoneIn animation = m_enemyDisplay.gameObject.AddComponent("BallZoneIn") as BallZoneIn;
				animation.sprite = Resources.Load<Sprite>("Textures/circle");
				animation.callback = this.HandleAnimationComplete;
				m_enemyDisplay.enabled = true;
			}
			
			m_ready = false;
			// wait for the animation to complete
			StartCoroutine("Wait");
		}
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(4);
		DoneWithText();
	}
	
	private void DoneWithText()
	{
		m_statusText = "";
		StopCoroutine("Wait");
		m_ready = true;
	}
}
