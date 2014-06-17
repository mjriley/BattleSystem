using UnityEngine;
using Moves;

public class TypeTag
{
	static bool m_isInit = false;
	static Texture2D m_tagTexture;
	static Texture2D m_tagBorderTexture;
	static GUIStyle m_defaultTypeNameStyle;
	
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		m_tagTexture = Resources.Load<Texture2D>("Textures/TypeTag");
		m_tagBorderTexture = Resources.Load<Texture2D>("Textures/TypeTagBorder");
		
		m_defaultTypeNameStyle = new GUIStyle();
		m_defaultTypeNameStyle.normal.textColor = Color.white;
		m_defaultTypeNameStyle.fontSize = 11;
		m_defaultTypeNameStyle.alignment = TextAnchor.MiddleCenter;
		
		m_isInit = true;
	}
	
	public static Vector2 CalcSize()
	{
		Init();
		
		return new Vector2(m_tagBorderTexture.width, m_tagBorderTexture.height);
	}
	
	public static void Display(Rect bounds, BattleType battleType, GUIStyle typeNameStyle=null)
	{
		Init();
		
		if (typeNameStyle == null)
		{
			typeNameStyle = m_defaultTypeNameStyle;
		}
		
		Color typeColor = TypeColor.GetColor(battleType);
		GUIUtils.DrawGroup(bounds, delegate()
		{
			Rect tagBounds = new Rect(0, 0, m_tagBorderTexture.width, m_tagBorderTexture.height);
			Color prevColor = GUI.color;
			GUI.DrawTexture(tagBounds, m_tagBorderTexture);
			
			prevColor = GUI.color;
			GUI.color = typeColor;
			GUI.DrawTexture(tagBounds, m_tagTexture);
			GUI.color = prevColor;
			
			GUI.Label(tagBounds, battleType.ToString().ToUpper(), typeNameStyle);
		});
	}
}

