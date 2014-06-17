using UnityEngine;
using PokemonGUI;

namespace PokemonGUI {
namespace Controls {

public class SwitchPokemonButton
{
	static Texture2D m_background;
	static Texture2D m_gradient;
	static GUIStyle m_defaultStyle;
	
	static SwitchPokemonButton()
	{
		m_background = Resources.Load<Texture2D>("Textures/SwitchPokemonButton");
		m_gradient = Resources.Load<Texture2D>("Textures/SwitchPokemonGradient");
		m_defaultStyle = new GUIStyle();
		m_defaultStyle.normal.background = m_gradient;
		m_defaultStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	public static bool Display(Rect rect, string text, Color color, GUIStyle style=null)
	{
		if (style == null)
		{
			style = m_defaultStyle;
		}
		
		bool returned = false;
		
		GUIUtils.DrawGroup(rect, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0.0f, 0.0f, m_background.width, m_background.height), m_background);
			
			Color prevColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			returned = GUI.Button(new Rect(0.0f, 0.0f, style.normal.background.width, style.normal.background.height), text, style);
			GUI.backgroundColor = prevColor;
		});
		
		return returned;
	}
}

}}