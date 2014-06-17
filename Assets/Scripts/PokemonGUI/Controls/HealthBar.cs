using UnityEngine;
using PokeCore;

namespace PokemonGUI {
namespace Controls {

public class HealthBar
{
	static bool m_isInit = false;
	
	static Texture2D m_healthBarTexture;
	static Texture2D m_healthTexture;
	
	const int HEALTH_WIDTH = 84;
	const int HEALTH_X = 14;
	const int HEALTH_BORDER_Y = 1;
	
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		m_healthBarTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/SwitchHPBar");
		m_healthTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Health");
		
		m_isInit = true;
	}
	
	public static Vector2 CalcSize()
	{
		Init();
		
		return new Vector2(m_healthBarTexture.width, m_healthBarTexture.height);
	}
	
	public static void Display(Rect bounds, int currentHP, int maxHP)
	{
		Init();
		
		GUIUtils.DrawGroup(bounds, delegate()
		{
			GUI.DrawTexture(new Rect(0, 0, m_healthBarTexture.width, m_healthBarTexture.height), m_healthBarTexture);
			
			float healthWidth = HEALTH_WIDTH * currentHP / (float)maxHP;
			GUI.DrawTexture(new Rect(HEALTH_X, HEALTH_BORDER_Y, healthWidth, m_healthTexture.height), m_healthTexture);
		});
	}
}

}}
