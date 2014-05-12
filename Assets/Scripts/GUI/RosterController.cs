using System;
using System.Linq;
using UnityEngine;

public class RosterController
{
	IDisplayState m_currentState;
	
	StartState m_startState;
	RosterState m_rosterState;
	PokemonListState m_optionState;
	
	PlayerRoster m_rosterModel;
	
	Pokemon.Species[] m_roster;
	
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
		m_roster = new Pokemon.Species[6];
		for (int i=0; i<6; ++i)
		{
			m_roster[i] = m_rosterModel.GetRosterSlot(i);
		}
	}
	
	void CommitChoices()
	{
		for (int i=0; i<6; ++i)
		{
			m_rosterModel.SetRosterSlot(i, m_roster[i]);
		}
	}
	
	void ChangeState(IDisplayState newState)
	{
		m_currentState.OnLeave();
		
		m_currentState = newState;
		
		m_currentState.OnEnter();
	}
	
	public Pokemon.Species GetRosterSlot(int slot)
	{
		//return m_roster.GetRosterSlot(slot);
		return m_roster[slot];
	}
	
	public void DisplaySlotOptions(int slot)
	{
		m_optionState.SetActiveSlot(slot);
		//m_currentState = State.Options;
		ChangeState(m_optionState);
	}
	
	public void SetSlotValue(int slot, Pokemon.Species species)
	{
		//m_roster[slot] = species;
		//m_roster.SetRosterSlot(slot, species);
		m_roster[slot] = species;
		
		ChangeState(m_rosterState);
		//m_currentState = State.Roster;
	}
	
	public IDisplayState GetActiveDisplay()
	{
		return m_currentState;
//		if (m_currentState == State.Start)
//		{
//			return m_startState;
//		}
//		else if (m_currentState == State.Roster)
//		{
//			return m_rosterState;
//		}
//		else if (m_currentState == State.Options)
//		{
//			return m_optionState;
//		}
		
		//return null;
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

