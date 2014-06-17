using UnityEngine;
using System.Collections.Generic;
using PokemonGUI;

using PokeGame;
using PokeCore.Pokemon;

namespace PokemonGUI {
namespace Scenes {
namespace Scores {

public class HighScoresDisplay : MonoBehaviour
{
	Texture2D m_solidTexture;
	const int POKEMON_TEXTURE_WIDTH = 32;
	const int POKEMON_TEXTURE_HEIGHT = 32;
	HighScores m_highScores;
	List<HighScore> m_scores;
	Dictionary<Species, Texture2D> m_thumbnails;
	
	public int m_yGap = 4;
	public int m_rowHeight = 40;
	public GUIStyle m_nameStyle;
	public GUIStyle m_winStyle;
	
	Vector2 m_scrollPosition = Vector2.zero;
	
	void Awake()
	{
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
		m_highScores = new HighScores();
		m_highScores.Load();
	}
	
	void Start()
	{
		m_scores = m_highScores.GetScores();
		
		m_thumbnails = new Dictionary<Species, Texture2D>();
		foreach (HighScore score in m_scores)
		{
			foreach (Species pokemon in score.Pokemon)
			{
				if (!m_thumbnails.ContainsKey(pokemon))
				{
					m_thumbnails[pokemon] = Pokemon.GetThumbnail(pokemon);
				}
			}
		}
	}
	
	void OnGUI()
	{
		DrawTopScreen();
		GUIUtils.DrawSeparatorBar();
		DrawBottomScreen();
	}
	
	public int winWidth = 45;
	public int headerHeight = 50;
	public GUIStyle headerStyle;
	public int m_startY = 5;
	
	void DrawTopScreen()
	{
		float scrollHeight = (m_rowHeight + m_yGap) * m_scores.Count;
		
		GUIUtils.DrawGroup(ScreenCoords.TopScreen, delegate(Rect bounds)
		{
			float scrollOffset = GUI.skin.verticalScrollbar.fixedWidth + GUI.skin.verticalScrollbar.margin.left;
			Rect scrollBounds = new Rect(0.0f, 0.0f, bounds.width - scrollOffset, scrollHeight);
			Color prevColor;
			
			// background
			prevColor = GUI.color;
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0, 0, bounds.width, bounds.height), m_solidTexture);
			GUI.color = prevColor;
			
			int borderSize = 1;
			int pokemonGap = 2;
			
			int numPokemon = 6;
			float pokemonWidth = (POKEMON_TEXTURE_WIDTH + borderSize * 2 + pokemonGap) * numPokemon;
			float nameWidth = scrollBounds.width - winWidth - pokemonWidth;
			
			// header
			GUI.Label(new Rect(0.0f, 0.0f, winWidth, headerHeight), "Wins", headerStyle);
			GUI.Label(new Rect(winWidth, 0.0f, nameWidth, headerHeight), "Name", headerStyle);
			GUI.Label(new Rect(winWidth + nameWidth, 0.0f, pokemonWidth, headerHeight), "Pokemon", headerStyle);
			
			int pokemonBoundsWidth = (POKEMON_TEXTURE_WIDTH + borderSize * 2 + pokemonGap) * numPokemon; 
			int pokemonBoundsHeight = POKEMON_TEXTURE_HEIGHT + borderSize * 2;
			
			m_scrollPosition = GUI.BeginScrollView(new Rect(0, headerHeight + m_startY, 
				bounds.width, bounds.height - headerHeight - m_startY), m_scrollPosition, scrollBounds);
				for (int i = 0; i < m_scores.Count; ++i)
				{
					int y = i * (m_rowHeight + m_yGap);
					GUI.DrawTexture(new Rect(0.0f, y, bounds.width, m_rowHeight), m_solidTexture);
					GUI.Label(new Rect(0, y, winWidth, m_rowHeight), m_scores[i].Wins.ToString(), m_winStyle);
					GUI.Label(new Rect(winWidth, y, nameWidth, m_rowHeight), m_scores[i].Name, m_nameStyle);
					
					GUIUtils.DrawGroup(new Rect(scrollBounds.width - pokemonBoundsWidth, y + (m_rowHeight - pokemonBoundsHeight) / 2.0f, pokemonBoundsWidth, pokemonBoundsHeight),
						delegate(Rect pokemonBounds)
						{
							for (int pokemonIndex=0; pokemonIndex < numPokemon; ++pokemonIndex)
							{
								prevColor = GUI.color;
								GUI.color = Color.black;
								float x = pokemonIndex * (POKEMON_TEXTURE_WIDTH + borderSize * 2 + pokemonGap);
								GUI.DrawTexture(new Rect(x, 0.0f, POKEMON_TEXTURE_WIDTH + borderSize * 2, pokemonBounds.height), m_solidTexture);
								GUI.color = prevColor;
								
								GUI.DrawTexture(new Rect(x + borderSize, borderSize, POKEMON_TEXTURE_WIDTH, POKEMON_TEXTURE_HEIGHT), m_solidTexture);
								if (pokemonIndex < m_scores[i].Pokemon.Count)
								{
									Species species = m_scores[i].Pokemon[pokemonIndex];
									GUI.DrawTexture(new Rect(x + borderSize, borderSize, POKEMON_TEXTURE_WIDTH, POKEMON_TEXTURE_HEIGHT), m_thumbnails[species]);
								}
							}
						});
				}
			GUI.EndScrollView();
		});
	}
	
	int buttonWidth = 120;
	int buttonHeight = 40;
	void DrawBottomScreen()
	{
		GUIUtils.DrawBottomScreenBackground(ScreenCoords.BottomScreen);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds) {
			if (GUI.Button(new Rect((bounds.width - buttonWidth) / 2.0f, (bounds.height - buttonHeight) / 2.0f, buttonWidth, buttonHeight), "Back To Menu"))
			{
				Application.LoadLevel("Start");
			}
		});
	}
}

}}}