using System;
using UnityEngine;

	public class Pokemon
	{
		public enum Species
		{
			Bulbasaur,
			Charmander,
			Chespin,
			Magikarp,
			Pikachu,
			Squirtle
		};
		
		public static Texture2D GetThumbnail(Species species)
		{
			return Resources.Load<Texture2D>("Textures/PokemonThumbnails/" + Enum.GetName(typeof(Species), species).ToLower());
		}
	}

