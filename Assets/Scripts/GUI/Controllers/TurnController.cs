//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using PokeCore;
//
//public class TurnController : MonoBehaviour, IScreenState
//{
//	public enum Screen
//	{
//		TurnAction,
//		Abilities,
//		SwitchPokemon,
//		ItemCategories,
//		ItemSelection,
//		ItemDescription,
//		PokemonSelection
//	}
//	
//	Stack<IScreenController> m_screenControllers;
//	BattleController m_battleController;
//	NewBattleSystem m_system;
//	
//	void Awake()
//	{
//		m_battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
//	}
//	
//	void Start()
//	{
//		m_system = m_battleController.GetSystem();
//		
//		m_screenControllers = new Stack<IScreenController>();
//		
//		IScreenController startScreen = new TurnActionController(this, gameObject, m_system, m_battleController);
//		LoadScreen(startScreen);
//	}
//	
//	void OnEnable()
//	{
//	}
//	
//	public void LoadScreen(IScreenController controller)
//	{
//		if (m_screenControllers.Count > 0)
//		{
//			m_screenControllers.Peek().Disable();
//		}
//		
//		m_screenControllers.Push(controller);
//		
//		controller.Enable();
//	}
//	
//	public void UnloadScreen()
//	{
//		IScreenController currentScreen = m_screenControllers.Pop();
//		currentScreen.Disable();
//		
//		if (m_screenControllers.Count > 0)
//		{
//			m_screenControllers.Peek().Enable();
//		}
//	}
//	
//	public void SubmitTurn(ITurnAction action)
//	{
//		m_battleController.SetAction(action);
//		
//		IScreenController currentScreen = m_screenControllers.Pop();
//		currentScreen.Disable();
//		
//		gameObject.SetActive(false);
//	}
//}
