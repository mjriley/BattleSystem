using UnityEngine;
using System.Collections.Generic;
using PokeCore;
using PokemonGUI;

public class ItemCategoryController : IScreenController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	ItemCategoryDisplay m_display;
	
	public ItemCategoryController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<ItemCategoryDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void ProcessInput(int selection)
	{
		ItemCategory category = (ItemCategory)selection;
		ItemListController nextScreen = new ItemListController(m_manager, m_gameObject, m_system, m_request, category);
		
		m_manager.LoadScreen(nextScreen);
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
