using System;
using System.Collections.Generic;
using UnityEngine;

public class PokemonRosterButton
{
	static bool isInit = false;
	static Dictionary<Pokemon.Species, Texture2D> textures;
	static Texture2D solidColor;
	
	static void LoadPokemonThumbnail(Pokemon.Species species)
	{
		textures[species] = Resources.Load<Texture2D>("Textures/PokemonThumbnails/" + species);
	}
	
	static void Init()
	{
		if (!isInit)
		{
			solidColor = Resources.Load<Texture2D>("Textures/white_tile");
			textures = new Dictionary<Pokemon.Species, Texture2D>();
			foreach (Pokemon.Species species in Enum.GetValues(typeof(Pokemon.Species)))
			{
				if (species != Pokemon.Species.None)
				{
					LoadPokemonThumbnail(species);
				}
			}
//			LoadPokemonThumbnail(Pokemon.Species.Pikachu);
//			LoadPokemonThumbnail(Pokemon.Species.Bulbasaur);
//			LoadPokemonThumbnail(Pokemon.Species.Charmander);
//			LoadPokemonThumbnail(Pokemon.Species.Chespin);
//			LoadPokemonThumbnail(Pokemon.Species.Magikarp);
//			LoadPokemonThumbnail(Pokemon.Species.Squirtle);
			
			isInit = true;
		}
	}
		
	public static bool Display(Rect rect, Pokemon.Species species)
	{
		Init();
		
		GUI.BeginGroup(rect);
			Rect parentBounds = new Rect(0.0f, 0.0f, rect.width, rect.height);
			bool result = GUI.Button(parentBounds, "");
			GUI.DrawTexture(new Rect(0.0f, 0.0f, 64, 64), solidColor);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, 64, 64), textures[species]);
		GUI.EndGroup();
		
		return result;
	}
}
