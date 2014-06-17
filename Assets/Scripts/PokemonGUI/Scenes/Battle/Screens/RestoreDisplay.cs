using UnityEngine;
using PokeCore;
using PokeCore.Pokemon;
using PokemonGUI;
using PokemonGUI.Controls;

using PokemonGUI.Scenes.Battle.Controllers;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
namespace Screens {

public class RestoreDisplay : MonoBehaviour
{
	Texture2D m_thumbnail;
	Texture2D m_background;
	Texture2D m_maleTexture;
	Texture2D m_femaleTexture;
	Texture2D m_nameBackdropTexture;
	
	public GUIStyle m_speciesStyle;
	public GUIStyle m_buttonStyle;
	
	Character m_pokemon;
	RestoreController m_controller;
	
	void Awake()
	{
		m_thumbnail = Pokemon.GetThumbnail(Species.Pikachu);
		m_background = Resources.Load<Texture2D>("Textures/ButtonLayout");
		
		m_maleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Male");
		m_femaleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Female");
		
		m_nameBackdropTexture = Resources.Load<Texture2D>("Textures/NameBackdrop");
	}
	
	public void SetController(RestoreController controller)
	{
		m_controller = controller;
	}
	
	void OnEnable()
	{
		if (m_controller == null)
		{
			PokemonDefinition def = PokemonDefinition.GetEntry(Species.Pikachu);
			m_pokemon = new Character("Test", def, Gender.Male, 50, null);
		}
		else
		{
			m_pokemon = m_controller.GetPokemon();
		}
		
		m_thumbnail = Pokemon.GetThumbnail(m_pokemon.Species);
	}
	
	public int m_labelY = 40;
	public int m_labelWidth = 180;
	public int m_labelHeight = 30;
	public int m_genderOffset = 10;
	
	public int m_thumbnailY = 75;
	
	public int m_healthY = 110;
	
	public int m_buttonsY = 200;
	public int m_buttonPaddingX = 5;
	public int m_buttonPaddingY = 8;
	public int m_buttonHeight = 30;
	public int m_buttonWidth = 165;
	
	public float m_opacity = 0.7f;
	
	void OnGUI()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0, 0, bounds.width, bounds.height), m_background);
			
			Texture2D genderTexture = (m_pokemon.Gender == Gender.Male) ? m_maleTexture : m_femaleTexture;
			Rect labelBounds = new Rect((bounds.width - m_labelWidth) / 2.0f, m_labelY, m_labelWidth, m_labelHeight);
			
			Color prevColor = GUI.color;
			GUI.color = new Color(1.0f, 1.0f, 1.0f, m_opacity);
			GUI.DrawTexture(labelBounds, m_nameBackdropTexture);
			GUI.color = prevColor;
			
			GUI.BeginGroup(labelBounds, m_pokemon.Species.ToString(), m_speciesStyle);
				GUI.DrawTexture(new Rect(labelBounds.width - genderTexture.width - m_genderOffset, (labelBounds.height - genderTexture.height) / 2.0f,
					genderTexture.width, genderTexture.height), genderTexture);
			GUI.EndGroup();
			
			GUI.DrawTexture(new Rect((bounds.width - m_thumbnail.width) / 2.0f, m_thumbnailY, m_thumbnail.width, m_thumbnail.height), m_thumbnail);
			
			Vector2 healthSize = HealthBar.CalcSize();
			HealthBar.Display(new Rect((bounds.width - healthSize.x) / 2.0f, m_healthY, healthSize.x, healthSize.y), m_pokemon.CurrentHP, m_pokemon.MaxHP);
			
			if (GUI.Button(new Rect(m_buttonPaddingX, m_buttonsY, m_buttonWidth, m_buttonHeight), "RESTORE HP/PP", m_buttonStyle))
			{
				m_controller.ProcessInput((int)RestoreController.Options.RestoreResource);
			}
			
			if (GUI.Button(new Rect(m_buttonPaddingX, m_buttonsY + m_buttonHeight + m_buttonPaddingY, m_buttonWidth, m_buttonHeight), "RESTORE STATUS", m_buttonStyle))
			{
				m_controller.ProcessInput((int)RestoreController.Options.RestoreStatus);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}


}}}}