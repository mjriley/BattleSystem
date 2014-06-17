using UnityEngine;
using PokeCore;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Controllers;

public class TurnRequest : IActionRequest
{
	NewBattleSystem m_system;
	GameObject m_gameObject;
	ScreenManager m_manager;
	BattleDisplay m_display;
	NewBattleSystem.ProcessAction m_actionCallback;
	
	public TurnRequest(NewBattleSystem system, GameObject gameObject, BattleDisplay display)
	{
		m_system = system;
		m_gameObject = gameObject;
		m_display = display;
	}
	
	public void SubmitAction(ITurnAction action)
	{
		m_display.ClearMessage();
		if (m_actionCallback(action))
		{
			m_manager.Disable();
		}
	}
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		m_manager = new ScreenManager();
		IScreenController startScreen = new TurnActionController(m_manager, m_gameObject, m_system, this);
		m_manager.LoadScreen(startScreen);
		
		m_actionCallback = actionCallback;
	}
	
	public RequestType RequestType { get { return RequestType.Turn; } }
}

}}}