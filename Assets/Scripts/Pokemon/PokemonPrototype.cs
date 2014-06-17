using System.Collections.Generic;
using System;

namespace Pokemon {

[Serializable]
public class PokemonPrototype
{
	public Species Species { get; set; }
	public uint Level { get; set; }
	public List<uint> Moves { get; set; }
	public Gender Gender { get; set; }
	
	public PokemonPrototype(Species species, uint level, Gender gender, List<uint> moveIndices=null)
	{
		Species = species;
		Level = level;
		Gender = gender;
		
		if (moveIndices == null)
		{
			moveIndices = new List<uint> { 0, 1, 2, 3 };
		}
		
		Moves = moveIndices;
	}
}

}