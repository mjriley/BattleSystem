using System;
using System.Collections.Generic;
using UnityEngine;
using Pokemon;

public class PokemonRosterButton
{
	static bool isInit = false;
	static Dictionary<Pokemon.Species, Texture2D> textures;
	static Texture2D m_background;
	static Texture2D m_highlight;
	static Texture2D m_frame;
	static GUIStyle m_emptyStyle;
	static GUIStyle m_speciesStyle;
	static GUIStyle m_levelStyle;
	
	static int borderX;
	static int borderY;
	
	static void LoadPokemonThumbnail(Pokemon.Species species)
	{
		textures[species] = Resources.Load<Texture2D>("Textures/PokemonThumbnails/" + species);
	}
	
	static void Init()
	{
		if (!isInit)
		{
			m_emptyStyle = new GUIStyle();
			
			m_speciesStyle = new GUIStyle();
			m_speciesStyle.fontSize = 18;
			
			m_levelStyle = new GUIStyle();
			m_levelStyle.fontSize = 10;
			m_levelStyle.alignment = TextAnchor.UpperCenter;
			
			m_background = Resources.Load<Texture2D>("Textures/PokemonRosterButton");
			m_highlight = Resources.Load<Texture2D>("Textures/PokemonRosterButtonHighlight");
			m_frame = Resources.Load<Texture2D>("Textures/PokemonRosterButtonFrame");
			textures = new Dictionary<Pokemon.Species, Texture2D>();
			foreach (Pokemon.Species species in Enum.GetValues(typeof(Pokemon.Species)))
			{
				if (species != Pokemon.Species.None)
				{
					LoadPokemonThumbnail(species);
				}
			}
			
			borderX = (m_highlight.width - m_background.width) / 2;
			borderY = (m_highlight.height - m_background.height) / 2;
			
			isInit = true;
		}
	}
	
	public static Vector2 CalcSize()
	{
		Init();
		
		return new Vector2(m_highlight.width, m_highlight.height);
	}
		
	public static bool Display(Rect rect, PokemonPrototype prototype, bool isSelected=false, GUIStyle speciesStyle=null)
	{
		Init();
		
		if (speciesStyle == null)
		{
			speciesStyle = m_speciesStyle;
		}
		
		GUI.BeginGroup(rect);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, m_frame.width, m_frame.height), m_frame);
			Rect parentBounds = new Rect(borderX, borderY, m_background.width, m_background.height);
			bool result = false;
			
			if (prototype != null)
			{
				Texture2D speciesTex = textures[prototype.Species];
				result = GUI.Button(parentBounds, m_background, m_emptyStyle);
				GUI.DrawTexture(new Rect(10.0f, 10.0f, speciesTex.width, speciesTex.height), speciesTex);
				GUI.Label(new Rect(42.0f, 10.0f, 100.0f, 30.0f), prototype.Species.ToString(), speciesStyle);
				GUI.Label(new Rect(rect.width - 100, 2.0f, 100, 20), "Lv. " + prototype.Level, m_levelStyle);
			}
			
			if (isSelected)
			{
				GUI.DrawTexture(new Rect(0.0f, 0.0f, m_highlight.width, m_highlight.height), m_highlight);
			}
		GUI.EndGroup();
		
		return result;
	}
}
