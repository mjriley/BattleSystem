using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using PokeCore;
using PokemonGUI;
using PokeCore.Items;

namespace PokemonGUI {
namespace Scenes {
namespace Battle { 
namespace Controllers {

public class ItemListController : IScreenController
{
	protected ScreenManager m_manager;
	protected GameObject m_gameObject;
	protected NewBattleSystem m_system;
	protected IActionRequest m_request;
	
	ItemListDisplay m_display;
	
	protected List<KeyValuePair<IItem, int>> m_items;
	ItemCategory m_category;
	
	int m_activePage = 0;
	
	const int ITEMS_PER_PAGE = 6;
	
	public bool HasPrevPage { get { return m_activePage > 0; } }
	public bool HasNextPage { get { return m_activePage < (TotalPageCount - 1); } }
	
	int TotalPageCount { get { return (int)Math.Ceiling(m_items.Count / (float)ITEMS_PER_PAGE); } }
	
	public ItemListController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request, ItemCategory category)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_category = category;
		m_items = m_system.UserPlayer.Inventory.GetCategory(category);
		
		m_display = gameObject.GetComponentsInChildren<ItemListDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public string GetCategoryName()
	{
		return L18N.GetItemCategory(m_category).ToUpper();
	}
	
	public int GetCurrentPage()
	{
		return m_activePage + 1;
	}
	
	public int GetTotalPages()
	{
		return TotalPageCount;
	}
	
	public List<KeyValuePair<IItem, int>> GetItems()
	{
		int startingIndex = m_activePage * ITEMS_PER_PAGE;
		List<KeyValuePair<IItem, int>> returned = new List<KeyValuePair<IItem, int>>();
		for (int i = startingIndex; i < m_items.Count && i < startingIndex + ITEMS_PER_PAGE; ++i)
		{
			returned.Add(m_items[i]);
		}
		
		return returned;
	}
	
	public void NextPage()
	{
		m_activePage += 1;
		m_display.Invalidate();
	}
	
	public void PrevPage()
	{
		m_activePage -= 1;
		m_display.Invalidate();
	}
	
	public virtual void ProcessInput(int selection)
	{
		ItemDescriptionController nextScreen = new ItemDescriptionController(m_manager, m_gameObject, m_system, m_request);
		int fullIndex = m_activePage * ITEMS_PER_PAGE + selection;
		nextScreen.SetItem(m_items[fullIndex]);
		nextScreen.SetCategory(m_category);
		
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


}}}}