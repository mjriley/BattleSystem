using UnityEngine;
using PokeCore;
using PokemonGUI;

public class MoveDetailsController : IMoveController
{
	ScreenManager m_manager;
	MoveDetailsDisplay m_display;
	IActionRequest m_request;
	
	Character m_pokemon;
	int m_moveIndex;
	
	public MoveDetailsController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		
		m_display = gameObject.GetComponentsInChildren<MoveDetailsDisplay>(true)[0];
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
	
	public void SetMoveIndex(int index)
	{
		m_moveIndex = index;
	}
	
	public int GetMoveIndex()
	{
		return m_moveIndex;
	}
	
}
