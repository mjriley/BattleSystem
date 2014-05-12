using UnityEngine;
using System.Collections;
using System;

public class GUIMenu : MonoBehaviour
{
	public float height = 70.0f;
	public int fontHeight = 20;
	public int fontWidth = 70;
	public GUIStyle optionsStyle;
	
	int optionSelectedIndex = 0;
	
	enum Options : int
	{
		Battle = 0,
		Roster
	}
	
	Options[] m_options = (Options[])Enum.GetValues(typeof(Options));
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (m_options[optionSelectedIndex] == Options.Roster)
			{
				Application.LoadLevel("roster");
			}
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			optionSelectedIndex = (optionSelectedIndex + 1) % m_options.Length;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			optionSelectedIndex = optionSelectedIndex - 1;
			if (optionSelectedIndex < 0)
			{
				optionSelectedIndex = m_options.Length + optionSelectedIndex;
			}
		}
	}
	public void OnGUI()
	{
		Rect parentRect = new Rect(0.0f, Screen.height - height, Screen.width, height);
		
		GUI.BeginGroup(parentRect);
			for (int i=0; i < m_options.Length; ++i)
			{
				GUI.Label(new Rect((Screen.width - fontWidth) / 2.0f, i * fontHeight, fontWidth, fontHeight), m_options[i].ToString(), optionsStyle);
			}
			
			GUI.Label(new Rect((Screen.width - fontWidth) / 2.0f - 20.0f, optionSelectedIndex * 20.0f, 20.0f, fontHeight), "➤", optionsStyle);
		GUI.EndGroup();
	}

}
