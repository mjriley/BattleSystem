using System;
using UnityEngine;

namespace Pokemon {

public class Pokemon
{
	public static int GetStat(Species species, Stat stat, uint level)
	{
		PokemonDefinition defintion = PokemonDefinition.GetEntry(species);
		int baseStat = defintion.GetStat(stat);
		
		int factor = (stat == Stat.HP) ? 100 : 0;
		int baseValue = (stat == Stat.HP) ? 10 : 5;
		
		return (2 * baseStat + factor) * (int)level / 100 + baseValue;
	}
	
	public static Texture2D GetThumbnail(Species species)
	{
		return Resources.Load<Texture2D>("Textures/PokemonThumbnails/" + Enum.GetName(typeof(Species), species).ToLower());
	}
}

}
