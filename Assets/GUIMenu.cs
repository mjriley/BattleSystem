﻿using UnityEngine;
using System.Collections;
using System;

public class GUIMenu : MonoBehaviour
{
	public float height = 70.0f;
	public int fontHeight = 20;
	public int fontWidth = 70;
	public GUIStyle optionsStyle;
	
	int optionSelectedIndex = 0;
	
	Rect m_bottomScreen = new Rect(0.0f, 260.0f, 400.0f, 240.0f);
	
	enum Options : int
	{
		Battle = 0,
		Roster
	}
	
	Options[] m_options = (Options[])Enum.GetValues(typeof(Options));
	
	void Start()
	{
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (m_options[optionSelectedIndex] == Options.Battle)
			{
				Application.LoadLevel("battle");
			}
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
		GUIUtils.DrawSeparatorBar();
		DrawBottomScreen();
	}
	
	void DrawBottomScreen()
	{
		GUIUtils.DrawBottomScreenBackground(m_bottomScreen);
		
		GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds)
		{
			GUIContent cursor = new GUIContent("➤");
			Vector2 textBounds = optionsStyle.CalcSize(cursor);
			
			float textVerticalSize = textBounds.y * m_options.Length;
			float textVerticalStart = (bounds.height - textVerticalSize) / 2.0f;
			
			for (int i=0; i < m_options.Length; ++i)
			{
				GUI.Label(new Rect((bounds.width - fontWidth) / 2.0f, textVerticalStart + i * textBounds.y, fontWidth, textBounds.y), m_options[i].ToString(), optionsStyle);
			}
			
			GUI.Label(new Rect((Screen.width - fontWidth) / 2.0f - textBounds.x, textVerticalStart + optionSelectedIndex * textBounds.y, textBounds.x, textBounds.y), cursor, optionsStyle);
		});
	}

}
