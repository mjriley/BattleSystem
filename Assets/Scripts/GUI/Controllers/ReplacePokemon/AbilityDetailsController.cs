using UnityEngine;
using PokeCore;
using PokemonGUI;

public class AbilityDetailsController : IAbilityController
{
	ScreenManager m_manager;
	AbilityDetailsDisplay m_display;
	IActionRequest m_request;
	
	Character m_pokemon;
	int m_abilityIndex;
	
	public AbilityDetailsController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		
		m_display = gameObject.GetComponentsInChildren<AbilityDetailsDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void ProcessInput(int selection)
	{
	
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
	
	//public bool Extended { get { return true; } }
	
	public void SetPokemon(Character pokemon)
	{
		m_pokemon = pokemon;
	}
	
	public Character GetPokemon()
	{
		return m_pokemon;
	}
	
	public void SetAbilityIndex(int index)
	{
		m_abilityIndex = index;
	}
	
	public int GetAbilityIndex()
	{
		return m_abilityIndex;
	}
	
}
