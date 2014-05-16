using System;
using System.Collections.Generic;
using UnityEngine;

public class PokemonRosterButton
{
	static bool isInit = false;
	static Dictionary<Pokemon.Species, Texture2D> textures;
	static Texture2D solidColor;
	static Texture2D m_background;
	static GUIStyle m_emptyStyle;
	static GUIStyle m_speciesStyle;
	
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
			
			m_background = Resources.Load<Texture2D>("Textures/PokemonRosterButton");
			solidColor = Resources.Load<Texture2D>("Textures/white_tile");
			textures = new Dictionary<Pokemon.Species, Texture2D>();
			foreach (Pokemon.Species species in Enum.GetValues(typeof(Pokemon.Species)))
			{
				if (species != Pokemon.Species.None)
				{
					LoadPokemonThumbnail(species);
				}
			}
			
			isInit = true;
		}
	}
		
	public static bool Display(Rect rect, Pokemon.Species species, GUIStyle speciesStyle=null)
	{
		Init();
		
		if (speciesStyle == null)
		{
			speciesStyle = m_speciesStyle;
		}
		
		Texture2D speciesTex = textures[species];
		
		GUI.BeginGroup(rect);
			Rect parentBounds = new Rect(0.0f, 0.0f, m_background.width, m_background.height);
			bool result = GUI.Button(parentBounds, m_background, m_emptyStyle);
			//GUI.DrawTexture(new Rect(0.0f, 0.0f, speciesTex.width, speciesTex.height), solidColor);
			GUI.DrawTexture(new Rect(10.0f, 10.0f, speciesTex.width, speciesTex.height), speciesTex);
			GUI.Label(new Rect(42.0f, 10.0f, 100.0f, 30.0f), species.ToString(), speciesStyle);
		GUI.EndGroup();
		
		return result;
	}
}
