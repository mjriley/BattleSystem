using UnityEngine;
using System.Collections.Generic;
using Moves;
using PokeCore;
using PokemonGUI;

public class MoveController : IMoveController
{
	ScreenManager m_manager;
	MoveDisplay m_display;
	
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	int m_pokemonIndex = 0;
	
	public MoveController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request, int pokemonIndex)
	{
		m_manager = manager;
		m_display = gameObject.GetComponentsInChildren<MoveDisplay>(true)[0];
		m_display.SetController(this);
		
		m_system = system;
		m_request = request;
		
		SetPokemonIndex(pokemonIndex);
	}
	
	public void SetPokemonIndex(int index)
	{
		m_pokemonIndex = index;
	}
	
	public Character GetPokemon()
	{
		return m_system.UserPlayer.Pokemon[m_pokemonIndex];
	}
	
	public void ProcessInput(int selection)
	{
		Move selectedMove = m_system.UserPlayer.Pokemon[m_pokemonIndex].getMoves()[selection];
		
		ITurnAction action = new MoveUse(m_system.UserPlayer.ActivePokemon, m_system.EnemyPlayer, selectedMove);
		
		SubmitTurn(action);
	}
	
	void SubmitTurn(ITurnAction action)
	{
		m_request.SubmitAction(action);
	}
	
	public void Unload()
	{
		m_manager.UnloadScreen();
	}
	
	public void Enable()
	{
		m_display.enabled = true;
	}
	
	public void Disable()
	{
		m_display.enabled = false;
	}
	
	//public bool Extended { get { return false; } }
}
