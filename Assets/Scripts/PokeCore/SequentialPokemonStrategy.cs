using System.Collections.Generic;

namespace PokeCore {

public class SequentialPokemonStrategy : INextPokemonStrategy
{
	public SequentialPokemonStrategy()
	{
	}
	
	public int getNextPokemon(Player subject, Player enemy)	
	{
		List<Character> myPokemon = subject.Pokemon;
		int index = myPokemon.IndexOf(subject.ActivePokemon);
		
		int nextIndex = (index + 1) % myPokemon.Count;
		
		// the current active pokemon cannot be our next pokemon,
		// but every other pokemon can. Return the first non-dead pokemon we find
		// in these count - 1 pokemon
		for (int i = 0; i < myPokemon.Count - 1; ++i)
		{
			if (!myPokemon[nextIndex].isDead())
			{
				return nextIndex;
			}
			
			nextIndex = (nextIndex + 1) % myPokemon.Count;
		}
		
		return -1;
	}
}

}