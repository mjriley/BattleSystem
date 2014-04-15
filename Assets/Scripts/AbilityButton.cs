using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityButton : MonoBehaviour
{
	private static bool m_isInit = false;
	private static Texture2D m_baseTexture;
	private static Texture2D m_tagTexture;
	
	private static int m_tagOffset = 40;
	
	private static Dictionary<string, Color> typeColors = new Dictionary<string, Color>();
	
	private static Color makeIntColor(int r, int g, int b)
	{
		return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
	}
	
	private static void Init()
	{
		if (!m_isInit)
		{
			m_baseTexture = Resources.Load<Texture2D>("Textures/base_tab");
			m_tagTexture = Resources.Load<Texture2D>("Textures/type_tag");
			m_isInit = true;
		}
		
		typeColors["Fire"] = makeIntColor(240, 128, 48);
		typeColors["Water"] = makeIntColor(104, 144, 240);
		typeColors["Grass"] = makeIntColor(120, 200, 80);
		typeColors["Flying"] = makeIntColor(168, 144, 240);
	}
	
	public static bool Display(Rect rect, Ability ability, bool drawReversed, GUIStyle typeNameStyle, GUIStyle abilityNameStyle, GUIStyle abilityDetailsStyle, GUIStyle buttonStyle)
	{
		Init();
		
		bool result = false;
		
		Color typeColor = typeColors[ability.Type];
		
		Rect fullBounds;
		if (drawReversed)
		{
			fullBounds = new Rect(rect.width, 0, -rect.width, rect.height);
		}
		else
		{
			fullBounds = new Rect(0, 0, rect.width, rect.height);
		}
		
		Rect tagBounds = new Rect(m_tagOffset, rect.height /2 + 10, 70, 25);
		
		GUI.BeginGroup(rect);
			Color previousColor = GUI.color;
			GUI.color = typeColor;
			GUI.DrawTexture(fullBounds, m_baseTexture);
			GUI.DrawTexture(tagBounds, m_tagTexture);
			GUI.color = previousColor;
			GUI.Label(tagBounds, ability.Type, typeNameStyle);
			GUI.Label(new Rect(0, 0, rect.width, rect.height / 2), ability.Name, abilityNameStyle);
			GUI.Label(new Rect(0, rect.height / 2, rect.width, rect.height / 2), "PP " + ability.CurrentUses + "/" + ability.MaxUses, abilityDetailsStyle);
			result = GUI.Button(new Rect(0, 0, rect.width, rect.height), "", buttonStyle);
		GUI.EndGroup();
		
		return result;
	}
}