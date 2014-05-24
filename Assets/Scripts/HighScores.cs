using System;
using System.Linq;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScores
{
	const int NUMBER_SCORES = 10;
	List<HighScore> m_highScores;
	
	public HighScores()
	{
		m_highScores = new List<HighScore>();
	}
	
	public bool IsHighScore(int wins)
	{
		if (m_highScores.Count < NUMBER_SCORES)
		{
			return true;
		}
		
		foreach (HighScore score in m_highScores)
		{
			if (wins > score.Wins)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void InsertScore(HighScore score)
	{
		m_highScores.Add(score);
		m_highScores.Sort(delegate(HighScore x, HighScore y) { return y.CompareTo(x); });
		
		m_highScores = m_highScores.Take(NUMBER_SCORES).ToList();
	}
	
	public List<HighScore> GetScores()
	{
		return m_highScores;
	}
	
	const string SCORE_KEY = "HighScores";
	public void Save()
	{
		MemoryStream stream = new MemoryStream();
		BinaryFormatter serializer = new BinaryFormatter();
		
		serializer.Serialize(stream, m_highScores);
		string serializedScores = Convert.ToBase64String(stream.ToArray());
		
		PlayerPrefs.SetString(SCORE_KEY, serializedScores);
	}
	
	public void Load()
	{
		if (!PlayerPrefs.HasKey(SCORE_KEY))
		{
			return;
		}
		
		string serializedScores = PlayerPrefs.GetString(SCORE_KEY);
		byte[] bytes = Convert.FromBase64String(serializedScores);
		MemoryStream stream = new MemoryStream();
		stream.Write(bytes, 0, bytes.Length);
		stream.Seek(0, SeekOrigin.Begin);
		
		BinaryFormatter serializer = new BinaryFormatter();
		m_highScores = (List<HighScore>)serializer.Deserialize(stream);
	}
}

[Serializable]
public class HighScore : IComparable<HighScore>
{
	public string Name { get; set; }
	public int Wins { get; set; }
	public List<Pokemon.Species> Pokemon { get; set; }
	
	public static HighScore CreateFromPlayer(Player player, int wins)
	{
		List<Pokemon.Species> pokemon = new List<Pokemon.Species>();
		foreach (Character character in player.Pokemon)
		{
			pokemon.Add(character.Species);
		}
		
		return new HighScore(player.Name, wins, pokemon);
	}
	
	public HighScore(string name, int wins, List<Pokemon.Species> pokemon)
	{
		Name = name;
		Wins = wins;
		Pokemon = pokemon;
	}
	
	public int CompareTo(HighScore other)
	{
		return this.Wins.CompareTo(other.Wins);
	}
}