using System;
using UnityEngine;

public class RosterState : IDisplayState
{
	const int NUM_ROWS = 3;
	const int NUM_COLUMNS = 2;
	
	PokemonListState m_listState;
	
	RosterController m_controller;
	
	Rect topScreen = new Rect(0, 0, 400, 240);
	Rect bottomScreen = new Rect(0, 260, 400, 240);
	
	Texture2D m_backgroundTexture;
	
	int m_activeRow = 0;
	int m_activeColumn = 0;
	
	public RosterState(RosterController controller)
	{
		m_controller = controller;
		
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/RosterSelectionBackground");
	}
	
	int GetIndex(int row, int column)
	{
		return row * NUM_COLUMNS + column;
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			m_activeColumn = Mathf.Min(m_activeColumn + 1, NUM_COLUMNS - 1);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			m_activeColumn = Mathf.Max(m_activeColumn - 1, 0);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_activeRow = Mathf.Max(m_activeRow - 1, 0);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			m_activeRow = Mathf.Min(m_activeRow + 1, NUM_ROWS - 1);
		}
	}
	
	public GUIStyle speciesStyle = new GUIStyle();
	public void Display()
	{
		Vector2 buttonSize = PokemonRosterButton.CalcSize();
		
		int activeIndex = GetIndex(m_activeRow, m_activeColumn);
		
		GUIUtils.DrawGroup(topScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_backgroundTexture);
			
			for (int y = 0; y < NUM_ROWS; ++y)
			{
				for (int x = 0; x < NUM_COLUMNS; ++x)
				{
					float offsetY = (x % 2 == 0) ? 0 : buttonSize.y / 2;
					float offsetX = (x % 2 == 0) ? 0 : bounds.width - buttonSize.x;
					
					//int index = NUM_COLUMNS * y + x;
					int index = GetIndex(y, x);
					
					PokemonPrototype prototype = m_controller.GetRosterSlot(index);
					
					if (PokemonRosterButton.Display(new Rect(offsetX, y * buttonSize.y + offsetY, buttonSize.x, buttonSize.y), 
						prototype, (activeIndex == index)))
					{
						m_controller.DisplaySlotOptions(index);
					}
				}
			}
		});
		
		DrawBottomScreen();
	}
	
	void SetActiveIndex(int index)
	{
		m_activeColumn = index % NUM_COLUMNS;
		m_activeRow = index / NUM_COLUMNS;
	}
	
	public int m_offsetX = 10;
	public int m_offsetY = 10;
	public int m_buttonHeight = 50;
	public int m_buttonWidth = 180;
	
	public int m_confirmButtonWidth = 76;
	public int m_confirmButtonHeight = 30;
	public int m_confirmOffsetX = 10;
	public int m_confirmOffsetY = 10;
	void DrawBottomScreen()
	{
		int borderSize = 5;
		GUIUtils.DrawBottomScreenBackground(bottomScreen, borderSize);
		
		Rect contentArea = new Rect(bottomScreen.x + borderSize, bottomScreen.y + borderSize, bottomScreen.width - borderSize * 2, bottomScreen.height - borderSize * 2);
		
		
		GUIUtils.DrawGroup(contentArea, delegate(Rect bounds)
		{
			int activeIndex = GetIndex(m_activeRow, m_activeColumn);
			
			bool isExistingPokemon = (m_controller.GetRosterSlot(activeIndex) != null);
			
			Rect pokemonRect = new Rect(m_offsetX, m_offsetY, m_buttonWidth, m_buttonHeight);
			if (GUI.Button(pokemonRect, "Change Pokemon"))
			{
				m_controller.DisplaySlotOptions(activeIndex);
			}
			
			
			GUIUtils.DrawEnabled(isExistingPokemon, delegate()
			{	
				Rect detailsRect = new Rect(bounds.width - m_offsetX - m_buttonWidth, m_offsetY, m_buttonWidth, m_buttonHeight);
				if (GUI.Button(detailsRect, "Details"))
				{
					m_controller.DisplayDetails(activeIndex);
				}
				
				Rect abilitiesRect = new Rect(m_offsetX, 2 * m_offsetY + m_buttonHeight, m_buttonWidth, m_buttonHeight);
				GUI.Button(abilitiesRect, "Abilities");
				
				GUIUtils.DrawEnabled((m_controller.GetRosterSize() != 1), delegate()
				{
					Rect removeRect = new Rect(bounds.width - m_offsetX - m_buttonWidth, 2 * m_offsetY + m_buttonHeight, m_buttonWidth, m_buttonHeight);
					if (GUI.Button(removeRect, "Remove"))
					{
						m_controller.RemovePokemonAtSlot(activeIndex);
						if (activeIndex >= m_controller.GetRosterSize())
						{
							SetActiveIndex(activeIndex - 1);
						}
					}
				});
				
				Rect levelUpRect = new Rect(m_offsetX, 3 * m_offsetY + m_buttonHeight * 2, m_buttonWidth, m_buttonHeight);
				//if (GUI.RepeatButton(levelUpRect, "Increase Level"))
				if (AccelerationButton.Display(levelUpRect, new GUIContent("Increase Level")))
				{
					m_controller.ModifyLevel(activeIndex, 1);
				}
				
				Rect levelDownRect = new Rect(bounds.width - m_offsetX - m_buttonWidth, 3 * m_offsetY + m_buttonHeight * 2, m_buttonWidth, m_buttonHeight);
				//if (GUI.RepeatButton(levelDownRect, "Decrease Level"))
				if (AccelerationButton.Display(levelDownRect, new GUIContent("Decrease Level")))
				{
					m_controller.ModifyLevel(activeIndex, -1);
				}
			});
			
			Rect acceptRect = new Rect(bounds.width - m_confirmOffsetX * 2 - m_confirmButtonWidth * 2, bounds.height - m_confirmOffsetY - m_confirmButtonHeight, 
				m_confirmButtonWidth, m_confirmButtonHeight);
			if (GUI.Button(acceptRect, "Accept"))
			{
				m_controller.ExitScene();
			}
			
			Rect cancelRect = new Rect(bounds.width - m_confirmOffsetX - m_confirmButtonWidth, bounds.height - m_confirmOffsetY - m_confirmButtonHeight,
				m_confirmButtonWidth, m_confirmButtonHeight);
			if (GUI.Button(cancelRect, "Cancel"))
			{
				m_controller.ExitScene(cancel: true);
			}
		});
	}
	
	public void OnEnter()
	{
	
	}
	
	public void OnLeave()
	{
	
	}
}

