using UnityEngine;
using PokeCore;
using PokemonGUI;

public class BattleController : MonoBehaviour
{
	NewBattleSystem m_system;
	
	public GameObject m_turnParent;
	public GameObject m_screenParent;
	public GameObject m_battleDisplay;
	
	void Awake()
	{
		m_system = new NewBattleSystem();
		PlayerRoster roster = GameObject.Find("PlayerRoster").GetComponent<PlayerRoster>();
		
		BattleDisplay display = m_battleDisplay.GetComponent<BattleDisplay>();
		
		TurnRequest turnRequest = new TurnRequest(m_system, m_screenParent, display);
		ReplaceRequest replaceRequest = new ReplaceRequest(m_system, m_screenParent, display);
		CounterReplaceRequest counterReplaceRequest = new CounterReplaceRequest(m_system, m_screenParent, display);
		m_system.InitializePlayer(roster.roster, turnRequest, replaceRequest, counterReplaceRequest);
	}
	
	public NewBattleSystem GetSystem()
	{
		return m_system;
	}
}
