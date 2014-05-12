using UnityEngine;

public class PlayerRoster : MonoBehaviour
{
	public Pokemon.Species[] roster;

	public void Awake()
	{
		DontDestroyOnLoad(this);
	}
	
	public Pokemon.Species GetRosterSlot(int slot)
	{
		return roster[slot];
	}
	
	public void SetRosterSlot(int slot, Pokemon.Species species)
	{
		roster[slot] = species; 
	}
}
