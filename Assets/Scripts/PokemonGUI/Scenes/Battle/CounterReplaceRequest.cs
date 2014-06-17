using UnityEngine;
using PokeCore;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Controllers;

public class CounterReplaceRequest : IActionRequest
{
	NewBattleSystem m_system;
	GameObject m_gameObject;
	NewBattleSystem.ProcessAction m_actionCallback;
	BattleDisplay m_display;
	
	ScreenManager m_manager;
	
	public CounterReplaceRequest(NewBattleSystem system, GameObject gameObject, BattleDisplay display)
	{
		m_system = system;
		m_gameObject = gameObject;
		m_display = display;
	}
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		m_manager = new ScreenManager();
		IScreenController startScreen = new CounterReplaceController(m_manager, m_gameObject, m_system, this);
		m_manager.LoadScreen(startScreen);
		
		m_actionCallback = actionCallback;
	}
	
	public void SubmitAction(ITurnAction action)
	{
		m_display.ClearMessage();
		if (m_actionCallback(action))
		{
			m_manager.Disable();
		}
	}
	
	public RequestType RequestType { get { return RequestType.CounterReplace; } }
}

}}}