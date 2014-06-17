using UnityEngine;

namespace PokemonGUI {

public class CustomGUI
{
	public static void BorderedLabel(Rect position, string text, int borderThickness, Color borderColor, GUIStyle style)
	{
		BorderedLabel(position, new GUIContent(text), borderThickness, borderColor, style);
	}
	
	public static void BorderedLabel(Rect position, GUIContent content, int borderThickness, Color borderColor, GUIStyle style)
	{
		Color prevColor = style.normal.textColor;
		style.normal.textColor = borderColor;
		
		position.x -= borderThickness;
		position.y -= borderThickness;
		GUI.Label(position, content, style);
		
		position.x += borderThickness * 2;
		GUI.Label(position, content, style);
		
		position.y += borderThickness * 2;
		GUI.Label(position, content, style);
		
		position.x -= borderThickness * 2;
		GUI.Label(position, content, style);
		
		style.normal.textColor = prevColor;
		position.x += borderThickness;
		position.y -= borderThickness;
		GUI.Label(position, content, style);
	}
	
}

}
