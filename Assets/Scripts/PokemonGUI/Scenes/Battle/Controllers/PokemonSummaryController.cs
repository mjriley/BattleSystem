using UnityEngine;
using PokeCore;
using System.Collections.Generic;
using System;

namespace PokemonGUI {
namespace Scenes {
namespace Battle { 
	using Screens;
namespace Controllers {

public class PokemonSummaryController : IScreenController, IMoveController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	StatDisplay m_display;
	WeakReference m_parentController;
	IActionRequest m_request;
	
	int m_pokemonIndex = 0;
	
	public enum ActiveScreen
	{
		Summary,
		Moves
	}
	
	ActiveScreen m_activeScreen;
	Dictionary<ActiveScreen, IGameScreen> m_displays;
	
	
	public PokemonSummaryController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, 
		ActiveScreen activeScreen, WeakReference parentController, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_activeScreen = activeScreen;
		m_parentController = parentController;
		m_request = request;
		
		StatDisplay statDisplay = gameObject.GetComponentsInChildren<StatDisplay>(true)[0];
		statDisplay.SetController(this);
		MoveExtendedDisplay moveDisplay = gameObject.GetComponentsInChildren<MoveExtendedDisplay>(true)[0];
		moveDisplay.SetController(this);
		
		m_displays = new Dictionary<ActiveScreen, IGameScreen>
		{
			{ActiveScreen.Summary, statDisplay},
			{ActiveScreen.Moves, moveDisplay}
		};
	}
	
	public void SetPokemonIndex(int index)
	{
		m_pokemonIndex = index;
	}
	
	public Character GetPokemon()
	{
		return m_system.UserPlayer.Pokemon[m_pokemonIndex];
	}
	
	public bool Extended { get { return true; } }
	
	public void ProcessInput(int selection)
	{
		if (m_activeScreen == ActiveScreen.Moves)
		{
			MoveDetailsController nextScreen = new MoveDetailsController(m_manager, m_gameObject, m_system, m_request);
			nextScreen.SetPokemon(m_system.UserPlayer.Pokemon[m_pokemonIndex]);
			nextScreen.SetMoveIndex(selection);
			m_manager.LoadScreen(nextScreen);
		}
	}
	
	public void NextPage()
	{
		m_pokemonIndex += 1;
		m_displays[m_activeScreen].Invalidate();
		((PokemonDetailsController)m_parentController.Target).SetPokemonIndex(m_pokemonIndex);
	}
	
	public void PreviousPage()
	{
		m_pokemonIndex -= 1;
		m_displays[m_activeScreen].Invalidate();
		((PokemonDetailsController)m_parentController.Target).SetPokemonIndex(m_pokemonIndex);
	}
	
	public bool HasPreviousPage { get { return m_pokemonIndex > 0; } }
	public bool HasNextPage { get { return m_pokemonIndex < m_system.UserPlayer.Pokemon.Count - 1; } }
	
	public void SwitchScreens(ActiveScreen nextScreen)
	{
		m_displays[m_activeScreen].enabled = false;
		
		m_activeScreen = nextScreen;
		
		m_displays[m_activeScreen].enabled = true;
	}
	
	public void Enable()
	{
		m_displays[m_activeScreen].enabled = true;
	}
	
	public void Disable()
	{
		m_displays[m_activeScreen].enabled = false;
	}
	
	public void Unload()
	{
		m_manager.UnloadScreen();
	}
}

}}}}