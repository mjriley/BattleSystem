using PokeCore;

public class InputPokemonStrategy : INextPokemonStrategy
{
	public delegate int GetNextPokemon();
	
	GetNextPokemon m_callback;
	
	public InputPokemonStrategy(GetNextPokemon callback)
	{
		m_callback = callback;
	}
	
	public int getNextPokemon(Player subject, Player enemy)
	{
		return m_callback();
	}
}
