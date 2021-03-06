﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PokeCore.Pokemon;

namespace PokemonGUI {

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
		roster[0] = new PokemonPrototype(Species.Bulbasaur, 50, Gender.Male);
		roster[1] = new PokemonPrototype(Species.Pikachu, 50, Gender.Male);
		roster[2] = new PokemonPrototype(Species.Charmander, 50, Gender.Male);
		roster[3] = new PokemonPrototype(Species.Chespin, 50, Gender.Male);
		roster[4] = new PokemonPrototype(Species.Froakie, 50, Gender.Male);
		roster[5] = new PokemonPrototype(Species.Squirtle, 50, Gender.Male);
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
		//PlayerPrefs.DeleteAll();
		string serializedRoster = PlayerPrefs.GetString(ROSTER_KEY);
		byte[] bytes = Convert.FromBase64String(serializedRoster);
		MemoryStream stream = new MemoryStream();
		stream.Write(bytes, 0, bytes.Length);
		stream.Seek(0, SeekOrigin.Begin);
		
		BinaryFormatter serializer = new BinaryFormatter();
		roster = (PokemonPrototype[])serializer.Deserialize(stream);
	}
}

}