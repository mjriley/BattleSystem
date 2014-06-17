using UnityEngine;
using PokeCore;
using PokeCore.Pokemon;

using PokemonGUI;
using PokemonGUI.Controls;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {

public static class DetailElements
{
	static Texture2D m_backgroundTexture;
	static Texture2D m_maleTexture;
	static Texture2D m_femaleTexture;
	
	static GUIStyle m_nameStyle;
	
	static DetailElements()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/PokemonHeaderBar");
		m_maleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Male");
		m_femaleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Female");
		
		m_nameStyle = new GUIStyle();
		m_nameStyle.normal.textColor = Color.white;
		m_nameStyle.fontSize = 16;
		m_nameStyle.alignment = TextAnchor.MiddleLeft;
	}
	
	const int HEADER_WIDTH = 390;
	const int HEADER_HEIGHT = 32;
	const int HEADER_Y = 5;
	const int THUMBNAIL_X = 5;
	const int NAME_X = 40;
	const int GENDER_X = 180;
	const int TAG_X = 198;
	
	public static void Display(Rect bounds, Character pokemon, GUIStyle nameStyle=null)
	{
		if (nameStyle == null)
		{
			nameStyle = m_nameStyle;
		}
		
		GUIUtils.DrawGroup(bounds, delegate()
		{
			Rect headerBounds = new Rect((bounds.width - HEADER_WIDTH) / 2.0f, HEADER_Y, HEADER_WIDTH, HEADER_HEIGHT);
				
			GUIUtils.DrawGroup(headerBounds, delegate() {
				Texture2D thumbnailTexture = Pokemon.GetThumbnail(pokemon.Species);
				
				Color prevColor = GUI.color;
				GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
				GUI.DrawTexture(new Rect(0, 0, headerBounds.width, headerBounds.height), m_backgroundTexture);
				GUI.color = prevColor;
				
				GUI.DrawTexture(new Rect(THUMBNAIL_X, (headerBounds.height - thumbnailTexture.height) / 2.0f , thumbnailTexture.width, thumbnailTexture.height), thumbnailTexture);
				GUI.Label(new Rect(NAME_X, 0, 100, headerBounds.height), pokemon.Name, nameStyle);
				Texture2D genderTexture = (pokemon.Gender == Gender.Male) ? m_maleTexture : m_femaleTexture;
				GUI.DrawTexture(new Rect(GENDER_X, (headerBounds.height - genderTexture.height) / 2.0f, genderTexture.width, genderTexture.height), genderTexture);
				
				Vector2 typeSize = TypeTag.CalcSize();
				
				for (int i = 0; i < pokemon.Types.Count; ++i)
				{
					TypeTag.Display(new Rect(TAG_X + (typeSize.x + 2) * i, (headerBounds.height - typeSize.y) / 2.0f, typeSize.x, typeSize.y), pokemon.Types[i]);
				}
			});
		});
	}
}

}}}