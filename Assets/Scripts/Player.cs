using System.Collections.Generic;

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
	
	public Player(string name)
	{
		m_name = name;
	}
	
	public void AddPokemon(Character pokemon)
	{
		m_pokemon.Add(pokemon);
	}
}
