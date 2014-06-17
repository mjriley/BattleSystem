using System;
using UnityEngine;
using PokeCore.Pokemon;
using PokemonGUI;
using PokemonGUI.Controls;

namespace PokemonGUI {
namespace Scenes {
namespace Roster {

public class PokemonListState : IDisplayState
{
	const float KEY_PRESS_DELAY = 0.2f; // ms
	GUIStyle m_style = new GUIStyle();
	
	Species[] m_options;
	float elapsed;
	
	bool m_arrowDownPressed = false;
	bool m_arrowUpPressed = false;
	
	int selectedIndex = 0;
	int scrollIndex = 0;
	const int ROWS_DISPLAYED = 6;
	
	int m_rowWidth;
	int m_rowHeight;
	
	int m_activeSlot;
	
	Vector2 scrollPosition = Vector2.zero;
	
	RosterController m_controller;
	
	Rect topScreen = new Rect(0, 0, 400, 240);
	Rect bottomScreen = new Rect(0, 260, 400, 240);
	
	Texture2D m_backgroundTexture;

	public PokemonListState(RosterController controller)
	{
		m_controller = controller;
		elapsed = KEY_PRESS_DELAY; // make sure key input is detected immediately
		
		InitScrollBarStyle();
		
		int scrollBarWidth = (int)(m_style.fixedWidth + m_style.margin.left);
		m_rowWidth = (int)(topScreen.width - scrollBarWidth);
		m_rowHeight = (int)(topScreen.height / ROWS_DISPLAYED);
		
		m_activeSlot = 0;
		
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/RosterSelectionBackground");
	}
	
	void InitScrollBarStyle()
	{
		m_style.fixedWidth = 15;
		m_style.margin.left = 1;
	}
	
	public void SetActiveSlot(int slot)
	{
		m_activeSlot = slot;
		m_options = m_controller.GetSlotOptions(slot);
		PokemonPrototype prototype = m_controller.GetRosterSlot(slot);
		Species species = (prototype == null) ? Species.None : prototype.Species;
		selectedIndex = Array.BinarySearch(m_options, species);
		
		FocusSelected();
	}
	
	// move the selected index into view
	public void FocusSelected()
	{
		scrollIndex = selectedIndex;
		scrollPosition.y = scrollIndex * m_rowHeight;
	}
	
	void SubmitPokemon()
	{
		PokemonPrototype prototype = new PokemonPrototype(m_options[selectedIndex], 50, Gender.Male);
		m_controller.SetSlotValue(m_activeSlot, prototype);
		
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SubmitPokemon();
			return;
		}
		
		elapsed += Time.deltaTime;
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (elapsed >= KEY_PRESS_DELAY || !m_arrowDownPressed)
			{
				if (selectedIndex + 1 < m_options.Length)
				{
					++selectedIndex;
					
					if (selectedIndex >= scrollIndex + ROWS_DISPLAYED)
					{
						++scrollIndex;
						scrollPosition.y = scrollIndex * m_rowHeight;
					}
				}
				
				m_arrowDownPressed = true;
				elapsed = 0.0f;
			}
		}
		else
		{
			m_arrowDownPressed = false;
		}
		
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			
			if (elapsed >= KEY_PRESS_DELAY || !m_arrowUpPressed)
			{
				if (selectedIndex > 0)
				{
					--selectedIndex;
					
					if (selectedIndex < scrollIndex)
					{
						--scrollIndex;
						scrollPosition.y = scrollIndex * m_rowHeight;
					}
				}
				
				m_arrowUpPressed = true;
				elapsed = 0.0f;
			}
		}
		else
		{
			m_arrowUpPressed = false;
		}
	}
	
	public void Display()
	{
		GUI.BeginGroup(topScreen);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, topScreen.width, topScreen.height), m_backgroundTexture);
			scrollPosition = GUI.BeginScrollView(new Rect(0.0f, 0.0f, topScreen.width, topScreen.height), scrollPosition, new Rect(0.0f, 0.0f, m_rowWidth, m_options.Length * m_rowHeight));
			
			for (int i = 0; i < m_options.Length; ++i)
			{
				PokemonSelectionButton.Display(new Rect(0.0f, i * m_rowHeight, m_rowWidth, m_rowHeight), m_options[i], (i == selectedIndex));
			}
			
			GUI.EndScrollView();
		GUI.EndGroup();
		
		GUIUtils.DrawBottomScreenBackground(bottomScreen);
		
		GUI.BeginGroup(bottomScreen);
			int width = 100;
			int height = 40;
			if (GUI.Button(new Rect((bottomScreen.width - width) / 2.0f, (bottomScreen.height - height) / 2.0f, width, height), "Accept"))
			{
				SubmitPokemon();
			}
		GUI.EndGroup();
	}
	
	public void OnEnter()
	{
	
	}
	
	public void OnLeave()
	{
	
	}
}

}}}
