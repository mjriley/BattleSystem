using UnityEngine;
using System.Collections;

public class PlayerStatusDisplay : MonoBehaviour
{
	private static bool m_isInit = false;
	
	private static Texture2D m_maleTexture;
	private static Texture2D m_femaleTexture;
	private static Texture2D m_healthTexture;
	private static Texture2D m_healthOutlineTexture;
	
	private static void Init()
	{
		if (!m_isInit)
		{
			m_maleTexture = Resources.Load<Texture2D>("Textures/male");
			m_femaleTexture = Resources.Load<Texture2D>("Textures/female");
			m_healthTexture = Resources.Load<Texture2D>("Textures/white_tile");
			m_healthOutlineTexture = Resources.Load<Texture2D>("Textures/HealthOutline");
			
			m_isInit = true;
		}
	}
	
	public static Vector2 CalcMinSize(GUIStyle style)
	{
		Init();
		
		GUIContent content = new GUIContent("Any Text");
		Vector2 textSize = style.CalcSize(content);
		
		float width = m_healthOutlineTexture.width;
		float height = textSize.y * 2 + m_healthOutlineTexture.height;
		
		return new Vector2(width, height);
	}
	
	public static void Display(Rect screenRect, Character pokemon, GUIStyle style)
	{
		Init();
		
		GUIContent playerContent = new GUIContent(pokemon.Name);
		Vector2 contentSize = style.CalcSize(playerContent);
		
		GUIContent lvlContent = new GUIContent("Lv. 35");
		Vector2 lvlSize = style.CalcSize(lvlContent);
		
		float genderIconSize = contentSize.y;
		
		float maxWidth = m_healthOutlineTexture.width;
		float nameBarWidth = contentSize.x + genderIconSize + lvlSize.x;
		
		Texture2D genderTexture = (pokemon.Gender == Character.Sex.Male) ? m_maleTexture : m_femaleTexture;
		
		GUI.BeginGroup(screenRect);
			GUI.BeginGroup(new Rect((maxWidth - nameBarWidth) / 2, 0, maxWidth, contentSize.y));
			
				GUI.Label(new Rect(0, 0, contentSize.x, contentSize.y), playerContent, style);
				GUI.DrawTexture(new Rect(contentSize.x, 0, genderIconSize, genderIconSize), genderTexture);
				GUI.Label(new Rect(contentSize.x + genderIconSize, 0, lvlSize.x, lvlSize.y), lvlContent, style);
			
			GUI.EndGroup();
			
			GUI.DrawTexture(new Rect(0, contentSize.y, m_healthOutlineTexture.width, m_healthOutlineTexture.height), m_healthOutlineTexture);
			Color prevColor = GUI.color;
			GUI.color = new Color(0, 1.0f, 0);
			float healthWidth = 81.0f;
			float healthRatio = (float)pokemon.CurrentHP / (float)pokemon.MaxHP;
			GUI.DrawTexture(new Rect(53, contentSize.y + 5, healthWidth * healthRatio, 14), m_healthTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect(0, contentSize.y + m_healthOutlineTexture.height, maxWidth, contentSize.y), "100/100", style);
		GUI.EndGroup();
	}
}