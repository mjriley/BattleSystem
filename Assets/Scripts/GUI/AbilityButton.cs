using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AbilityButton : MonoBehaviour
{
	private static bool m_isInit = false;
	private static Texture2D m_baseTexture;
	private static Texture2D m_tagTexture;
	private static Texture2D m_tagBorderTexture;
	
	static GUIStyle m_defaultTypeNameStyle;
	static GUIStyle m_defaultAbilityNameStyle;
	static GUIStyle m_defaultAbilityDetailsStyle;
	static GUIStyle m_defaultButtonStyle;
	
	private static Dictionary<BattleType, Color> typeColors = new Dictionary<BattleType, Color>();
	
	private static Color makeIntColor(int r, int g, int b)
	{
		return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
	}
	
	private static void Init()
	{
		if (!m_isInit)
		{
			//m_baseTexture = Resources.Load<Texture2D>("Textures/base_tab");
			m_tagTexture = Resources.Load<Texture2D>("Textures/TypeTag");
			m_baseTexture = Resources.Load<Texture2D>("Textures/AbilityButton");
			m_tagBorderTexture = Resources.Load<Texture2D>("Textures/TypeTagBorder");
			
			m_defaultTypeNameStyle = new GUIStyle();
			m_defaultTypeNameStyle.normal.textColor = Color.white;
			m_defaultTypeNameStyle.fontSize = 11;
			m_defaultTypeNameStyle.alignment = TextAnchor.MiddleCenter;
			
			m_defaultAbilityNameStyle = new GUIStyle();
			m_defaultAbilityNameStyle.fontSize = 16;
			m_defaultAbilityNameStyle.alignment = TextAnchor.LowerCenter;
			m_defaultAbilityNameStyle.contentOffset = new Vector2(0, -3);
			
			m_defaultAbilityDetailsStyle = new GUIStyle();
			m_defaultAbilityDetailsStyle.normal.textColor = Color.white;
			m_defaultAbilityDetailsStyle.fontSize = 16;
			m_defaultAbilityDetailsStyle.alignment = TextAnchor.UpperRight;
			m_defaultAbilityDetailsStyle.contentOffset = new Vector2(-5, 2);
			
			m_defaultButtonStyle = new GUIStyle();
			
			m_isInit = true;
		}
		
		typeColors[BattleType.Fire] = makeIntColor(240, 128, 48);
		typeColors[BattleType.Water] = makeIntColor(104, 144, 240);
		typeColors[BattleType.Grass] = makeIntColor(120, 200, 80);
		typeColors[BattleType.Flying] = makeIntColor(168, 144, 240);
		typeColors[BattleType.Normal] = makeIntColor(168, 168, 120);
		typeColors[BattleType.Poison] = makeIntColor(160, 64, 160);
	}
	
	public static bool Display(Rect rect, AbstractAbility ability, bool drawReversed=false, 
		GUIStyle typeNameStyle=null, 
		GUIStyle abilityNameStyle=null, 
		GUIStyle abilityDetailsStyle=null,
		GUIStyle buttonStyle=null)
	{
		Init();
		
		if (typeNameStyle == null)
		{
			typeNameStyle = m_defaultTypeNameStyle;
		}
		
		if (abilityNameStyle == null)
		{
			abilityNameStyle = m_defaultAbilityNameStyle;
		}
		
		if (abilityDetailsStyle == null)
		{
			abilityDetailsStyle = m_defaultAbilityDetailsStyle;
		}
		
		if (buttonStyle == null)
		{
			buttonStyle = m_defaultButtonStyle;
		}
		
		bool result = false;
		
		Color typeColor = typeColors[ability.BattleType];
		
		Rect fullBounds;
		if (drawReversed)
		{
			fullBounds = new Rect(rect.width, 0, -rect.width, rect.height);
		}
		else
		{
			fullBounds = new Rect(0, 0, rect.width, rect.height);
		}
		
		float leftBound = (drawReversed) ? 20 : 0;
		Rect tagBounds = new Rect(leftBound, rect.height / 2.0f + abilityDetailsStyle.contentOffset.y, m_tagTexture.width, m_tagTexture.height);
		
		GUI.BeginGroup(rect);
			Color previousColor = GUI.color;
			GUI.color = typeColor;
			GUI.DrawTexture(fullBounds, m_baseTexture);
			GUI.color = previousColor;
			
			GUI.DrawTexture(tagBounds, m_tagBorderTexture);
			
			previousColor = GUI.color;
			GUI.color = typeColor;
			GUI.DrawTexture(tagBounds, m_tagTexture);
			GUI.color = previousColor;
			
			GUI.Label(tagBounds, ability.BattleType.ToString().ToUpper(), typeNameStyle);
			GUI.Label(new Rect(0, 0, rect.width, rect.height / 2), ability.Name, abilityNameStyle);
			float detailsWidth = (drawReversed) ? rect.width : rect.width - 20;
			Rect detailBounds = new Rect(0, rect.height / 2.0f, detailsWidth, rect.height / 2);
			GUI.Label(detailBounds, "PP\t" + ability.CurrentPP + " / " + ability.MaxPP, abilityDetailsStyle);
			result = GUI.Button(new Rect(0, 0, rect.width, rect.height), "", buttonStyle);
		GUI.EndGroup();
		
		return result;
	}
}