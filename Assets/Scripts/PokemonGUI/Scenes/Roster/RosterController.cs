using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PokeCore.Pokemon;

using PokemonGUI;

namespace PokemonGUI {
namespace Scenes {
namespace Roster {

public class RosterController
{
	IDisplayState m_currentState;
	
	RosterState m_rosterState;
	PokemonListState m_optionState;
	PokemonDetailsState m_detailsState;
	
	PlayerRoster m_rosterModel;
	
	List<PokemonPrototype> m_roster;
	
	
	public RosterController()
	{
		m_rosterState = new RosterState(this);
		m_optionState = new PokemonListState(this);
		m_detailsState = new PokemonDetailsState(this);
		
		m_currentState = m_rosterState;
		
		m_rosterModel = GameObject.Find("PlayerRoster").GetComponent<PlayerRoster>();
		InitRoster();
	}
	
	void InitRoster()
	{
		m_roster = new List<PokemonPrototype>();
		for (int i=0; i<6; ++i)
		{
			PokemonPrototype prototype = m_rosterModel.GetRosterSlot(i);
			if (prototype != null)
			{
				m_roster.Add(prototype);
			}
		}
	}
	
	void CommitChoices()
	{
		m_rosterModel.roster = m_roster.ToArray();
		m_rosterModel.Save();
	}
	
	void ChangeState(IDisplayState newState)
	{
		m_currentState.OnLeave();
		
		m_currentState = newState;
		
		m_currentState.OnEnter();
	}
	
	public PokemonPrototype GetRosterSlot(int slot)
	{
		if (slot >= m_roster.Count)
		{
			return null;
		}
		
		return m_roster[slot];
	}
	
	public Species[] GetSlotOptions(int slot)
	{
		Species[] species = (Species[])Enum.GetValues(typeof(Species));
		
		if (GetRosterSlot(slot) != null)
		{
			return species.Where(x => x != Species.None).ToArray();
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
	
	public void SetSlotValue(int slot, PokemonPrototype prototype)
	{
		// None is never added to the roster
		if (prototype != null)
		{
			if (slot >= m_roster.Count)
			{
				m_roster.Add(prototype);
			}
			else
			{
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
	
	public void DisplayDetails(int slot)
	{
		m_detailsState.SetActiveSlot(slot);
		ChangeState(m_detailsState);
	}
	
	public void DisplayRoster()
	{
		ChangeState(m_rosterState);
	}
	
	public void ExitScene(bool cancel=false)
	{
		if (!cancel)
		{
			CommitChoices();
		}
		
		Application.LoadLevel("Start");
	}
}

}}}