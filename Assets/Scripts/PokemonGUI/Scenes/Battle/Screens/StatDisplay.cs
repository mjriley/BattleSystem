using UnityEngine;
using PokeCore;
using PokeCore.Pokemon;
using System.Collections.Generic;

using PokemonGUI;
using PokemonGUI.Controls;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Controllers;

namespace Screens {

public class StatDisplay : MonoBehaviour, IGameScreen
{
	Texture2D m_solidTexture;
	Texture2D m_background;
	
	PokemonSummaryController m_controller;
	
	Character m_pokemon;
	
	KeyValuePair<string, int>[] m_values;
	
	
	void Awake()
	{
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
		m_background = Resources.Load<Texture2D>("Textures/ButtonLayout");
	}
	
	public void SetController(PokemonSummaryController controller)
	{
		m_controller = controller;
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
		
		m_values = new KeyValuePair<string, int>[5];
		m_values[0] = new KeyValuePair<string, int>("ATTACK", m_pokemon.Atk);
		m_values[1] = new KeyValuePair<string, int>("DEFENSE", m_pokemon.Def);
		m_values[2] = new KeyValuePair<string, int>("SP. ATK", m_pokemon.SpAtk);
		m_values[3] = new KeyValuePair<string, int>("SP. DEF", m_pokemon.SpDef);
		m_values[4] = new KeyValuePair<string, int>("SPEED", m_pokemon.Spd);
	}
	
	public int m_statX = 265;
	public int m_statOffsetX = 5;
	public int m_statY = 80;
	public int m_statOffsetY = 5;
	public int m_statHeight = 26;
	public int m_statNameWidth = 70;
	public int m_statValueWidth;
	public int m_fieldWidth;
	
	public GUIStyle m_statStyle;
	public GUIStyle m_numberStyle;
	
	public GUIStyle m_backgroundStyle;
	public GUIStyle m_movesStyle;
	
	public int m_itemX = 40;
	public int m_itemY = 140;
	public int m_itemWidth = 150;
	public int m_itemHeight = 26;
	public GUIStyle m_itemStyle;
	
	public int m_moveX = 0;
	public int m_moveY = 100;
	public int m_moveWidth = 300;
	public int m_moveHeight = 60;
	public GUIStyle m_moveStyle;
	
	public int m_boxY = 40;
	public int m_boxHeight = 160;
	
	public int m_levelX = 10;
	public int m_hpX = 350;
	public int m_levelY = 4;
	public int m_nextLvY = 24;
	
	public int m_hpBarX = 300;
	public int m_hpBarY = 50;
	
	public Color m_color1 = new Color(0.2f, 0.2f, 0.2f);
	public Color m_color2 = new Color(0.4f, 0.4f, 0.4f);
	
	void OnGUI()
	{
		Color prevColor = GUI.color;
		GUI.color = Color.black;
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_solidTexture);
		GUI.color = prevColor;
		
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0, 0, bounds.width, bounds.height), m_background);
			
			DetailElements.Display(new Rect(0, 0, bounds.width, bounds.height), m_pokemon);
			
			Rect boxBounds = new Rect(0, m_boxY, bounds.width, m_boxHeight);
			GUIUtils.DrawGroup(boxBounds, delegate()
			{
				prevColor = GUI.color;
				GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
				GUI.DrawTexture(new Rect(0, 0, boxBounds.width, boxBounds.height), m_solidTexture);
				GUI.color = prevColor;
				
				GUI.Label(new Rect(m_levelX, m_levelY, 100, m_statHeight), "Lv. " + m_pokemon.Level, m_statStyle);
				GUI.Label(new Rect(m_levelX, m_nextLvY, 100, m_statHeight), "NEXT LV.", m_statStyle);
				GUI.Label(new Rect(m_hpX, m_levelY, 100, m_statHeight), "HP", m_statStyle);
				
				Vector2 healthSize = HealthBar.CalcSize();
				HealthBar.Display(new Rect(m_hpBarX, m_hpBarY, healthSize.x, healthSize.y), m_pokemon.CurrentHP, m_pokemon.MaxHP);
				
				GUIContent healthRatio = new GUIContent(m_pokemon.CurrentHP + "/" + m_pokemon.MaxHP);
				GUI.Label(new Rect(m_hpBarX + healthSize.x - 100, m_levelY, 100, m_statHeight), healthRatio, m_numberStyle);
			});
			
			// placeholder for move
			prevColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;
			GUI.Label(new Rect(m_moveX, m_moveY, m_moveWidth, m_moveHeight), "<move>", m_moveStyle);
			GUI.backgroundColor = prevColor;
			
			// placeholder for held item
			prevColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.yellow;
			GUI.Label(new Rect(m_itemX, m_itemY, m_itemWidth, m_itemHeight), "<Held Item>", m_itemStyle);
			GUI.backgroundColor = prevColor;
		
			float fieldWidth = bounds.width - m_statX;
			Rect statBounds = new Rect(m_statX, m_statY, fieldWidth, m_statHeight * 5 + m_statOffsetY * 2);
			GUIUtils.DrawGroup(statBounds, delegate()
			{
				prevColor = GUI.color;
				GUI.color = Color.red;
				GUI.Box(new Rect(0, 0, statBounds.width, statBounds.height), "", m_backgroundStyle);
				GUI.color = prevColor;
				
				m_statValueWidth = (int)(fieldWidth - m_statNameWidth - m_statOffsetX * 2);
				
				for (int i = 0; i < m_values.Length; ++i)
				{
					prevColor = GUI.color;
					GUI.color = (i % 2 == 0) ? m_color1 : m_color2;
					GUI.DrawTexture(new Rect(0, m_statOffsetY + m_statHeight * i, fieldWidth, m_statHeight), m_solidTexture);
					GUI.color = prevColor;
					
					GUI.Label(new Rect(m_statOffsetX, m_statOffsetY + m_statHeight * i, m_statNameWidth, m_statHeight), 
						m_values[i].Key, m_statStyle);
					GUI.Label(new Rect(m_statOffsetX + m_statNameWidth, m_statOffsetY + m_statHeight * i, fieldWidth - m_statNameWidth - m_statOffsetX * 2, m_statHeight),
						m_values[i].Value.ToString(), m_numberStyle);
				}
			});
			
			if (GUI.Button(new Rect(155, 199, 140, 26), "MOVES", m_movesStyle))
			{
				m_controller.SwitchScreens(PokemonSummaryController.ActiveScreen.Moves);
			}
		});
		
		GUIUtils.DrawEnabled(m_controller.HasPreviousPage, delegate
		{
			if (Common3DS.BackButton())
			{
				m_controller.PreviousPage();
			}
		});
		
		GUIUtils.DrawEnabled(m_controller.HasNextPage, delegate
		{
			if (Common3DS.NextButton())
			{
				m_controller.NextPage();
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}

}}}}