using UnityEngine;
using System.Collections;

public class AbilityButton : MonoBehaviour
{
	private static bool m_isInit = false;
	private static Texture2D m_baseTexture;
	
	private static int m_tagOffset = 40;
	
	private static void Init()
	{
		if (!m_isInit)
		{
			m_baseTexture = Resources.Load<Texture2D>("Textures/base_tab");
			m_isInit = true;
		}
	}
	
	public static bool Display(Rect rect, Ability ability, bool drawReversed, GUIStyle typeNameStyle, GUIStyle abilityNameStyle, GUIStyle abilityDetailsStyle)
	{
		Init();
		
		bool result = false;
		
		Rect fullBounds;
		if (drawReversed)
		{
			fullBounds = new Rect(rect.width, 0, -rect.width, rect.height);
		}
		else
		{
			fullBounds = new Rect(0, 0, rect.width, rect.height);
		}
		
		GUI.BeginGroup(rect);
			GUI.DrawTexture(fullBounds, m_baseTexture);
			GUI.Box(new Rect(m_tagOffset, rect.height / 2 + 10, 70, 25), ability.Type, typeNameStyle);
			GUI.Label(new Rect(0, 0, rect.width, rect.height / 2), ability.Name, abilityNameStyle);
			GUI.Label(new Rect(0, rect.height / 2, rect.width, rect.height / 2), "PP " + ability.CurrentUses + "/" + ability.MaxUses, abilityDetailsStyle);
			result = GUI.Button(new Rect(0, 0, rect.width, rect.height), "");
		GUI.EndGroup();
		
		return result;
	}
}