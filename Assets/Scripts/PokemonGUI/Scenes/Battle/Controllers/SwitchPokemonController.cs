using UnityEngine;
using PokeCore;

namespace PokemonGUI {
namespace Scenes {
namespace Battle { 
	using Screens;
namespace Controllers {

public class SwitchPokemonController : ISelectPokemonController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	ChoosePokemonDisplay m_display;
	
	public SwitchPokemonController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<ChoosePokemonDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void ProcessInput(int selection)
	{
		PokemonDetailsController nextScreen = new PokemonDetailsController(m_manager, m_gameObject, m_system, m_request);
		nextScreen.SetPokemonIndex(selection);
		
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
		if (m_request.RequestType == RequestType.CounterReplace)
		{
			// For whatever reason, Pokemon X decides that trying to back out of this screen is the same as cancelling the request
			// So, for purposes of accuracy, that logic is duplicated here
			NoTurnAction action = new NoTurnAction(m_system.UserPlayer.ActivePokemon);
			m_request.SubmitAction(action);
			return;
		}
		
		m_manager.UnloadScreen();
	}
	
	public bool BackEnabled { get { return m_request.RequestType != RequestType.Replace; } }
}

}}}}
