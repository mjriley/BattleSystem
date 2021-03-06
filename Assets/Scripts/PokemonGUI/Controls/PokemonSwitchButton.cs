using UnityEngine;

using PokeCore;
using PokeCore.Pokemon;
using PokemonGUI;

namespace PokemonGUI {
namespace Controls {

public class PokemonSwitchButton
{
	static Texture2D m_backgroundTexture;
	static Texture2D m_hpFrameTexture;
	static Texture2D m_maleTexture;	
	static Texture2D m_femaleTexture;
	static Texture2D m_healthTexture;
	
	
	static GUIStyle m_nameStyle;
	static GUIStyle m_detailsStyle;
	static GUIStyle m_emptyStyle;
	
	const int HEALTH_WIDTH = 84;
	
	static PokemonSwitchButton()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/PokemonSwitchButton");
		m_hpFrameTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/SwitchHPBar");
		m_maleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Male");
		m_femaleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Female");
		m_healthTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Health");
		
		m_nameStyle = new GUIStyle();
		m_nameStyle.fontSize = 16;
		
		m_detailsStyle = new GUIStyle();
		m_detailsStyle.fontSize = 12;
		m_detailsStyle.normal.textColor = Color.white;
		m_detailsStyle.fontStyle = FontStyle.Bold;
		
		m_emptyStyle = new GUIStyle();
	}
	
	public static Vector2 CalcMinSize(GUIStyle style)
	{
		return new Vector2(m_backgroundTexture.width, m_backgroundTexture.height);
	}
	
	public static bool Display(Rect bounds, Character pokemon, bool drawReversed=false)
	{
		bool pressed = false;
		
		GUIUtils.DrawGroup(bounds, delegate(Rect group)
		{
			Rect backBounds;
			if (drawReversed)
			{
				backBounds = new Rect(m_backgroundTexture.width, 0.0f, -m_backgroundTexture.width, m_backgroundTexture.height);
			}
			else
			{
				backBounds = new Rect(0.0f, 0.0f, m_backgroundTexture.width, m_backgroundTexture.height);
			}
			
			if (GUI.Button(new Rect(0, 0, group.width, group.height), "", m_emptyStyle))
			{
				pressed = true;
			}
			
			GUI.DrawTexture(backBounds, m_backgroundTexture);
			
			float x = drawReversed ? 11 : 0;
			
			GUIUtils.DrawGroup(new Rect(x, 0, group.width - 11, group.height), delegate(Rect innerBounds) {
				Texture2D thumbnail = Pokemon.GetThumbnail(pokemon.Species);
				int thumbOffsetX = 2;
				int detailsOffsetY = 33;
				int startNameX = thumbOffsetX + thumbnail.width + 5;
				GUI.DrawTexture(new Rect(thumbOffsetX, 4, thumbnail.width, thumbnail.height), thumbnail);
				GUI.Label(new Rect(startNameX, 7, 100, 30), pokemon.Name, m_nameStyle);
				GUIContent level = new GUIContent("Lv. " + pokemon.Level.ToString());
				Vector2 detailSize = m_detailsStyle.CalcSize(level);
				CustomGUI.BorderedLabel(new Rect(thumbOffsetX + 10, detailsOffsetY, detailSize.x, detailSize.y), level, 1, Color.black, m_detailsStyle);
				
				GUI.DrawTexture(new Rect(70, 26, m_hpFrameTexture.width, m_hpFrameTexture.height), m_hpFrameTexture);
				float healthWidth = HEALTH_WIDTH * pokemon.CurrentHP / (float)pokemon.MaxHP;
				GUI.DrawTexture(new Rect(84, 27, healthWidth, m_healthTexture.height), m_healthTexture);
				
				GUIContent health = new GUIContent(pokemon.CurrentHP + " / " + pokemon.MaxHP);
				Vector2 healthSize = m_detailsStyle.CalcSize(health);
				CustomGUI.BorderedLabel(new Rect(70 + m_hpFrameTexture.width - healthSize.x, detailsOffsetY, healthSize.x, detailSize.y), health, 1, Color.black, m_detailsStyle);
				
				Texture2D genderTexture = (pokemon.Gender == Gender.Male) ? m_maleTexture : m_femaleTexture;
				GUI.DrawTexture(new Rect(168, 10, 13, 13), genderTexture);
			});
			
		});
		
		return pressed;
	}
}

}}