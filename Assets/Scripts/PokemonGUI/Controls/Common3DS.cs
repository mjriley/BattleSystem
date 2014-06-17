using UnityEngine;

namespace PokemonGUI {
namespace Controls {

// GUI methods for drawing common GUI objects in the same location between screens
// These methods work with 3DS coordinates, using the bottom screen unless otherwise specified
public static class Common3DS
{
	const int ARROW_START_Y = 190;
	
	// can be used to aid in placement by elements which depend on the arrow positioning
	public static int ArrowStartY { get { return ARROW_START_Y; } }
	
	public static bool BackButton()
	{
		bool result = false;
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Vector2 arrowSize = NavigationArrow.CalcSize();
			result =  NavigationArrow.Display(new Rect(20, ARROW_START_Y, arrowSize.x, arrowSize.y), NavigationArrow.Direction.Previous);
		});
		
		return result;
	}
	
	public static bool NextButton()
	{
		bool result = false;
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Vector2 arrowSize = NavigationArrow.CalcSize();
			result = NavigationArrow.Display(new Rect(84, ARROW_START_Y, arrowSize.x, arrowSize.y), NavigationArrow.Direction.Next);
		});
		
		return result;
	}
}

}}