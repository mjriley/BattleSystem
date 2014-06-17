using UnityEngine;
using PokeCore;

using PokemonGUI;
using PokemonGUI.Controls;
using PokemonGUI.Scenes.Battle;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Controllers;
namespace Screens {

public class ChoosePokemonDisplay : MonoBehaviour
{
	NewBattleSystem m_system;
	public GameObject m_systemController;
	GUIStyle m_emptyStyle = new GUIStyle();
	
	public int pokemonButtonGapY = 7;
	
	Texture2D m_backgroundTexture;
	
	ISelectPokemonController m_controller;
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
	}
	
	void Start()
	{
		m_system = m_systemController.GetComponent<BattleController>().GetSystem();
	}
	
	public void SetController(ISelectPokemonController controller)
	{
		m_controller = controller;
	}
	
	public int m_statusY = 200;
	public int m_statusWidth = 300;
	public int m_statusHeight = 20;
	public GUIStyle m_statusStyle;
	
	void OnGUI()
	{
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds) {
		
			for (int i = 0; i < 3; ++i)
			{
				int leftPokemonIndex = i * 2;
				if (leftPokemonIndex >= m_system.UserPlayer.Pokemon.Count)
				{
					break;
				}
				
				Vector2 buttonSize = PokemonSwitchButton.CalcMinSize(m_emptyStyle);
				
				Character leftPokemon = m_system.UserPlayer.Pokemon[leftPokemonIndex];
				if (leftPokemon != null)
				{
					Rect pokemonButtonLeft = new Rect(0, i * (buttonSize.y + pokemonButtonGapY), buttonSize.x, buttonSize.y);
					if (PokemonSwitchButton.Display(pokemonButtonLeft, leftPokemon))
					{
						m_controller.ProcessInput(leftPokemonIndex);
					}
				}
				
				int rightPokemonIndex = i * 2 + 1;
				if (rightPokemonIndex >= m_system.UserPlayer.Pokemon.Count)
				{
					break;
				}
				Character rightPokemon = m_system.UserPlayer.Pokemon[rightPokemonIndex];
				if (rightPokemon != null)
				{
					Rect pokemonButtonRight = new Rect(bounds.width - buttonSize.x, i * (buttonSize.y + pokemonButtonGapY), 
						buttonSize.x, buttonSize.y);
					if (PokemonSwitchButton.Display(pokemonButtonRight, rightPokemon, true))
					{
						m_controller.ProcessInput(rightPokemonIndex);
					}
				}
			}
			
			GUI.Box(new Rect(0, m_statusY, m_statusWidth, m_statusHeight), "Choose a Pokemon.", m_statusStyle);
		});
		
		bool prevEnabled = GUI.enabled;
		if (!m_controller.BackEnabled)
		{
			GUI.enabled = false;
		}
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
		GUI.enabled = prevEnabled;
	}
}

}}}}