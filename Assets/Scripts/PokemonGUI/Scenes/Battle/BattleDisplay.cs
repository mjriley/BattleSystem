using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PokeCore;

using PokemonGUI.Controls;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {

public class BattleDisplay : MonoBehaviour
{
	public GameObject ParentController;
	public GameObject splashScreen;
	TrainerDisplay m_trainerDisplay;
	
	NewBattleSystem m_system;
	
	List<IAnimationEffect> m_animations = new List<IAnimationEffect>();
	
	GameObject m_playerObject;
	GameObject m_enemyObject;
	SpriteRenderer m_playerDisplay;
	SpriteRenderer m_enemyDisplay;
	
	Texture2D m_backgroundTexture;
	
	VictoryDisplay m_victoryDisplay;
	
	AudioSource m_audio;
	AudioClip m_damageSound;
	
	PlayerStatusDisplay m_playerStatusDisplay;
	PlayerStatusDisplay m_enemyStatusDisplay;
	
	bool m_ready = true;
	string m_statusText = "";
	
	/** GUI variables ******/
	public GUIStyle statusStyle;
	public GUIStyle m_playerNameStyle;
	/***************/
	
	public delegate void AnimationCallback(MonoBehaviour script);
	
	void HandleAnimationComplete(MonoBehaviour script)
	{
		script.gameObject.GetComponent<Animator>().enabled = true;
		Destroy(script);
	}
	
	void ClearDisplay()
	{
		ClearMessage();
		m_animations.Clear();
		m_playerDisplay.enabled = false;
		m_enemyDisplay.enabled = false;
	}
	
	void Reset()
	{
		ClearDisplay();
		m_system.Reset();
	}
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
	}
	
	void Start()
	{
		m_system = ParentController.GetComponent<BattleController>().GetSystem();
		m_victoryDisplay = new VictoryDisplay(m_system, this);
		
		m_trainerDisplay = splashScreen.GetComponent<TrainerDisplay>();
		
		m_audio = gameObject.GetComponent<AudioSource>();
		m_damageSound = Resources.Load<AudioClip>("SoundEffects/attack_sound");
		
		m_playerObject = GameObject.FindGameObjectWithTag("PlayerDisplay");
		m_playerDisplay = m_playerObject.GetComponent<SpriteRenderer>();
		
		m_enemyObject = GameObject.FindGameObjectWithTag("EnemyDisplay");
		m_enemyDisplay = m_enemyObject.GetComponent<SpriteRenderer>();
		
		m_playerDisplay.enabled = false;
		
		m_enemyDisplay.enabled = false;
		
		m_system.BattleProgress += HandleBattleEvents;
		m_system.EnterState += HandleEnterState;
		m_system.LeaveState += HandleLeaveState;
		
		Vector2 displayCoords = PlayerStatusDisplay.CalcMinSize(m_playerNameStyle);
		Rect playerStatusRect = new Rect(0, 0, displayCoords.x, displayCoords.y);
		
		Rect enemyStatusRect = new Rect(Screen.width - displayCoords.x, 0, displayCoords.x, displayCoords.y);
		
		m_playerStatusDisplay = new PlayerStatusDisplay(playerStatusRect);
		m_playerStatusDisplay.ActivePlayer = m_system.UserPlayer;
		
		m_enemyStatusDisplay = new PlayerStatusDisplay(enemyStatusRect);
	}

	void HandleLeaveState(object sender, StateChangeArgs e)
	{
		if (e.State == NewBattleSystem.State.Splash)
		{
			splashScreen.SetActive(false);
		}
	}

	void HandleEnterState(object sender, StateChangeArgs e)
	{
		if (e.State == NewBattleSystem.State.Splash)
		{
			splashScreen.SetActive(true);
		}
	}
	
	bool IsReady()
	{
		return (m_ready && m_animations.Count == 0);
	}
	
	void UpdateAnimations()
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
		if (Input.GetKeyDown(KeyCode.F5))
		{
			Reset();
		}
		
		if (Input.GetKeyDown("space"))
		{
			ClearMessage();
		}
		
		UpdateAnimations();
		
		if (IsReady())
		{
			m_system.Update();
		}
		
		if (m_system.CurrentState == NewBattleSystem.State.Victory)
		{
			m_victoryDisplay.Update();
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
	
	void OnGUI()
	{
		GUI.depth = 5;
		GUIUtils.DrawSeparatorBar();
		
		m_playerStatusDisplay.Display(m_playerNameStyle);
		m_enemyStatusDisplay.Display(m_playerNameStyle);
		
		if (m_system.CurrentState == NewBattleSystem.State.Victory)
		{
			m_victoryDisplay.Display();
		}
	
		GUIUtils.DrawGroup(ScreenCoords.TopScreen, delegate(Rect bounds)
		{
			statusStyle.wordWrap = true;
			GUI.Label(new Rect(0, bounds.height - statusStyle.normal.background.height, bounds.width, statusStyle.normal.background.height), m_statusText, statusStyle);
		});
		
		Color prevColor = GUI.color;
		GUI.color = new Color(0.4f, 0.4f, 0.4f);
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		GUI.color = prevColor;
	}
	
	private void HandleBattleEvents(object sender, EventArgs e)
	{
		if (e is NewEncounterEventArgs)
		{
			ClearDisplay();
		
			NewEncounterEventArgs args = (NewEncounterEventArgs)e;
			m_enemyStatusDisplay.ActivePlayer = m_system.EnemyPlayer;
			m_trainerDisplay.UpdateTrainer(args.Trainer); 
			
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
		ClearMessage();
	}
	
	public void ClearMessage()
	{
		m_statusText = "";
		StopCoroutine("Wait");
		m_ready = true;
	}
}

}}}