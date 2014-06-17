using UnityEngine;
using PokeCore;
using System;
using PokemonGUI;

public class PokemonDetailsController : IScreenController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	PokemonDetailsDisplay m_display;
	
	int m_pokemonIndex = 0;
	
	string m_deployStatus;
	
	public enum Options
	{
		Submit,
		Restore,
		Summary,
		Moves
	};
	
	public PokemonDetailsController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<PokemonDetailsDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void SetPokemonIndex(int index)
	{
		m_pokemonIndex = index;
		
		Character pokemon = m_system.UserPlayer.Pokemon[m_pokemonIndex];
		
		if (m_system.UserPlayer.ActivePokemon == pokemon)
		{
			m_deployStatus = "In Battle";
		}
		else if (pokemon.isDead())
		{
			m_deployStatus = "Cannot Battle";
		}
		else
		{
			m_deployStatus = "Switch";
		}
	}
	
	public Character GetPokemon()
	{
		return m_system.UserPlayer.Pokemon[m_pokemonIndex];
	}
	
	public string GetDeployStatus()
	{
		return m_deployStatus;
	}
	
	public bool RestoreEnabled()
	{
		return (m_request.RequestType == RequestType.Turn);
	}
	
	public void ProcessInput(int selection)
	{
		switch ((Options)selection)
		{
			case Options.Submit:
			{
				ITurnAction action;
				if (m_request.RequestType == RequestType.Replace)
				{
					action = new DeployAction(m_system.UserPlayer, m_pokemonIndex, true);
				}
				else
				{
					action = new SwapAction(m_system.UserPlayer, m_pokemonIndex);
				}
				m_request.SubmitAction(action);
				return;
			}
			case Options.Restore:
			{
				RestoreController nextScreen = new RestoreController(m_manager, m_gameObject, m_system, m_request);
				nextScreen.SetPokemon(GetPokemon());
				m_manager.LoadScreen(nextScreen);
				break;
			}
			case Options.Summary:
			{
				PokemonSummaryController nextScreen = new PokemonSummaryController(m_manager, m_gameObject, m_system, 
					PokemonSummaryController.ActiveScreen.Summary, new WeakReference(this), m_request);
				nextScreen.SetPokemonIndex(m_pokemonIndex);
				m_manager.LoadScreen(nextScreen);
				break;
			}
			case Options.Moves:
			{
				PokemonSummaryController nextScreen = new PokemonSummaryController(m_manager, m_gameObject, m_system,
					PokemonSummaryController.ActiveScreen.Moves, new WeakReference(this), m_request);
				nextScreen.SetPokemonIndex(m_pokemonIndex);
				m_manager.LoadScreen(nextScreen);
				break;
			}
		}
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
}
