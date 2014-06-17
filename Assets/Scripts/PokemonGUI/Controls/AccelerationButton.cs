using UnityEngine;

namespace PokemonGUI {
namespace Controls {

public class AccelerationButton
{
	static int currentDelay = 0;
	
	static int timePressed = 0;
	static int lastProcessed = 0;
	
	static int getNextDelay(int timePressed)
	{
		if (timePressed < 1000)
		{
			return 250;
		}
		else if ((timePressed >= 1000) && (timePressed < 2000))
		{
			return 100;
		}
		else if ((timePressed >= 2000) && (timePressed < 3000))
		{
			return 50;
		}
		else
		{
			return 25;
		}
	}
	
	public static bool Display(Rect bounds, GUIContent content)
	{
		bool result = false;
		bool mouseUp = (Event.current.type == EventType.MouseUp) ? true : false;
		
		if (GUI.RepeatButton(bounds, content))
		{
			lastProcessed += (int)(Time.deltaTime * 1000);
			timePressed += (int)(Time.deltaTime * 1000);
			
			if (lastProcessed > currentDelay)
			{
				result = true;
				lastProcessed = 0;
				currentDelay = getNextDelay(timePressed);
			}
			
			if (mouseUp)
			{
				timePressed = 0;
				lastProcessed = 0;
				currentDelay = 0;
			}
		}
		
		
		return result;
	}
}

}}