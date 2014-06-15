using PokeCore;
using UnityEngine;
using PokemonGUI;

public class TargettedItemListController : ItemListController
{
	ItemCategory m_category;
	
	public TargettedItemListController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request, ItemCategory category)
	: base(manager, gameObject, system, request, category)
	{
	}
	
	public override void ProcessInput(int selection)
	{
		TargettedItemDescriptionController nextScreen = new TargettedItemDescriptionController(m_manager, m_gameObject, m_system, m_request, m_items[selection]);
		m_manager.LoadScreen(nextScreen);
	}
}
