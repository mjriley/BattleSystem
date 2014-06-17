using UnityEngine;
using PokeCore;
using PokemonGUI;
using Items;

public class ChoosePokemonController : ISelectPokemonController
{
	ScreenManager m_manager;
	GameObject m_gameObject;
	NewBattleSystem m_system;
	IActionRequest m_request;
	ChoosePokemonDisplay m_display;
	
	IItem m_item;
	
	public ChoosePokemonController(ScreenManager manager, GameObject gameObject, NewBattleSystem system, IActionRequest request)
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
		Character pokemon = m_system.UserPlayer.Pokemon[selection];
		if (m_item is EtherItem)
		{
			EtherItem ether = (EtherItem)m_item;
			if (!ether.AllMoves)
			{
				IScreenController nextScreen = new RestoreMoveController(m_manager, m_gameObject, m_system, m_request, m_item, pokemon);
				m_manager.LoadScreen(nextScreen);
				return;
			}
		}
		ItemContext context = new ItemContext(m_system.UserPlayer, pokemon);
		ItemUse use = new ItemUse(m_item, context, m_system.UserPlayer.Inventory);
		m_request.SubmitAction(use);
	}
	
	public void SetItem(IItem item)
	{
		m_item = item;
	}
	
	public IItem GetItem()
	{
		return m_item;
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
	
	public bool BackEnabled { get { return true; } }
}

