using System;
using UnityEngine;

public class RosterState : IDisplayState
{
	int m_buttonWidth;
	int m_buttonHeight;
	
	const int NUM_ROWS = 3;
	const int NUM_COLUMNS = 2;
	
	PokemonListState m_listState;
	
	RosterController m_controller;
	
	Rect topScreen = new Rect(0, 0, 400, 240);
	Rect bottomScreen = new Rect(0, 260, 400, 240);
	
	public RosterState(RosterController controller)
	{
		m_buttonWidth = (int)(topScreen.width / NUM_COLUMNS);
		m_buttonHeight = (int)(topScreen.height / NUM_ROWS);
		
		m_controller = controller;
	}
	
	public void Update()
	{
	}
	
	public GUIStyle speciesStyle = new GUIStyle();
	public void Display()
	{
		GUI.BeginGroup(topScreen);
			for (int y = 0; y < NUM_ROWS; ++y)
			{
				for (int x = 0; x < NUM_COLUMNS; ++x)
				{
					int index = NUM_COLUMNS * y + x;
					if (PokemonRosterButton.Display(new Rect(x * m_buttonWidth, y * m_buttonHeight, m_buttonWidth, m_buttonHeight), m_controller.GetRosterSlot(index)))
					{
						m_controller.DisplaySlotOptions(index);
					}
				}
			}
		GUI.EndGroup();
		
		DrawBottomScreen();
		
	}
	
	void DrawBottomScreen()
	{
		GUIUtils.DrawBottomScreenBackground(bottomScreen);
		
		GUIUtils.DrawGroup(bottomScreen, delegate(Rect bounds) {
			int width = 100;
			int height = 40;
			if (GUI.Button(new Rect((bounds.width - width) / 2.0f, (bounds.height - height) / 2.0f, width, height), "Accept"))
			{
				m_controller.ExitScene();
			}
			
			
			if (GUI.Button(new Rect((bounds.width - width) / 2.0f, (bounds.height - height), width, height), "Exit"))
			{
				m_controller.ExitScene(cancel: true);
			}
		});
		
		GUI.BeginGroup(bottomScreen);
		GUI.EndGroup();
	}
	
	public void OnEnter()
	{
	
	}
	
	public void OnLeave()
	{
	
	}
}

