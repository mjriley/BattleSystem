using UnityEngine;

using PokemonGUI;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
	using Controllers;
namespace Screens {

public class TurnActionDisplay : MonoBehaviour
{
	Texture2D m_backgroundTexture;
	
	Texture2D m_topBarTexture;
	Texture2D m_bottomBarTexture;
	
	Texture2D m_bagButtonTexture;
	Texture2D m_bagShadowTexture;
	
	Texture2D m_runButtonTexture;
	Texture2D m_runShadowTexture;
	
	Texture2D m_pokemonButtonTexture;
	Texture2D m_pokemonShadowTexture;
	
	Texture2D m_fightButtonTexture;
	
	GUIStyle m_buttonStyle;
	
	//IScreenController m_controller = new TurnActionController();
	IScreenController m_controller = null;
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/OptionBackground");
		m_topBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenTopEdge");
		m_bottomBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenEdge");
		
		m_bagButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Bag");
		m_bagShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/BagShadow");
		
		m_runButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Run");
		m_runShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/RunShadow");
		
		m_pokemonButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Pokemon");
		m_pokemonShadowTexture = Resources.Load<Texture2D>("Textures/Buttons/PokemonShadow");
		
		m_fightButtonTexture = Resources.Load<Texture2D>("Textures/Buttons/Fight");
		
		m_buttonStyle = new GUIStyle();
	}
	
	public void SetController(IScreenController controller)
	{
		m_controller = controller;
	}
	
	
	// Tuning Variables
	public int m_topBarHeight = 40;
	public int m_fightButtonGap = 2;
	
	void OnGUI()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Rect bagRect = new Rect(0.0f, bounds.height - m_bagButtonTexture.height, m_bagButtonTexture.width, m_bagButtonTexture.height);
			Rect runRect = new Rect((bounds.width - m_runButtonTexture.width) / 2.0f, bounds.height - m_runButtonTexture.height, m_runButtonTexture.width, m_runButtonTexture.height);
			Rect pokemonRect = new Rect(bounds.width - m_pokemonButtonTexture.width, bounds.height - m_pokemonButtonTexture.height, m_pokemonButtonTexture.width, m_pokemonButtonTexture.height);
			
			// Draw Background
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_backgroundTexture);
			
			// button shadows
			GUI.DrawTexture(bagRect, m_bagShadowTexture);
			GUI.DrawTexture(runRect, m_runShadowTexture);
			GUI.DrawTexture(pokemonRect, m_pokemonShadowTexture);
			
			// top bar
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, m_topBarTexture.height), m_topBarTexture);
			
			// bottom bar
			GUI.DrawTexture(new Rect(0.0f, bounds.height - m_bottomBarTexture.height, bounds.width, m_bottomBarTexture.height), m_bottomBarTexture);
			
			if (GUI.Button(new Rect((bounds.width - m_fightButtonTexture.width) / 2.0f, m_topBarHeight + m_fightButtonGap, 
				m_fightButtonTexture.width, m_fightButtonTexture.height), m_fightButtonTexture, m_buttonStyle))
			{
				m_controller.ProcessInput((int)TurnActionController.Options.Fight);
			}
			
			if (GUI.Button(bagRect, m_bagButtonTexture, m_buttonStyle))
			{
				m_controller.ProcessInput((int)TurnActionController.Options.Bag);
			}
			
			if (GUI.Button(runRect, m_runButtonTexture, m_buttonStyle))
			{
				m_controller.ProcessInput((int)TurnActionController.Options.Run);
			}
			
			if (GUI.Button(pokemonRect, m_pokemonButtonTexture, m_buttonStyle))
			{
				m_controller.ProcessInput((int)TurnActionController.Options.Pokemon);
			}
		});
	}
		
}


}}}}
