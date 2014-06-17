using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PokeCore.Moves;

namespace PokemonGUI {
namespace Controls {

public static class MoveButton
{
	static Texture2D m_baseTexture;
	static Texture2D m_tagTexture;
	
	static GUIStyle m_defaultTypeNameStyle;
	static GUIStyle m_defaultMoveNameStyle;
	static GUIStyle m_defaultMoveDetailsStyle;
	static GUIStyle m_defaultButtonStyle;
	
	static MoveButton()
	{
		m_tagTexture = Resources.Load<Texture2D>("Textures/TypeTag");
		m_baseTexture = Resources.Load<Texture2D>("Textures/MoveButton");
		
		m_defaultTypeNameStyle = new GUIStyle();
		m_defaultTypeNameStyle.normal.textColor = Color.white;
		m_defaultTypeNameStyle.fontSize = 11;
		m_defaultTypeNameStyle.alignment = TextAnchor.MiddleCenter;
		
		m_defaultMoveNameStyle = new GUIStyle();
		m_defaultMoveNameStyle.fontSize = 16;
		m_defaultMoveNameStyle.alignment = TextAnchor.LowerCenter;
		m_defaultMoveNameStyle.contentOffset = new Vector2(0, -3);
		
		m_defaultMoveDetailsStyle = new GUIStyle();
		m_defaultMoveDetailsStyle.normal.textColor = Color.white;
		m_defaultMoveDetailsStyle.fontSize = 16;
		m_defaultMoveDetailsStyle.alignment = TextAnchor.UpperRight;
		m_defaultMoveDetailsStyle.contentOffset = new Vector2(-5, 2);
		
		m_defaultButtonStyle = new GUIStyle();
	}
	
	public static bool Display(Rect rect, Move move, bool drawReversed=false, 
		GUIStyle typeNameStyle=null, 
		GUIStyle moveNameStyle=null, 
		GUIStyle moveDetailsStyle=null,
		GUIStyle buttonStyle=null)
	{
		if (typeNameStyle == null)
		{
			typeNameStyle = m_defaultTypeNameStyle;
		}
		
		if (moveNameStyle == null)
		{
			moveNameStyle = m_defaultMoveNameStyle;
		}
		
		if (moveDetailsStyle == null)
		{
			moveDetailsStyle = m_defaultMoveDetailsStyle;
		}
		
		if (buttonStyle == null)
		{
			buttonStyle = m_defaultButtonStyle;
		}
		
		bool result = false;
		
		Color typeColor = TypeColor.GetColor(move.BattleType);
		
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
		Rect tagBounds = new Rect(leftBound, rect.height / 2.0f + moveDetailsStyle.contentOffset.y, m_tagTexture.width, m_tagTexture.height);
		
		GUI.BeginGroup(rect);
			Color previousColor = GUI.color;
			GUI.color = typeColor;
			GUI.DrawTexture(fullBounds, m_baseTexture);
			GUI.color = previousColor;
			
			TypeTag.Display(tagBounds, move.BattleType);
			
			GUI.Label(new Rect(0, 0, rect.width, rect.height / 2), move.Name, moveNameStyle);
			float detailsWidth = (drawReversed) ? rect.width : rect.width - 20;
			Rect detailBounds = new Rect(0, rect.height / 2.0f, detailsWidth, rect.height / 2);
			GUI.Label(detailBounds, "PP\t" + move.CurrentPP + " / " + move.MaxPP, moveDetailsStyle);
			result = GUI.Button(new Rect(0, 0, rect.width, rect.height), "", buttonStyle);
		GUI.EndGroup();
		
		return result;
	}
}

}}