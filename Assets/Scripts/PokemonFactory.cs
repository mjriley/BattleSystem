using System;
using System.Linq;
using System.Collections.Generic;

using Abilities;

public class PokemonFactory
{
	public static Character CreatePokemon(Pokemon.Species species, uint level, string name="", Pokemon.Gender gender=Pokemon.Gender.Random, IAttackStrategy strategy=null)
	{
		PokemonDefinition definition = PokemonDefinition.GetEntry(species);
		
		if (name == "")
		{
			name = species.ToString();
		}
		
		if (gender == Pokemon.Gender.Random)
		{
			Random generator = new Random();
			double rand = generator.NextDouble() * 100;
			gender = (rand < definition.MaleRatio) ? Pokemon.Gender.Male : Pokemon.Gender.Female;
		}
		
		Character pokemon = new Character(name, definition, gender, level, strategy);
		
		for (int i = 0; i < 4; ++i)
		{
			string abilityName = definition.GetAbility(i);
			
			if (abilityName == "")
			{
				// If it's an empty string, there are no more abilities to fetch
				break;
			}
			
			//AbstractAbility ability = AbilityFactory.GetAbility(abilityName);
			Ability ability = AbilityFactory.GetAbility(abilityName);
			pokemon.addAbility(ability);
		}
		
		return pokemon;
	}
}
