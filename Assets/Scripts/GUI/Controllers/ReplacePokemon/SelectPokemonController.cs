//using UnityEngine;
//using PokeCore;
//using PokemonGUI;
//
//public class SelectPokemonController : ISelectPokemonController
//{
//	ScreenManager m_manager;
//	GameObject m_gameObject;
//	ChoosePokemonDisplay m_display;
//	NewBattleSystem m_system;
//	
//	
//	public SelectPokemonController(ScreenManager manager, GameObject gameObject, NewBattleSystem system)
//	{
//		m_manager = manager;
//		m_gameObject = gameObject;
//		m_system = system;
//		
//		m_display = gameObject.GetComponentsInChildren<ChoosePokemonDisplay>(true)[0];
//		m_display.SetController(this);
//	}
//	
//	public void ProcessInput(int selection)
//	{
//		PokemonDetailsController nextScreen = new PokemonDetailsController(m_manager, m_gameObject, m_system);
//		nextScreen.SetPokemonIndex(selection);
//		m_manager.LoadScreen(nextScreen);
//	}
//	
//	public void Unload()
//	{
//		// base screen, can't be unloaded
//	}
//	
//	public void Enable()
//	{
//		m_display.enabled = true;
//	}
//	
//	public void Disable()
//	{
//		m_display.enabled = false;
//	}
//	
//	public bool BackEnabled { get { return false; } }
//}
//
