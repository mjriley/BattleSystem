using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleDisplay : MonoBehaviour
{
	public GameObject splashScreen;
	
	private NewBattleSystem m_system = new NewBattleSystem();
	
	private List<IAnimationEffect> m_animations = new List<IAnimationEffect>();
	
	GameObject m_playerObject;
	GameObject m_enemyObject;
	SpriteRenderer m_playerDisplay;
	SpriteRenderer m_enemyDisplay;
	
	AudioSource m_audio;
	AudioClip m_damageSound;
	
	private PlayerStatusDisplay m_playerStatusDisplay;
	private PlayerStatusDisplay m_enemyStatusDisplay;
	
	bool m_ready = true;
	string m_statusText = "";
	
	Texture2D m_fightButtonDownTexture;
	GUIStyle m_fightButtonStyle;
	
	Texture2D m_bagButtonDownTexture;
	GUIStyle m_bagButtonStyle;
	
	Texture2D m_pokemonButtonDownTexture;
	GUIStyle m_pokemonButtonStyle;
	
	Texture2D m_runButtonDownTexture;
	GUIStyle m_runButtonStyle;
	
	Texture2D m_backButtonTexture;
	Texture2D m_backButtonDownTexture;
	GUIStyle m_backButtonStyle;
	
	Texture2D m_basicTexture;
	Texture2D m_topBarTexture;
	Texture2D m_bottomBarTexture;
	
	Texture2D m_fightButtonTexture;
	Texture2D m_bagButtonShadowTexture;
	Texture2D m_bagButtonTexture;
	Texture2D m_runButtonShadowTexture;
	Texture2D m_runButtonTexture;
	Texture2D m_pokemonButtonShadowTexture;
	Texture2D m_pokemonButtonTexture;
	
	Texture2D m_buttonLayoutTexture;
	
	public GUIStyle typeNameStyle;
	public GUIStyle abilityNameStyle;
	public GUIStyle abilityDetailsStyle;
	public GUIStyle buttonStyle;
	
	Rect m_screenArea;
	
	Rect m_topScreen = new Rect(0, 0, 400, 240);
	Rect m_bottomScreen = new Rect(0, 240, 400, 240);
	
	public delegate void AnimationCallback(MonoBehaviour script);
	
	void HandleAnimationComplete(MonoBehaviour script)
	{
		script.gameObject.GetComponent<Animator>().enabled = true;
		Destroy(script);
	}
	
	
	/** GUI variables ******/
	public int statusHeight = 10;
	public GUIStyle statusStyle;
	
	/***************/
	
	public void Start()
	{
		m_basicTexture = Resources.Load<Texture2D>("Textures/OptionBackground");
		m_topBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenTopEdge");
		m_bottomBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenEdge");
		
		m_fightButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Fight");
		
		m_bagButtonShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/BagShadow");
		m_bagButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Bag");
		
		m_runButtonShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/RunShadow");
		m_runButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Run");
		
		m_pokemonButtonShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonShadow");
		m_pokemonButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Pokemon");
		
		m_buttonLayoutTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		
		m_audio = gameObject.GetComponent<AudioSource>();
		m_damageSound = Resources.Load<AudioClip>("SoundEffects/attack_sound");
		
		m_system.CreatePlayerPokemon();
		m_playerObject = GameObject.FindGameObjectWithTag("PlayerDisplay");
		m_playerDisplay = m_playerObject.GetComponent<SpriteRenderer>();
		
		m_enemyObject = GameObject.FindGameObjectWithTag("EnemyDisplay");
		m_enemyDisplay = m_enemyObject.GetComponent<SpriteRenderer>();
		
		m_playerDisplay.enabled = false;
		
		m_enemyDisplay.enabled = false;
		
		m_system.BattleProgress += HandleBattleEvents;
		m_system.EnterState += HandleEnterState;
		m_system.LeaveState += HandleLeaveState;
		
		m_backButtonTexture = Resources.Load<Texture2D>("Textures/BackButton");
		m_backButtonDownTexture = Resources.Load<Texture2D>("Textures/BackButtonDown");
		m_backButtonStyle = new GUIStyle();
		m_backButtonStyle.normal.background = m_backButtonTexture;
		m_backButtonStyle.active.background = m_backButtonDownTexture;
		
		m_screenArea = new Rect(0, areaY, Screen.width, Screen.height - areaY - statusHeight);
		
		Vector2 displayCoords = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
		Rect playerStatusRect = new Rect(0, 0, displayCoords.x, displayCoords.y);
		
		Rect enemyStatusRect = new Rect(Screen.width - displayCoords.x, 0, displayCoords.x, displayCoords.y);
		
		m_playerStatusDisplay = new PlayerStatusDisplay(playerStatusRect);
		m_playerStatusDisplay.ActivePlayer = m_system.UserPlayer;
		
		m_enemyStatusDisplay = new PlayerStatusDisplay(enemyStatusRect);
	}

	void HandleLeaveState (object sender, StateChangeArgs e)
	{
		if (e.State == NewBattleSystem.State.Splash)
		{
			splashScreen.SetActive(false);
		}
	}

	void HandleEnterState (object sender, StateChangeArgs e)
	{
		if (e.State == NewBattleSystem.State.Splash)
		{
			splashScreen.SetActive(true);
		}
	}
	
	private bool IsReady()
	{
		return (m_ready && m_animations.Count == 0);
	}
	
	private void UpdateAnimations()
	{
		List<IAnimationEffect> nextAnimations = new List<IAnimationEffect>();
		
		foreach (IAnimationEffect anim in m_animations)
		{
			anim.Update();
			
			if (!anim.Done)
			{
				nextAnimations.Add(anim);
			}
		}
		
		m_animations = nextAnimations;
	}
	
	void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			DoneWithText();
		}
		
		UpdateAnimations();
		
		if (IsReady())
		{
			m_system.Update();
		}
		
		if (m_system.CurrentState == NewBattleSystem.State.ProcessTurn)
		{
			m_playerStatusDisplay.DisplayBalls = false;
			m_enemyStatusDisplay.DisplayBalls = false;
		}
		else
		{
			m_playerStatusDisplay.DisplayBalls = true;
			m_enemyStatusDisplay.DisplayBalls = true;
		}
	}
	
	public int areaY = 100;
	
	public int abilityButtonGapX = 10;
	public int abilityButtonGapY = 10;
	public int abilityButtonHeight = 60;
	
	public GUIStyle m_playerNameStyle;
	
	public int pokemonButtonHeight = 90;
	public int pokemonButtonGapX = 50;
	public int pokemonButtonGapY = 7;
	
	public int nameOffsetX = 0;
	public int nameOffsetY = 0;
	
	public GUIStyle m_pokemonChoiceStyle;
	
	public int statOffsetX = 0;
	public int statOffsetY = 0;
	
	public GUIStyle m_statStyle;
	
	private void DisplayBackButton()
	{
		if (GUI.Button(new Rect(m_screenArea.width - m_backButtonTexture.width, m_screenArea.height - m_backButtonTexture.height, m_backButtonTexture.width, m_backButtonTexture.height), "", m_backButtonStyle))
		{
			m_system.ProcessUserChoice(-2);
			DoneWithText();
		}
	}
	
	private void DisplayPokemon()
	{
		for (int i = 0; i < 3; ++i)
		{
			int leftPokemonIndex = i * 2;
			Character leftPokemon = m_system.UserPlayer.Pokemon[leftPokemonIndex];
			if (leftPokemon != null)
			{
				Rect pokemonButtonLeft = new Rect(0, i * (pokemonButtonHeight + pokemonButtonGapY), (m_screenArea.width - pokemonButtonGapX) / 2.0f, pokemonButtonHeight);
				if (GUI.Button(pokemonButtonLeft, leftPokemon.Name))
				{
					DoneWithText();
					m_system.ProcessUserChoice(leftPokemonIndex);
				}
			}
			
			int rightPokemonIndex = i * 2 + 1;
			Character rightPokemon = m_system.UserPlayer.Pokemon[rightPokemonIndex];
			if (rightPokemon != null)
			{
				Rect pokemonButtonRight = new Rect((m_screenArea.width + pokemonButtonGapX) / 2.0f, i * (pokemonButtonHeight + pokemonButtonGapY), (m_screenArea.width - pokemonButtonGapX) / 2.0f, pokemonButtonHeight);
				if (GUI.Button(pokemonButtonRight, rightPokemon.Name))
				{
					DoneWithText();
					m_system.ProcessUserChoice(rightPokemonIndex);
				}
			}
		}
	}
	
	
	public Texture2D m_solidTexture;
	public int m_topBarHeight = 40;
	public int m_abilityVGap = 20;
	
	public int m_fightButtonGap = 2;
	
	GUIStyle m_emptyStyle = new GUIStyle();
	
	void OnGUI()
	{
		Color prevColor;
		m_playerStatusDisplay.Display(m_playerNameStyle);
		m_enemyStatusDisplay.Display(m_playerNameStyle);
		
		
		if (m_system.CurrentState == NewBattleSystem.State.CombatPrompt)
		{
			GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds) {
				Rect bagRect = new Rect(0.0f, bounds.height - m_bagButtonTexture.height, m_bagButtonTexture.width, m_bagButtonTexture.height);
				Rect runRect = new Rect((bounds.width - m_runButtonTexture.width) / 2.0f, bounds.height - m_runButtonTexture.height, m_runButtonTexture.width, m_runButtonTexture.height);
				Rect pokemonRect = new Rect(bounds.width - m_pokemonButtonTexture.width, bounds.height - m_pokemonButtonTexture.height, m_pokemonButtonTexture.width, m_pokemonButtonTexture.height);
			
				GUI.DrawTexture(new Rect(0.0f, 0.0f, m_screenArea.width, m_screenArea.height), m_basicTexture);
				
				// top bar
				GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, m_topBarTexture.height), m_topBarTexture);
				
				// button shadows
				GUI.DrawTexture(bagRect, m_bagButtonShadowTexture);
				GUI.DrawTexture(runRect, m_runButtonShadowTexture);
				GUI.DrawTexture(pokemonRect, m_pokemonButtonShadowTexture);
				
				// bottom bar
				GUI.DrawTexture(new Rect(0.0f, bounds.height - m_bottomBarTexture.height, bounds.width, m_bottomBarTexture.height), m_bottomBarTexture);
				
				//GUI.color = prevColor;
				//if (GUI.Button(new Rect((m_screenArea.width - fightButtonWidth) / 2.0f, 0.0f, fightButtonWidth, fightButtonHeight), "", m_fightButtonStyle))
				if (GUI.Button(new Rect((bounds.width - m_fightButtonTexture.width) / 2.0f, m_topBarHeight + m_fightButtonGap, m_fightButtonTexture.width, m_fightButtonTexture.height), m_fightButtonTexture, m_emptyStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Fight);
					DoneWithText();
				}
				
				//if (GUI.Button(new Rect(0, m_screenArea.height - sideButtonHeight, sideButtonWidth, sideButtonHeight), "", m_bagButtonStyle))
				if (GUI.Button(bagRect, m_bagButtonTexture, m_emptyStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Item);
					DoneWithText();
				}
				
				//if (GUI.Button(new Rect((m_screenArea.width - midButtonWidth) / 2.0f, m_screenArea.height - midButtonHeight, midButtonWidth, midButtonHeight), "", m_runButtonStyle))
				if (GUI.Button(runRect, m_runButtonTexture, m_emptyStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Run);
					DoneWithText();
				}
				
				//if (GUI.Button(new Rect(m_screenArea.width - sideButtonWidth, m_screenArea.height - sideButtonHeight, sideButtonWidth, sideButtonHeight), "", m_pokemonButtonStyle))
				if (GUI.Button(pokemonRect, m_pokemonButtonTexture, m_emptyStyle))
				{
					m_system.ProcessUserChoice((int)NewBattleSystem.CombatSelection.Pokemon);
					DoneWithText();
				}
			});
		}
		else if (m_system.CurrentState == NewBattleSystem.State.EnemyPokemonDefeated)
		{
			GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds) {
				GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_buttonLayoutTexture);
				
				GUIUtils.DrawGroup(new Rect(0.0f, bounds.height / 4.0f, bounds.width, bounds.height * 3.0f / 5.0f), delegate(Rect innerBounds)
				{
					prevColor = GUI.color;
					GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
					GUI.DrawTexture(new Rect(0.0f, 0.0f, innerBounds.width, innerBounds.height), m_basicTexture);
					GUI.color = prevColor;
					
					int buttonWidth = 340;
					int buttonHeight = 40;
					int buttonVPadding = 20;
					
					//if (GUI.Button(new Rect((innerBounds.width - buttonWidth) / 2.0f, buttonVPadding, buttonWidth, buttonHeight), "Switch Pokemon"))
					prevColor = GUI.backgroundColor;
					GUI.backgroundColor = Color.green;
					if (SwitchPokemonButton.Display(new Rect((innerBounds.width - buttonWidth) / 2.0f, buttonVPadding, buttonWidth, buttonHeight), "SWITCH POKÉMON"))
					{
						m_system.ProcessUserChoice(1);
						DoneWithText();
					}
					
					//if (GUI.Button(new Rect((innerBounds.width - buttonWidth) / 2.0f, innerBounds.height - buttonHeight - buttonVPadding, buttonWidth, buttonHeight), "Continue Battling"))
					GUI.backgroundColor = Color.cyan;
					if (SwitchPokemonButton.Display(new Rect((innerBounds.width - buttonWidth) / 2.0f, innerBounds.height - buttonHeight - buttonVPadding, buttonWidth, buttonHeight), "CONTINUE BATTLING"))
					{
						m_system.ProcessUserChoice(-2);
						DoneWithText();
					}
					
					GUI.backgroundColor = prevColor;
					
					});
				
			});
		}
		else if (m_system.CurrentState == NewBattleSystem.State.ReplacePokemon)
		{
			GUI.BeginGroup(m_screenArea);
				DisplayPokemon();
				DisplayBackButton();
			GUI.EndGroup();
		}
		else if (m_system.CurrentState == NewBattleSystem.State.FightPrompt ||
			m_system.CurrentState == NewBattleSystem.State.ItemPrompt ||
			m_system.CurrentState == NewBattleSystem.State.PokemonPrompt)
		{
			GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds) {
				GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_buttonLayoutTexture);
				
				if (m_system.CurrentState == NewBattleSystem.State.FightPrompt)
				{
					// top bar
					GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, m_topBarTexture.height), m_topBarTexture);
					
					int initialY = m_topBarTexture.height + m_abilityVGap;
					
					Character pokemon = m_system.UserPlayer.ActivePokemon;
					
					Rect buttonBounds = new Rect(0, initialY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
					//if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[0], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, m_emptyStyle))
					if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[0]))
					{
						DoneWithText();
						m_system.ProcessUserChoice(0);
					}
					
					buttonBounds = new Rect((bounds.width + abilityButtonGapX) / 2.0f, initialY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
					//if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[1], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, m_emptyStyle))
					if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[1], true))
					{
						DoneWithText();
						m_system.ProcessUserChoice(1);
					}
					
					buttonBounds = new Rect(0, initialY + abilityButtonHeight + abilityButtonGapY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
					//if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[2], false, typeNameStyle, abilityNameStyle, abilityDetailsStyle, m_emptyStyle))
					if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[2]))
					{
						DoneWithText();
						m_system.ProcessUserChoice(2);
					}
					
					buttonBounds = new Rect((bounds.width + abilityButtonGapX) / 2.0f, initialY + abilityButtonHeight + abilityButtonGapY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
					//if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[3], true, typeNameStyle, abilityNameStyle, abilityDetailsStyle, m_emptyStyle))
					if (AbilityButton.Display(buttonBounds, pokemon.getAbilities()[3], true))
					{
						DoneWithText();
						m_system.ProcessUserChoice(3);
					}
				}
				else if (m_system.CurrentState == NewBattleSystem.State.PokemonPrompt)
				{
					DisplayPokemon();
				}

				DisplayBackButton();
			});
		}
		else
		{
//			GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds)
//			{
//				
//			});

			GUIUtils.DrawBottomScreenBackground(m_bottomScreen);
			
		}
	
		GUIUtils.DrawGroup(m_topScreen, delegate(Rect bounds) {
			GUI.backgroundColor = new Color(0.2f, 0.2f, 0.4f);
			GUI.Box(new Rect(0, bounds.height - statusHeight, bounds.width, statusHeight), m_statusText, statusStyle);
		});
		
	}
	
	private void HandleBattleEvents(object sender, EventArgs e)
	{
		if (e is NewEncounterEventArgs)
		{
			m_enemyStatusDisplay.ActivePlayer = m_system.EnemyPlayer;
			
			m_playerStatusDisplay.Enabled = false;
			m_enemyStatusDisplay.Enabled = false;
		}
		if (e is StatusUpdateEventArgs)
		{
			StatusUpdateEventArgs args = (StatusUpdateEventArgs)e;
			m_statusText = args.Status; 
			m_ready = false;
			if (args.Expires)
			{
				StartCoroutine("Wait");
			}
		}
		else if (e is DeployEventArgs)
		{
			DeployEventArgs args = (DeployEventArgs)e;
			BallZoneIn zoneScript;
			
			//if (args.Friendly)
			if (args.Pokemon == m_system.UserPlayer.ActivePokemon)
			{
				zoneScript = m_playerDisplay.GetComponentInChildren<BallZoneIn>();
				zoneScript.Species = m_system.UserPlayer.ActivePokemon.Species;
				
				m_playerStatusDisplay.UpdatePokemon();
				m_playerStatusDisplay.Enabled = true;
			}
			else
			{
				zoneScript = m_enemyDisplay.GetComponentInChildren<BallZoneIn>();
				zoneScript.Species = m_system.EnemyPlayer.ActivePokemon.Species;
				
				m_enemyStatusDisplay.UpdatePokemon();
				m_enemyStatusDisplay.Enabled = true;
			}
			
			ScriptAnimation anim = new ScriptAnimation(zoneScript);
			m_animations.Add(anim);
			anim.Start();
		}
		else if (e is WithdrawEventArgs)
		{
			WithdrawEventArgs args = (WithdrawEventArgs)e;
			ScriptAnimation anim;
			if (args.Pokemon == m_system.UserPlayer.ActivePokemon)
			{
				anim = new ScriptAnimation(m_playerDisplay.GetComponentInChildren<BallZoneOut>());
				m_playerStatusDisplay.Enabled = false;
			}
			else
			{
				anim = new ScriptAnimation(m_enemyDisplay.GetComponentInChildren<BallZoneOut>());
				m_enemyStatusDisplay.Enabled = false;
			}
			m_animations.Add(anim);
			anim.Start();
		}
		else if (e is DamageEventArgs)
		{
			DamageEventArgs args = (DamageEventArgs)e;
			Animator animator;
			
			PlayerStatusDisplay targetDisplay;
			if (args.Player == m_system.UserPlayer)
			{
				animator = m_playerObject.transform.Find("explosionAnimation").GetComponent<Animator>();
				targetDisplay = m_playerStatusDisplay;
			}
			else
			{
				animator = m_enemyObject.transform.Find("explosionAnimation").GetComponent<Animator>();
				targetDisplay = m_enemyStatusDisplay;
			}
			
			GradualDamageAnimation healthAnim = new GradualDamageAnimation(targetDisplay, args.Amount, 60);
			m_animations.Add(healthAnim);
			healthAnim.Start();
			
			AnimatorAnimation anim = new AnimatorAnimation(animator);
			m_animations.Add(anim);
			anim.Start();
			
			SoundEffect sound = new SoundEffect(m_audio, m_damageSound);
			m_animations.Add(sound);
			sound.Start();
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
