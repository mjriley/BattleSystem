using UnityEngine;
using System.Collections;
using System;


namespace PokemonGUI {
namespace Scenes {
namespace Start {

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
		Roster,
		Leaderboard
	}
	
	Options[] m_options = (Options[])Enum.GetValues(typeof(Options));
	
	StarTransition m_transition;
	bool m_isExiting;
	Texture2D m_solidTexture;
	
	void Awake()
	{
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
	}
	
	void Start()
	{
		m_transition = GetComponent<StarTransition>();
		m_isExiting = false;
	}
	
	public void Update()
	{
		if (m_isExiting && !m_transition.IsAnimating)
		{
			Application.LoadLevel("Battle");
		}
		
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (m_options[optionSelectedIndex] == Options.Battle)
			{
				m_isExiting = true;
				m_transition.enabled = true;
			}
			if (m_options[optionSelectedIndex] == Options.Roster)
			{
				Application.LoadLevel("Roster");
			}
			else if (m_options[optionSelectedIndex] == Options.Leaderboard)
			{
				Application.LoadLevel("Scores");
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
		
		if (m_isExiting)
		{
			DrawBlankBottomScreen();
		}
		else
		{
			DrawBottomScreen();
		}
	}
	
	public int optionOffsetX = 10;
	public int optionPaddingY = 10;
	void DrawBottomScreen()
	{
		GUIUtils.DrawBottomScreenBackground(ScreenCoords.BottomScreen);
		
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUIContent cursor = new GUIContent("➤");
			Vector2 textBounds = optionsStyle.CalcSize(cursor);
			
			float textVerticalSize = (textBounds.y + optionPaddingY) * m_options.Length - optionPaddingY;
			float textVerticalStart = (bounds.height - textVerticalSize) / 2.0f;
			
			for (int i=0; i < m_options.Length; ++i)
			{
				float y = textVerticalStart + i * (textBounds.y + optionPaddingY);
				GUI.Label(new Rect((bounds.width - fontWidth) / 2.0f + optionOffsetX, y, fontWidth, textBounds.y), m_options[i].ToString(), optionsStyle);
				if (i == optionSelectedIndex)
				{
					GUI.Label(new Rect((bounds.width - fontWidth) / 2.0f - textBounds.x, y, textBounds.x, textBounds.y), cursor, optionsStyle);
				}
			}
		});
	}
	
	void DrawBlankBottomScreen()
	{
		Color prevColor = GUI.color;
		GUI.color = Color.black;
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_solidTexture);
		GUI.color = prevColor;
	}

}

}}}