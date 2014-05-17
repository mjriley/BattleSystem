using System;
using UnityEngine;

	public class Pokemon
	{
		public enum Species
		{
			Chespin,
			Quilladin,
			Chesnaught,
			Fennekin,
			Braixen,
			Delphox,
			Froakie,
			Frogadier,
			Greninja,
			Bunnelby,
			Diggersby,
			Zigzagoon,
			Bulbasaur,
			Charmander,
			Magikarp,
			Pikachu,
			Squirtle,
			None
		};
		
		public enum Gender
		{
			Male,
			Female,
			Random
		};
		
		public static Texture2D GetThumbnail(Species species)
		{
			return Resources.Load<Texture2D>("Textures/PokemonThumbnails/" + Enum.GetName(typeof(Species), species).ToLower());
		}
	}

