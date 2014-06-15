using PokeCore;
using UnityEngine;

public class PokemonTagDisplay
{
	private static Texture2D m_backgroundTexture;
	private static Texture2D m_healthBackgroundTexture;
	private static Texture2D m_healthTexture;
	private static Texture2D m_healthBorderTexture;
	private static Texture2D m_maleTexture;
	private static Texture2D m_femaleTexture;
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
			m_healthBackgroundTexture = Resources.Load<Texture2D>("Textures/HealthBackGradient");
			m_healthTexture = Resources.Load<Texture2D>("Textures/HealthGradient");
			m_healthBorderTexture = Resources.Load<Texture2D>("Textures/health_border");
			m_maleTexture = Resources.Load<Texture2D>("Textures/male_tab");
			m_femaleTexture = Resources.Load<Texture2D>("Textures/female_tab");
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
	
	public static bool Button(Rect screenRect, Character pokemon, bool drawReversed, GUIStyle style, int nameOffsetX, int nameOffsetY, int statOffsetX, int statOffsetY, GUIStyle statStyle)
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
			if (pokemon == null)
			{
				Color prevColor = GUI.color;
				GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
				GUI.DrawTexture(bounds, m_backgroundTexture);
				GUI.color = prevColor;
			}
			else
			{
				GUI.DrawTexture(bounds, m_backgroundTexture);
				
				int fullHealthWidth = m_healthBorderTexture.width - 1 - 25;
				int currentHealthWidth = (int)(fullHealthWidth * (float)pokemon.CurrentHP / (float)pokemon.MaxHP);
				GUI.DrawTexture(new Rect(93 + 25, 29, fullHealthWidth, m_healthBackgroundTexture.height), m_healthBackgroundTexture);
				GUI.DrawTexture(new Rect(93 + 25, 29, currentHealthWidth, m_healthTexture.height), m_healthTexture);
				GUI.DrawTexture(new Rect(93, 28, m_healthBorderTexture.width, m_healthBorderTexture.height), m_healthBorderTexture);
				Texture2D thumbnail = Pokemon.Pokemon.GetThumbnail(pokemon.Species);
				GUI.DrawTexture(new Rect(17, 13, thumbnail.width, thumbnail.height), thumbnail);
				Vector2 nameBounds = style.CalcSize(new GUIContent(pokemon.Name));
				GUI.Label(new Rect(nameOffsetX, nameOffsetY, nameBounds.x, nameBounds.y), pokemon.Name, style);
				
				GUIContent levelContent = new GUIContent("Lv. " + pokemon.Level);
				Vector2 statBounds = statStyle.CalcSize(levelContent);
				
				GUIContent hpContent = new GUIContent(pokemon.CurrentHP + "/" + pokemon.MaxHP);
				Vector2 hpBounds = statStyle.CalcSize(hpContent);
				
				CustomGUI.BorderedLabel(new Rect(statOffsetX, screenRect.height - statBounds.y - statOffsetY, 100, statBounds.y), levelContent, 1, Color.black, statStyle);
				CustomGUI.BorderedLabel(new Rect(93 + m_healthBorderTexture.width - hpBounds.x, screenRect.height - hpBounds.y - statOffsetY, hpBounds.x, hpBounds.y), hpContent, 1, Color.black, statStyle);
				
				Texture2D genderTexture = (pokemon.Gender == Pokemon.Gender.Male) ? m_maleTexture : m_femaleTexture;
				GUI.DrawTexture(new Rect(93 + m_healthBorderTexture.width, nameOffsetY, 18, 18), genderTexture);
			}
			
			result = GUI.Button(normalBounds, "", m_emptyButtonStyle);
		GUI.EndGroup();
		
		return result;
	}
}
