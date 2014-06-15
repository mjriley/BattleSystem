using PokemonGUI;
using PokeCore;
using UnityEngine;
using Items;

public class RestoreAbilityController : IAbilityController
{
	ScreenManager m_manager;
	NewBattleSystem m_system;
	IActionRequest m_request;
	IItem m_item;
	Character m_pokemon;
	
	AbilityDisplay m_display;
	
	public RestoreAbilityController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request, IItem item, Character pokemon)
	{
		m_manager = manager;
		m_system = system;
		m_request = request;
		m_item = item;
		m_pokemon = pokemon;
		
		m_display = gameObject.GetComponentsInChildren<AbilityDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public Character GetPokemon()
	{
		return m_pokemon;
	}
	
	public void ProcessInput(int selection)
	{
		ItemContext context = new ItemContext(m_system.UserPlayer, m_system.UserPlayer.ActivePokemon);
		context.AbilityIndex = selection;
		
		ItemUse itemUse = new ItemUse(m_item, context, m_system.UserPlayer.Inventory);
		
		m_request.SubmitAction(itemUse);
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

