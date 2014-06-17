using UnityEngine;

namespace PokemonGUI {
namespace Controls {

public class NavigationArrow
{
	static Texture2D m_arrowForward = Resources.Load<Texture2D>("Textures/Buttons/PageForwardArrow");
	static Texture2D m_arrowBackward = Resources.Load<Texture2D>("Textures/Buttons/PageBackArrow");
	static GUIStyle m_emptyStyle = new GUIStyle();
	
	public enum Direction
	{
		Next,
		Previous
	}
	
	public static Vector2 CalcSize()
	{
		// works with the assumption that both directions are the same size
		return new Vector2(m_arrowForward.width, m_arrowForward.height);
	}
	
	public static bool Display(Rect bounds, Direction direction)
	{
		Texture2D texture = (direction == Direction.Next) ? m_arrowForward : m_arrowBackward;
		bool result = false;
		GUIUtils.DrawGroup(bounds, delegate()
		{
			result = GUI.Button(new Rect(0, 0, texture.width, texture.height), texture, m_emptyStyle);
		});
		
		return result;
	}
}

}}