using UnityEngine;
using PokeCore;
using Moves;
using Pokemon;
using System.Collections.Generic;
using PokemonGUI;

public class MoveDisplay : MonoBehaviour, IGameScreen
{
	Texture2D m_backgroundTexture;
	Texture2D m_topBarTexture;
	
	IMoveController m_controller;
	
	List<Move> m_moves;
	protected Character m_pokemon;
	
	protected void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_topBarTexture = Resources.Load<Texture2D>("Textures/bottomScreenTopEdge");
	}
	
	void OnEnable()
	{
		Invalidate();
	}
	
	public void Invalidate()
	{
		if (m_controller == null)
		{
			PlayerRoster roster = GameObject.Find("PlayerRoster").GetComponent<PlayerRoster>();
			PokemonPrototype proto = roster.GetRosterSlot(0);
			
			m_pokemon = PokemonFactory.CreatePokemon(proto.Species, 50, "", Gender.Male);
		}
		else
		{
			m_pokemon = m_controller.GetPokemon();
		}
		
		m_moves = m_pokemon.getMoves();
	}
	
	public void SetController(IMoveController controller)
	{
		m_controller = controller;
	}
	
	// Tweaking variables
	public int m_moveVGap = 20;
	public int moveButtonGapX = 10;
	public int moveButtonGapY = 10;
	public int moveButtonHeight = 60;
	
	protected void OnGUI()
	{
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, m_topBarTexture.height), m_topBarTexture);
			
			int initialY = m_topBarTexture.height + m_moveVGap;
			
			Rect buttonBounds = new Rect(0, initialY, (bounds.width - moveButtonGapX) / 2.0f, moveButtonHeight);
			if (MoveButton.Display(buttonBounds, m_moves[0]))
			{
				m_controller.ProcessInput(0);
			}
			
			buttonBounds = new Rect((bounds.width + moveButtonGapX) / 2.0f, initialY, (bounds.width - moveButtonGapX) / 2.0f, moveButtonHeight);
			if (MoveButton.Display(buttonBounds, m_moves[1], true))
			{
				m_controller.ProcessInput(1);
			}
		
			buttonBounds = new Rect(0, initialY + moveButtonHeight + moveButtonGapY, (bounds.width - moveButtonGapX) / 2.0f, moveButtonHeight);
			if (MoveButton.Display(buttonBounds, m_moves[2]))
			{
				m_controller.ProcessInput(2);
			}
			
			buttonBounds = new Rect((bounds.width + moveButtonGapX) / 2.0f, initialY + moveButtonHeight + moveButtonGapY, (bounds.width - moveButtonGapX) / 2.0f, moveButtonHeight);
			if (MoveButton.Display(buttonBounds, m_moves[3], true))
			{
				m_controller.ProcessInput(3);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}
