using System.Collections.Generic;

namespace PokeCore {

public class Player
{
	private string m_name;
	public string Name
	{
		get { return m_name; }
	}
	
	private List<Character> m_pokemon = new List<Character>();
	public List<Character> Pokemon
	{
		get { return m_pokemon; }
	}
	
	private INextPokemonStrategy m_nextPokemonStrategy;
	
	private int m_activePokemonIndex = 0;
	public Character ActivePokemon { get { return m_pokemon[m_activePokemonIndex]; } }
	public void setActivePokemon(int index)
	{
		m_activePokemonIndex = index;
	}
	
	public Player(string name, INextPokemonStrategy nextPokemonStrategy)
	{
		m_name = name;
		m_nextPokemonStrategy = nextPokemonStrategy;
	}
	
	public void AddPokemon(Character pokemon)
	{
		pokemon.Owner = this;
		m_pokemon.Add(pokemon);
	}
	
	public void RestorePokemon()
	{
		foreach (Character pokemon in m_pokemon)
		{
			pokemon.Reset();
		}
	}
	
	public int GetNextPokemon(Player enemy)
	{
		return m_nextPokemonStrategy.getNextPokemon(this, enemy);
	}
	
	public ITurnAction GetTurnAction()
	{
		return ActivePokemon.getTurn();
	}
	
	public void UpdateConditions(Player enemy)
	{
		ActivePokemon.UpdateBattleConditions(enemy);
	}
	
	public bool IsDefeated()
	{
		Character alivePokemon = m_pokemon.Find(p => !p.isDead()); 
		return (alivePokemon == null);
	}
}

}