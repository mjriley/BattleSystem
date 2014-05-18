using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RosterController
{
	IDisplayState m_currentState;
	
	StartState m_startState;
	RosterState m_rosterState;
	PokemonListState m_optionState;
	
	PlayerRoster m_rosterModel;
	
	//List<Pokemon.Species> m_roster;
	List<PokemonPrototype> m_roster;
	
	
	public RosterController()
	{
		m_rosterState = new RosterState(this);
		m_optionState = new PokemonListState(this);
		
		m_currentState = m_rosterState;
		
		m_rosterModel = GameObject.Find("PlayerRoster").GetComponent<PlayerRoster>();
		InitRoster();
	}
	
	void InitRoster()
	{
		//m_roster = new List<Pokemon.Species>();
		m_roster = new List<PokemonPrototype>();
		for (int i=0; i<6; ++i)
		{
			m_roster.Add(m_rosterModel.GetRosterSlot(i));
		}
	}
	
	void CommitChoices()
	{
		m_rosterModel.roster = m_roster.ToArray();
	}
	
	void ChangeState(IDisplayState newState)
	{
		m_currentState.OnLeave();
		
		m_currentState = newState;
		
		m_currentState.OnEnter();
	}
	
	//public Pokemon.Species GetRosterSlot(int slot)
	public PokemonPrototype GetRosterSlot(int slot)
	{
		if (slot >= m_roster.Count)
		{
			//return Pokemon.Species.None;
			return null;
		}
		
		return m_roster[slot];
	}
	
	public Pokemon.Species[] GetSlotOptions(int slot)
	{
		Pokemon.Species[] species = (Pokemon.Species[])Enum.GetValues(typeof(Pokemon.Species));
		
		//if (GetRosterSlot(slot) != Pokemon.Species.None)
		if (GetRosterSlot(slot) != null)
		{
			return species.Where(x => x != Pokemon.Species.None).ToArray();
		}
		
		return species;
	}
	
	public void RemovePokemonAtSlot(int slot)
	{
		m_roster.RemoveAt(slot);
	}
	
	public int GetRosterSize()
	{
		return m_roster.Count;
	}
	
	public void DisplaySlotOptions(int slot)
	{
		m_optionState.SetActiveSlot(slot);
		ChangeState(m_optionState);
	}
	
	//public void SetSlotValue(int slot, Pokemon.Species species)
	public void SetSlotValue(int slot, PokemonPrototype prototype)
	{
		// None is never added to the roster
		//if (species != Pokemon.Species.None)
		if (prototype != null)
		{
			if (slot >= m_roster.Count)
			{
				//m_roster.Add(species);
				m_roster.Add(prototype);
			}
			else
			{
				//m_roster[slot] = species;
				m_roster[slot] = prototype;
			}
		}
		
		ChangeState(m_rosterState);
	}
	
	public void ModifyLevel(int slot, int increase)
	{
		PokemonPrototype prototype = GetRosterSlot(slot);
		
		if (prototype != null)
		{
			int newLevel = (int)prototype.Level + increase;
			
			newLevel = Mathf.Min(newLevel, 100);
			newLevel = Mathf.Max(newLevel, 1);
			
			prototype.Level = (uint)newLevel;
		}
	}
	
	public IDisplayState GetActiveDisplay()
	{
		return m_currentState;
	}
	
	public void ExitScene(bool cancel=false)
	{
		if (!cancel)
		{
			CommitChoices();
		}
		
		Application.LoadLevel("start_menu");
	}
}

