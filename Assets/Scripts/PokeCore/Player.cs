using System.Collections.Generic;
using Items;

namespace PokeCore {

public class Player
{
	Inventory m_inventory;
	public Inventory Inventory { get { return m_inventory; } }
	
	string m_name;
	public string Name
	{
		get { return m_name; }
	}
	
	List<Character> m_pokemon = new List<Character>();
	public List<Character> Pokemon
	{
		get { return m_pokemon; }
	}
	
	//INextPokemonStrategy m_nextPokemonStrategy;
	
	int m_activePokemonIndex = 0;
	public Character ActivePokemon { get { return m_pokemon[m_activePokemonIndex]; } }
	public void setActivePokemon(int index)
	{
		m_activePokemonIndex = index;
	}
	
	IActionRequest m_turnRequest;
	IActionRequest m_replaceRequest;
	IActionRequest m_counterReplaceRequest;
	
	public Player(string name, IActionRequest turnRequest, IActionRequest replaceRequest, IActionRequest counterReplaceRequest)
	{
		m_name = name;
		//m_nextPokemonStrategy = nextPokemonStrategy;
		m_turnRequest = turnRequest;
		m_replaceRequest = replaceRequest;
		m_counterReplaceRequest = counterReplaceRequest;
		
		m_inventory = new Inventory();
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
	
//	public int GetNextPokemon(Player enemy)
//	{
//		return m_nextPokemonStrategy.getNextPokemon(this, enemy);
//	}
	
//	public int GetReplacementPokemon()
//	{
//		return 0;
//	}
//	
//	public int GetCounterPokemon()
//	{
//		return 0;
//	}
	
	public void GetTurnAction(Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		m_turnRequest.GetAction(this, enemyPlayer, actionCallback);
	}
	
	public void GetNextPokemon(Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		m_replaceRequest.GetAction(this, enemyPlayer, actionCallback);
	}
	
	public void GetCounterPokemon(Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		m_counterReplaceRequest.GetAction(this, enemyPlayer, actionCallback);
	}
	
	public bool IsDefeated()
	{
		Character alivePokemon = m_pokemon.Find(p => !p.isDead()); 
		return (alivePokemon == null);
	}
}

}