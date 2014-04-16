using UnityEngine;

public class PokemonTagDisplay
{
	private static Texture2D m_backgroundTexture;
	private static GUIStyle m_internalStyle;
	private static GUIStyle m_emptyButtonStyle;
	
	private static bool m_isInit = false;
	
	private static void Init()
	{
		if (!m_isInit)
		{
			m_internalStyle = new GUIStyle();
			m_emptyButtonStyle = new GUIStyle();
			m_backgroundTexture = Resources.Load<Texture2D>("Textures/pokemon_tag");
			m_isInit = true;
		}
	}
	
	public static Vector2 CalcMinSize()
	{
		return CalcMinSize(m_internalStyle);
	}
	
	public static Vector2 CalcMinSize(GUIStyle style)
	{
		Init();
		return new Vector2(m_backgroundTexture.width, m_backgroundTexture.height);
	}
	
	public static bool Button(Rect screenRect, Character pokemon, bool drawReversed)
	{
		return Button(screenRect, pokemon, drawReversed, m_internalStyle);
	}
	
	public static bool Button(Rect screenRect, Character pokemon, bool drawReversed, GUIStyle style)
	{
		Init();
		
		Rect bounds;
		Rect normalBounds = new Rect(0, 0, m_backgroundTexture.width, m_backgroundTexture.height);
		if (drawReversed)
		{
			bounds = new Rect(m_backgroundTexture.width, 0, -m_backgroundTexture.width, m_backgroundTexture.height);
		}
		else
		{
			bounds = normalBounds;
		}
		
		bool result = false;
		
		GUI.BeginGroup(screenRect);
			GUI.DrawTexture(bounds, m_backgroundTexture);
			if (pokemon != null)
			{
				GUI.Label(new Rect(0, 0, screenRect.width, screenRect.height), pokemon.Name);
			}
			
			result = GUI.Button(normalBounds, "", m_emptyButtonStyle);
		GUI.EndGroup();
		
		return result;
	}
}
