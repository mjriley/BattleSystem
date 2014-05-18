using UnityEngine;

public class PlayerRoster : MonoBehaviour
{
	//public Pokemon.Species[] roster;
	public PokemonPrototype[] roster;

	public void Awake()
	{
		DontDestroyOnLoad(this);
		
		roster = new PokemonPrototype[6];
		roster[0] = new PokemonPrototype(Pokemon.Species.Bulbasaur, 50, Pokemon.Gender.Male);
		roster[1] = new PokemonPrototype(Pokemon.Species.Pikachu, 50, Pokemon.Gender.Male);
		roster[2] = new PokemonPrototype(Pokemon.Species.Charmander, 50, Pokemon.Gender.Male);
		roster[3] = new PokemonPrototype(Pokemon.Species.Chespin, 50, Pokemon.Gender.Male);
		roster[4] = new PokemonPrototype(Pokemon.Species.Froakie, 50, Pokemon.Gender.Male);
		roster[5] = new PokemonPrototype(Pokemon.Species.Squirtle, 50, Pokemon.Gender.Male);
	}
	
	//public Pokemon.Species GetRosterSlot(int slot)
	public PokemonPrototype GetRosterSlot(int slot)
	{
		return roster[slot];
	}
	
	//public void SetRosterSlot(int slot, Pokemon.Species species)
	public void SetRosterSlot(int slot, PokemonPrototype prototype)
	{
		roster[slot] = prototype; 
	}
}
