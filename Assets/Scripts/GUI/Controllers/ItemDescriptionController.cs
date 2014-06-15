using UnityEngine;
using PokeCore;
using PokemonGUI;
using Items;
using System.Collections.Generic;

public class ItemDescriptionController : IItemDescriptionController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	ItemDescriptionDisplay m_display;
	
	KeyValuePair<IItem, int> m_activeItem;
	ItemCategory m_category;
	
	public ItemDescriptionController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<ItemDescriptionDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void SetItem(KeyValuePair<IItem, int> item)
	{
		m_activeItem = item;
	}
	
	public void SetCategory(ItemCategory category)
	{
		m_category = category;
	}
	
	public KeyValuePair<IItem, int> GetItem()
	{
		return m_activeItem;
	}
	
	public void ProcessInput(int selection)
	{
		if ((m_category == ItemCategory.Balls) || (m_category == ItemCategory.Battle))
		{
			ItemContext context;
			if (m_category == ItemCategory.Balls)
			{
				context = new ItemContext(m_system.UserPlayer, m_system.EnemyPlayer.ActivePokemon);
			}
			else
			{
				context = new ItemContext(m_system.UserPlayer, m_system.UserPlayer.ActivePokemon);
			}
			
			ItemUse use = new ItemUse(m_activeItem.Key, context, m_system.UserPlayer.Inventory);
			m_request.SubmitAction(use);
			return;
		}
		
		ChoosePokemonController nextScreen = new ChoosePokemonController(m_manager, m_gameObject, m_system, m_request);
		nextScreen.SetItem(m_activeItem.Key);
		
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

