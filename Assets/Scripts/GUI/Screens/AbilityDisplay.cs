using UnityEngine;
using PokeCore;
using Abilities;
using Pokemon;
using System.Collections.Generic;
using PokemonGUI;

public class AbilityDisplay : MonoBehaviour, IGameScreen
{
	Texture2D m_backgroundTexture;
	Texture2D m_topBarTexture;
	
	IAbilityController m_controller;
	
	List<Ability> m_abilities;
	protected Character m_pokemon;
	
	protected void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_topBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenTopEdge");
	}
	
	void OnEnable()
	{
		Invalidate();
	}
	
	public void Invalidate()
	{
		if (m_controller == null)
		{
			PlayerRoster roster = GameObject.Find("PlayerRoster").GetComponent<PlayerRoster>();
			PokemonPrototype proto = roster.GetRosterSlot(0);
			
			m_pokemon = PokemonFactory.CreatePokemon(proto.Species, 50, "", Gender.Male);
		}
		else
		{
			m_pokemon = m_controller.GetPokemon();
		}
		
		m_abilities = m_pokemon.getAbilities();
	}
	
	public void SetController(IAbilityController controller)
	{
		m_controller = controller;
	}
	
	// Tweaking variables
	public int m_abilityVGap = 20;
	public int abilityButtonGapX = 10;
	public int abilityButtonGapY = 10;
	public int abilityButtonHeight = 60;
	
	protected void OnGUI()
	{
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, m_topBarTexture.height), m_topBarTexture);
			
			int initialY = m_topBarTexture.height + m_abilityVGap;
			
			Rect buttonBounds = new Rect(0, initialY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[0]))
			{
				m_controller.ProcessInput(0);
			}
			
			buttonBounds = new Rect((bounds.width + abilityButtonGapX) / 2.0f, initialY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[1], true))
			{
				m_controller.ProcessInput(1);
			}
		
			buttonBounds = new Rect(0, initialY + abilityButtonHeight + abilityButtonGapY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[2]))
			{
				m_controller.ProcessInput(2);
			}
			
			buttonBounds = new Rect((bounds.width + abilityButtonGapX) / 2.0f, initialY + abilityButtonHeight + abilityButtonGapY, (bounds.width - abilityButtonGapX) / 2.0f, abilityButtonHeight);
			if (AbilityButton.Display(buttonBounds, m_abilities[3], true))
			{
				m_controller.ProcessInput(3);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}
