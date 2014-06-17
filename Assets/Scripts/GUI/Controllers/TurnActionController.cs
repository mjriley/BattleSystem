using UnityEngine;
using PokeCore;
using PokemonGUI;

public class TurnActionController : IScreenController
{
	public enum Options : int
	{
		Fight = 0,
		Bag,
		Run,
		Pokemon
	}
	
	ScreenManager m_manager;
	GameObject m_gameObject;
	TurnActionDisplay m_display;
	NewBattleSystem m_system;
	IActionRequest m_request;
	
	public TurnActionController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
	{
		m_manager = manager;
		m_gameObject = gameObject;
		m_system = system;
		m_request = request;
		
		m_display = gameObject.GetComponentsInChildren<TurnActionDisplay>(true)[0];
		m_display.SetController(this);
	}
	
	public void ProcessInput(int selection)
	{
		IScreenController nextScreen;
		switch ((Options)selection)
		{
			case Options.Fight:
				int pokemonIndex = m_system.UserPlayer.Pokemon.IndexOf(m_system.UserPlayer.ActivePokemon);
				nextScreen = new MoveController(m_manager, m_gameObject, m_system, m_request, pokemonIndex);
				m_manager.LoadScreen(nextScreen);
				break;
			case Options.Bag:
				nextScreen = new ItemCategoryController(m_manager, m_gameObject, m_system, m_request);
				m_manager.LoadScreen(nextScreen);
				break;
			case Options.Run:
				RunUse run = new RunUse(m_system.UserPlayer.ActivePokemon);
				m_request.SubmitAction(run);
				break;
			case Options.Pokemon:
				nextScreen = new SwitchPokemonController(m_manager, m_gameObject, m_system, m_request);
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
	
	}
}

