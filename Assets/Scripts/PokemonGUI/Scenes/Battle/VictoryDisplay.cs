using UnityEngine;
using System;
using System.Reflection;
using PokeCore;
using PokemonGUI;

using PokemonGUI.Scenes.Battle;


// TODO: Should probably be a mono behaviour class

// The view for player victory.
// Allows the user to choose to continue or quit
public class VictoryDisplay
{
	Texture2D m_solidTexture;
	NewBattleSystem m_system;
	
	public GUIStyle victoryStyle;
	public GUIStyle statusStyle;
	public GUIStyle optionStyle;
	
	int m_currentIndex;
	
	int m_victoryY = 20;
	int m_statusY = 60;
	
	int m_optionX = 175;
	int m_optionStartY = 120;
	int m_optionWidth = 100;
	int m_optionHeight = 30;
	int m_optionVGap = 10;
	
	int m_arrowOffset = 30;
	
	Options[] m_optionValues;
	
	public VictoryDisplay(NewBattleSystem system, BattleDisplay parentDisplay)
	{
		m_system = system;
		
		Init();
	}

	public enum Options
	{
		Continue = 0,
		Quit
	}

	void Init()
	{
		m_optionValues = (Options[])Enum.GetValues(typeof(Options));
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
		
		victoryStyle = new GUIStyle();
		victoryStyle.alignment = TextAnchor.MiddleCenter;
		victoryStyle.normal.textColor = Color.white;
		victoryStyle.fontSize = 32;
		victoryStyle.fontStyle = FontStyle.Bold;
		
		statusStyle = new GUIStyle();
		statusStyle.alignment = TextAnchor.MiddleCenter;
		statusStyle.normal.textColor = new Color32(19, 255, 0, 255);
		statusStyle.fontSize = 16;
		
		optionStyle = new GUIStyle();
		optionStyle.alignment = TextAnchor.MiddleLeft;
		optionStyle.normal.textColor = Color.white;
		optionStyle.fontSize = 18;
		optionStyle.fontStyle = FontStyle.Bold;
		
		Reset();
	}
	
	void Reset()
	{
		m_currentIndex = 0;
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			m_system.ProcessUserChoice((int)m_optionValues[m_currentIndex]);
			return;
		}
			
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			m_currentIndex = (m_currentIndex + 1) % 2;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_currentIndex = (m_currentIndex + 2 - 1) % 2;
		}
	}
	
	public void OnGUI()
	{
		Display();
	}
	
	void DrawTopScreen()
	{
		GUIUtils.DrawGroup(ScreenCoords.TopScreen, delegate(Rect bounds)
		{
			Color prevColor = GUI.color;
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_solidTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect((bounds.width - m_optionWidth) / 2.0f, m_victoryY, m_optionWidth, m_optionHeight), "Victory!", victoryStyle);
			GUI.Label(new Rect((bounds.width - 200) / 2.0f, m_statusY, 200, m_optionHeight), "Consecutive Victories: " + m_system.Victories, statusStyle);
			
			for (int i = 0; i < m_optionValues.Length; ++i)
			{
				int y = m_optionStartY + i * (m_optionHeight + m_optionVGap);
				GUI.Label(new Rect(m_optionX, y, m_optionWidth, m_optionHeight), m_optionValues[i].ToString(), optionStyle);
				if (m_currentIndex == i)
				{
					GUIContent cursor = new GUIContent("➤");
					GUI.Label(new Rect(m_optionX - m_arrowOffset, y, m_arrowOffset, m_optionHeight), cursor, optionStyle);
				}
			}
		});
	}
	
	void DrawBottomScreen()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Color prevColor = GUI.color;
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_solidTexture);
			GUI.color = prevColor;
		});
	}
	
	public void Display()
	{
		DrawTopScreen();
		DrawBottomScreen();
	}
}
