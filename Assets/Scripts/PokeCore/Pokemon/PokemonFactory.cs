using System;
using System.Linq;
using System.Collections.Generic;

namespace PokeCore {
	using Moves;
namespace Pokemon {

public class PokemonFactory
{
	public static Character CreatePokemon(Species species, uint level, string name="", Gender gender=Gender.Random)
	{
		PokemonDefinition definition = PokemonDefinition.GetEntry(species);
		
		if (name == "")
		{
			name = species.ToString();
		}
		
		if (gender == Gender.Random)
		{
			Random generator = new Random();
			double rand = generator.NextDouble() * 100;
			gender = (rand < definition.MaleRatio) ? Gender.Male : Gender.Female;
		}
		
		Character pokemon = new Character(name, definition, gender, level);
		
		for (int i = 0; i < 4; ++i)
		{
			string moveName = definition.GetMove(i);
			
			if (moveName == "")
			{
				// If it's an empty string, there are no more moves to fetch
				break;
			}
			
			Move move = MoveFactory.GetMove(moveName);
			pokemon.addMove(move);
		}
		
		return pokemon;
	}
}

}}
