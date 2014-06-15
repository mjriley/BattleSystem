//using System.Collections.Generic;
//using UnityEngine;
//
//public class ReplacePokemonController : MonoBehaviour, IScreenState
//{
//	public enum Screen
//	{
//		Selection,
//		Details
//	};
//	
//	Stack<IScreenController> m_screenControllers;
//	
//	void Awake()
//	{
//		m_screenControllers = new Stack<IScreenController>();
//	}
//	
//	void Start()
//	{
//		BattleController battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
//		IScreenController startScreen = new SelectPokemonController(this, gameObject, battleController.GetSystem());
//		LoadScreen(startScreen);
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
//			IScreenController prevScreen = m_screenControllers.Peek();
//			prevScreen.Enable();
//		}
//	}
//}
