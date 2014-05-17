using UnityEngine;
using System.Collections;

public class PlayerStatusDisplay
{
	private static bool m_isInit = false;
	
	private static Texture2D m_maleTexture;
	private static Texture2D m_femaleTexture;
	private static Texture2D m_healthTexture;
	private static Texture2D m_healthOutlineTexture;
	
	private static Texture2D m_normalBallTexture;
	private static Texture2D m_statusBallTexture;
	private static Texture2D m_deadBallTexture;
	
	public float CurrentHealth { get; set; }
	
	public bool DisplayBalls { get; set; }
	
	public bool Enabled { get; set; }
	
	private Rect m_bounds;
	
	public Player ActivePlayer { get; set; }
	
	public PlayerStatusDisplay(Rect bounds)
	{
		Init();
		
		DisplayBalls = true;
		Enabled = true;
		
		m_bounds = bounds;
	}
	
	public void UpdatePokemon()
	{
		CurrentHealth = ActivePlayer.ActivePokemon.CurrentHP;
	}
	
	public void Display(GUIStyle style)
	{
		if (ActivePlayer == null || Enabled == false)
		{
			return;
		}
		
		Character pokemon = ActivePlayer.ActivePokemon;
		
		GUIContent playerContent = new GUIContent(pokemon.Name);
		Vector2 contentSize = style.CalcSize(playerContent);
		
		GUIContent lvlContent = new GUIContent("Lv. 35");
		Vector2 lvlSize = style.CalcSize(lvlContent);
		
		float genderIconSize = contentSize.y;
		
		float maxWidth = m_healthOutlineTexture.width;
		float nameBarWidth = contentSize.x + genderIconSize + lvlSize.x;
		
		Texture2D genderTexture = (pokemon.Gender == Pokemon.Gender.Male) ? m_maleTexture : m_femaleTexture;
		
		GUI.BeginGroup(m_bounds);
			GUI.BeginGroup(new Rect((maxWidth - nameBarWidth) / 2.0f, 0, maxWidth, contentSize.y));
			
				GUI.Label(new Rect(0, 0, contentSize.x, contentSize.y), playerContent, style);
				GUI.DrawTexture(new Rect(contentSize.x, 0, genderIconSize, genderIconSize), genderTexture);
				GUI.Label(new Rect(contentSize.x + genderIconSize, 0, lvlSize.x, lvlSize.y), lvlContent, style);
			
			GUI.EndGroup();
			
			GUI.DrawTexture(new Rect(0, contentSize.y, m_healthOutlineTexture.width, m_healthOutlineTexture.height), m_healthOutlineTexture);
			Color prevColor = GUI.color;
			GUI.color = new Color(0, 1.0f, 0);
			float healthWidth = 81.0f;
			float healthRatio = (float)CurrentHealth / (float)pokemon.MaxHP;
			GUI.DrawTexture(new Rect(53, contentSize.y + 5, healthWidth * healthRatio, 14), m_healthTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect(0, contentSize.y + m_healthOutlineTexture.height, maxWidth, contentSize.y), (int)CurrentHealth + "/" + pokemon.MaxHP, style);
			
			if (DisplayBalls)
			{
				for (int i = 0; i < ActivePlayer.Pokemon.Count; ++i)
				{	
					Texture2D texture;
					if (ActivePlayer.Pokemon[i].isDead())
					{
						texture = m_deadBallTexture;
					}
					else if (ActivePlayer.Pokemon[i].IsStatusAfflicted())
					{
						texture = m_statusBallTexture;
					}
					else
					{
						texture = m_normalBallTexture;
					}
					
					GUI.DrawTexture(new Rect(i * texture.width, contentSize.y * 2 + texture.height, texture.width, texture.height), texture);
				}
			}
		GUI.EndGroup();
	}
	
	private static void Init()
	{
		if (!m_isInit)
		{
			m_maleTexture = Resources.Load<Texture2D>("Textures/male");
			m_femaleTexture = Resources.Load<Texture2D>("Textures/female");
			m_healthTexture = Resources.Load<Texture2D>("Textures/white_tile");
			m_healthOutlineTexture = Resources.Load<Texture2D>("Textures/HealthOutline");
			m_normalBallTexture = Resources.Load<Texture2D>("Textures/normal_ball");
			m_statusBallTexture = Resources.Load<Texture2D>("Textures/status_ball");
			m_deadBallTexture = Resources.Load<Texture2D>("Textures/dead_ball");
			
			m_isInit = true;
		}
	}
	
	public static Vector2 CalcMinSize(GUIStyle style)
	{
		Init();
		
		GUIContent content = new GUIContent("Any Text");
		Vector2 textSize = style.CalcSize(content);
		
		float width = m_healthOutlineTexture.width;
		float height = textSize.y * 2 + m_healthOutlineTexture.height + m_normalBallTexture.height;
		
		return new Vector2(width, height);
	}
	
	public static void Display(Rect screenRect, Character pokemon, Player player, int currentHP, GUIStyle style)
	{
		Init();
		
		GUIContent playerContent = new GUIContent(pokemon.Name);
		Vector2 contentSize = style.CalcSize(playerContent);
		
		GUIContent lvlContent = new GUIContent("Lv. 35");
		Vector2 lvlSize = style.CalcSize(lvlContent);
		
		float genderIconSize = contentSize.y;
		
		float maxWidth = m_healthOutlineTexture.width;
		float nameBarWidth = contentSize.x + genderIconSize + lvlSize.x;
		
		Texture2D genderTexture = (pokemon.Gender == Pokemon.Gender.Male) ? m_maleTexture : m_femaleTexture;
		
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
			float healthRatio = (float)currentHP / (float)pokemon.MaxHP;
			GUI.DrawTexture(new Rect(53, contentSize.y + 5, healthWidth * healthRatio, 14), m_healthTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect(0, contentSize.y + m_healthOutlineTexture.height, maxWidth, contentSize.y), currentHP + "/" + pokemon.MaxHP, style);
			
			for (int i = 0; i < player.Pokemon.Count; ++i)
			{	
				Texture2D texture;
				if (player.Pokemon[i].isDead())
				{
					texture = m_deadBallTexture;
				}
				else if (player.Pokemon[i].IsStatusAfflicted())
				{
					texture = m_statusBallTexture;
				}
				else
				{
					texture = m_normalBallTexture;
				}
				
				GUI.DrawTexture(new Rect(i * texture.width, contentSize.y * 2 + texture.height, texture.width, texture.height), texture);
			}
		GUI.EndGroup();
	}
}