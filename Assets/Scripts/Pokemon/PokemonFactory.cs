using System;
using System.Linq;
using System.Collections.Generic;

using Abilities;
using PokeCore;

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
			string abilityName = definition.GetAbility(i);
			
			if (abilityName == "")
			{
				// If it's an empty string, there are no more abilities to fetch
				break;
			}
			
			Ability ability = AbilityFactory.GetAbility(abilityName);
			pokemon.addAbility(ability);
		}
		
		return pokemon;
	}
}

}
