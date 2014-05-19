using System.Collections.Generic;
using System;

[Serializable]
public class PokemonPrototype
{
	public Pokemon.Species Species { get; set; }
	public uint Level { get; set; }
	public List<uint> Abilities { get; set; }
	public Pokemon.Gender Gender { get; set; }
	
	public PokemonPrototype(Pokemon.Species species, uint level, Pokemon.Gender gender, List<uint> abilityIndices=null)
	{
		Species = species;
		Level = level;
		Gender = gender;
		
		if (abilityIndices == null)
		{
			abilityIndices = new List<uint> { 0, 1, 2, 3 };
		}
		
		Abilities = abilityIndices;
	}

}