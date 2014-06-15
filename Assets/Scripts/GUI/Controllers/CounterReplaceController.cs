using PokemonGUI;
using PokeCore;
using UnityEngine;

public class CounterReplaceController : IScreenController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	CounterReplaceDisplay m_display;
	
	public enum Options
	{
		Replace,
		Continue
	};
	
	public CounterReplaceController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<CounterReplaceDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void ProcessInput(int selection)
	{
		switch ((Options)selection)
		{
			case Options.Continue:
				NoTurnAction action = new NoTurnAction(m_system.UserPlayer.ActivePokemon);
				m_request.SubmitAction(action);
				return;
			case Options.Replace:
				IScreenController nextScreen = new SwitchPokemonController(m_manager, m_gameObject, m_system, m_request);
				m_manager.LoadScreen(nextScreen);
				break;
		}
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

