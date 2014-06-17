using UnityEngine;
using PokeCore;
using PokeCore.Items;
using System.Collections.Generic;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Screens;
namespace Controllers {

public class TargettedItemDescriptionController : IItemDescriptionController
{
	ScreenManager m_manager;
	
	ItemDescriptionDisplay m_display;
	
	KeyValuePair<IItem, int> m_activeItem;
	
	public TargettedItemDescriptionController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request, KeyValuePair<IItem, int> item)
	{
		m_manager = manager;
		
		m_activeItem = item;
		
		m_display = gameObject.GetComponentsInChildren<ItemDescriptionDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public KeyValuePair<IItem, int> GetItem()
	{
		return m_activeItem;
	}
	
	public void ProcessInput(int selection)
	{
		Debug.Log("Submitting targetted selection!");
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