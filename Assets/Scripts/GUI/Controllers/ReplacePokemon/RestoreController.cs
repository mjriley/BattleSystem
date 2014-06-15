using UnityEngine;
using System.Collections.Generic;
using PokeCore;
using PokemonGUI;

public class RestoreController : IScreenController
{
	public enum Options
	{
		RestoreResource,
		RestoreStatus
	};
	
	ScreenManager m_manager;
	RestoreDisplay m_display;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	Character m_pokemon;
	
	public RestoreController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<RestoreDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void SetPokemon(Character pokemon)
	{
		m_pokemon = pokemon;
	}
	
	public Character GetPokemon()
	{
		return m_pokemon;
	}
	
	public void ProcessInput(int selection)
	{
		ItemCategory category = ItemCategory.Resource;
		switch ((Options)selection)
		{
			case Options.RestoreResource:
			{
				category = ItemCategory.Resource;
				break;
			}
			case Options.RestoreStatus:
			{
				category = ItemCategory.Status;
				break;
			}
		}
		
		TargettedItemListController nextScreen = new TargettedItemListController(m_manager, m_gameObject, m_system, m_request, category);
		m_manager.LoadScreen(nextScreen);
	}
	
	public void Enable()
	{
		m_display.enabled = true;
	}
	
	public void Disable()
	{
		m_display.enabled = false;
	}
	
	public void Unload()
	{
		m_manager.UnloadScreen();
	}
}
