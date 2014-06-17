using System.Collections.Generic;

namespace PokemonGUI {

public class ScreenManager
{
	Stack<IScreenController> m_screens;
	
	public ScreenManager()
	{
		m_screens = new Stack<IScreenController>();
	}
	
	public void LoadScreen(IScreenController controller)
	{
		if (m_screens.Count > 0)
		{
			m_screens.Peek().Disable();
		}
		
		m_screens.Push(controller);
		
		controller.Enable();
	}
	
	public void UnloadScreen()
	{
		IScreenController currentScreen = m_screens.Pop();
		currentScreen.Disable();
		
		if (m_screens.Count > 0)
		{
			m_screens.Peek().Enable();
		}
	}
	
	public void Disable()
	{
		m_screens.Peek().Disable();
		m_screens.Clear();
	}
}

}