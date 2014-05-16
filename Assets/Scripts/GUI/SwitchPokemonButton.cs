using UnityEngine;

public class SwitchPokemonButton
{
	static Texture2D m_background;
	static GUIStyle m_defaultStyle;
	static bool m_isInit = false;
	
	static void Init()
	{
		if (!m_isInit)
		{
			m_background = Resources.Load<Texture2D>("Textures/SwitchPokemonButton");
			m_defaultStyle = new GUIStyle();
			m_defaultStyle.normal.background = m_background;
			m_defaultStyle.alignment = TextAnchor.MiddleCenter;
			m_defaultStyle.font = Resources.Load<Font>("Fonts/Courier");
		}
	}
	
	public static bool Display(Rect rect, string text, GUIStyle style=null)
	{
		Init();
		
		if (style == null)
		{
			style = m_defaultStyle;
		}
		
		bool returned = false;
		
		GUIUtils.DrawGroup(rect, delegate(Rect bounds)
		{
			returned = GUI.Button(new Rect(0.0f, 0.0f, style.normal.background.width, style.normal.background.height), text, style);
		});
		
		return returned;
	}
}
