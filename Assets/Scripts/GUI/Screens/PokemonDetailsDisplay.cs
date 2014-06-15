using UnityEngine;
using PokeCore;
using PokemonGUI;

public class PokemonDetailsDisplay : MonoBehaviour
{
	PokemonDetailsController m_controller;
	Texture2D m_background;
	Texture2D m_maleTexture;
	Texture2D m_femaleTexture;
	
	Character m_pokemon;
	
	public GUIStyle m_mainStyle;
	public GUIStyle m_summaryStyle;
	public GUIStyle m_nameStyle;
	public GUIStyle m_switchStyle;
	
	void Awake()
	{
		m_background = Resources.Load<Texture2D>("Textures/ButtonLayout");
		
		m_maleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Male");
		m_femaleTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonSwitch/Female");
	}
	
	public void SetController(PokemonDetailsController controller)
	{
		m_controller = controller;
	}
	
	void OnEnable()
	{
		m_pokemon = m_controller.GetPokemon();
	}
	
	public int paddingX = 10;
	public int startY = 40;
	public int mainButtonHeight = 100;
	public int subButtonHeight = 26;
	public int m_subButtonWidth = 140;
	
	public int restoreY = 150;
	public int summaryY = 190;
	public int m_restoreWidth;
	
	public int m_nameY = 10;
	public int m_nameX = 50;
	public int m_thumbnailY = 40;
	public int m_healthY = 80;
	public int m_switchY = 100;
	public int m_genderX = 200;
	
	void OnGUI()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0, 0, bounds.width, bounds.height), m_background);
			
			Rect buttonBounds = new Rect(paddingX, startY, bounds.width - paddingX * 2, mainButtonHeight);
			
			GUIUtils.DrawGroup(buttonBounds, delegate()
			{
				if (GUI.Button(new Rect(0, 0, buttonBounds.width, buttonBounds.height), "", m_mainStyle))
				{
					m_controller.ProcessInput((int)PokemonDetailsController.Options.Submit);
				}
				
				GUI.Label(new Rect(m_nameX, m_nameY, 100, 20), m_pokemon.Name, m_nameStyle);
				Texture2D genderTexture = (m_pokemon.Gender == Pokemon.Gender.Male ? m_maleTexture : m_femaleTexture);
				GUI.DrawTexture(new Rect(m_genderX, m_nameY, genderTexture.width, genderTexture.height), genderTexture);
				
				Texture2D thumbnail = Pokemon.Pokemon.GetThumbnail(m_pokemon.Species);
				GUI.DrawTexture(new Rect((buttonBounds.width - thumbnail.width) / 2.0f, m_thumbnailY, thumbnail.width, thumbnail.height), thumbnail);
				
				Vector2 healthSize = HealthBar.CalcSize();
				HealthBar.Display(new Rect((buttonBounds.width - healthSize.x) / 2.0f, m_healthY, healthSize.x, healthSize.y), m_pokemon.CurrentHP, m_pokemon.MaxHP);
				
				GUI.Label(new Rect(0, m_switchY, buttonBounds.width, 20), m_controller.GetDeployStatus().ToUpper(), m_switchStyle);
				
			});
			
			m_restoreWidth = (m_subButtonWidth + paddingX) * 2;
			
			GUIUtils.DrawEnabled(m_controller.RestoreEnabled(), delegate
			{
				if (GUI.Button(new Rect(paddingX, restoreY, m_restoreWidth, subButtonHeight), "RESTORE", m_summaryStyle))
				{
					m_controller.ProcessInput((int)PokemonDetailsController.Options.Restore);
				}
			});
			
			if (GUI.Button(new Rect(paddingX, summaryY, m_subButtonWidth, subButtonHeight), "SUMMARY", m_summaryStyle))
			{
				m_controller.ProcessInput((int)PokemonDetailsController.Options.Summary);
			}
			
			if (GUI.Button(new Rect(3 * paddingX + m_subButtonWidth, summaryY, m_subButtonWidth, subButtonHeight), "CHECK MOVES", m_summaryStyle))
			{
				m_controller.ProcessInput((int)PokemonDetailsController.Options.Moves);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}
