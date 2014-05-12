using UnityEngine;

public class StartState : IDisplayState
{
	RosterController m_controller;
	
	Rect topScreen = new Rect(0, 0, 400, 240);
	
	string[] m_options = new string[] { "Battle!", "Roster" };
	int m_optionSelectedIndex = 0;
	
	float height = 70.0f;
	int fontHeight = 20;
	int fontWidth = 70;
	GUIStyle optionsStyle;
	
	GameObject m_logoObject;
	
	public StartState(RosterController controller)
	{
		m_controller = controller;
		
		optionsStyle = new GUIStyle();
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			m_optionSelectedIndex = (m_optionSelectedIndex + 1) % m_options.Length;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_optionSelectedIndex = m_optionSelectedIndex - 1;
			if (m_optionSelectedIndex < 0)
			{
				m_optionSelectedIndex = m_options.Length + m_optionSelectedIndex;
			}
		}
	}
	
	public void Display()
	{
		GUI.BeginGroup(topScreen);
			Rect parentRect = new Rect(0.0f, topScreen.height - height, topScreen.width, height);
			
			GUI.BeginGroup(parentRect);
				for (int i=0; i < m_options.Length; ++i)
				{
					GUI.Label(new Rect((Screen.width - fontWidth) / 2.0f, i * fontHeight, fontWidth, fontHeight), m_options[i], optionsStyle);
				}
				
				GUI.Label(new Rect((Screen.width - fontWidth) / 2.0f - 20.0f, m_optionSelectedIndex * 20.0f, 20.0f, fontHeight), "➤", optionsStyle);
			GUI.EndGroup();
		GUI.EndGroup();
	}
	
	public void OnEnter()
	{
	
	}
	
	public void OnLeave()
	{
	
	}
}
