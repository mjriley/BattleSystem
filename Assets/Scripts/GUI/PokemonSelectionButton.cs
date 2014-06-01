using UnityEngine;

public class PokemonSelectionButton
{
	static bool isInit = false;
	static GUIStyle speciesStyle;
	static Texture2D m_texture;
	static Texture2D m_glow;
	
	static void Init()
	{
		if (!isInit)
		{
			// load textures
			isInit = true;
			
			speciesStyle = new GUIStyle();
			speciesStyle.alignment = TextAnchor.MiddleLeft;
			speciesStyle.fontSize = 16;
			
			m_texture = Resources.Load<Texture2D>("Textures/PokemonSelectionButton");
			m_glow = Resources.Load<Texture2D>("Textures/PokemonSelectionButtonGlow");
		}
	}
	
	public static void Display(Rect parent, Pokemon.Species species, bool selected)
	{
		Init();
		
		Texture2D thumbnail = Pokemon.Pokemon.GetThumbnail(species);
		float thumbnailX = 25.0f;
		float thumbnailY = (m_texture.height - thumbnail.height) / 2.0f;
		float nameX = thumbnailX + thumbnail.width + 10.0f;
		
		GUI.BeginGroup(parent);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, m_texture.width, m_texture.height), m_texture);
			GUI.DrawTexture(new Rect(thumbnailX, thumbnailY, thumbnail.width, thumbnail.height), thumbnail);
			GUI.Label(new Rect(nameX, 0.0f, 100, m_texture.height), species.ToString(), speciesStyle);
			if (selected)
			{
				GUI.DrawTexture(new Rect(0.0f, 0.0f, m_texture.width, m_texture.height), m_glow);
			}
		GUI.EndGroup();
	}
}
