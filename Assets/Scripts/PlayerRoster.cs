using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerRoster : MonoBehaviour
{
	const string ROSTER_KEY = "Roster";
	const int MAX_ROSTER_SLOTS = 6;
	public PokemonPrototype[] roster;

	public void Awake()
	{
		DontDestroyOnLoad(this);
		
		if (PlayerPrefs.HasKey(ROSTER_KEY))
		{
			Load();
		}
		else
		{
			CreateDefaultRoster();
		}
	}
	
	void CreateDefaultRoster()
	{
		roster = new PokemonPrototype[6];
		roster[0] = new PokemonPrototype(Pokemon.Species.Bulbasaur, 50, Pokemon.Gender.Male);
		roster[1] = new PokemonPrototype(Pokemon.Species.Pikachu, 50, Pokemon.Gender.Male);
		roster[2] = new PokemonPrototype(Pokemon.Species.Charmander, 50, Pokemon.Gender.Male);
		roster[3] = new PokemonPrototype(Pokemon.Species.Chespin, 50, Pokemon.Gender.Male);
		roster[4] = new PokemonPrototype(Pokemon.Species.Froakie, 50, Pokemon.Gender.Male);
		roster[5] = new PokemonPrototype(Pokemon.Species.Squirtle, 50, Pokemon.Gender.Male);
	}
	
	public PokemonPrototype GetRosterSlot(int slot)
	{
		if (slot >= roster.Length)
		{
			return null;
		}
		
		return roster[slot];
	}
	
	public void SetRosterSlot(int slot, PokemonPrototype prototype)
	{
		roster[slot] = prototype; 
	}
	
	
	public void Save()
	{
		MemoryStream stream = new MemoryStream();
		BinaryFormatter serializer = new BinaryFormatter();
		
		serializer.Serialize(stream, roster);
		string serializedRoster = Convert.ToBase64String(stream.ToArray());
		
		PlayerPrefs.SetString(ROSTER_KEY, serializedRoster);
	}
	
	void Load()
	{
		string serializedRoster = PlayerPrefs.GetString(ROSTER_KEY);
		byte[] bytes = Convert.FromBase64String(serializedRoster);
		MemoryStream stream = new MemoryStream();
		stream.Write(bytes, 0, bytes.Length);
		stream.Seek(0, SeekOrigin.Begin);
		
		BinaryFormatter serializer = new BinaryFormatter();
		roster = (PokemonPrototype[])serializer.Deserialize(stream);
	}
}
