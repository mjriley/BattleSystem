using UnityEngine;

public class BackButton
{
	static bool m_isInit = false;
	static GUIStyle m_style;
	static Texture2D m_buttonTexture;
	
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		m_buttonTexture = Resources.Load<Texture2D>("Textures/Buttons/BackButton");
		m_style = new GUIStyle(); 
		m_isInit = true;
	}
	
	public static Vector2 CalcSize()
	{
		Init();
		return new Vector2(m_buttonTexture.width, m_buttonTexture.height);
	}
	
	// Display the back button at the bottom right corner of the supplied bounds
	public static bool Display(Rect bounds)
	{
		Init();
		
		return GUI.Button(new Rect(bounds.x + bounds.width - m_buttonTexture.width,
			bounds.y + bounds.height - m_buttonTexture.height,
			m_buttonTexture.width, m_buttonTexture.height), m_buttonTexture, m_style);
	}
	
}

