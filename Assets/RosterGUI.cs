using UnityEngine;
using System.Collections;
using System;

public class RosterGUI : MonoBehaviour
{
	enum State
	{
		Main,
		List
	};
	
	public Vector2 scrollPosition = Vector2.zero;
	public GUIStyle style;
	
	public int scrollIndex = 0;
	public int m_rowHeight;
	public int selectedIndex = 0;
	int rowsDisplayed = 6;
	
	float keyPressedDelay = 0.2f; // ms
	float elapsed = 0.0f;
	
	bool m_arrowDownPressed = false;
	bool m_arrowUpPressed = false;
	
	State m_currentState = State.List;
	
	Pokemon.Species[] pokemon;
	
	Pokemon.Species[] species = new Pokemon.Species[] { 
		Pokemon.Species.Bulbasaur, Pokemon.Species.Charmander, 
		Pokemon.Species.Chespin, Pokemon.Species.Magikarp,
		Pokemon.Species.Pikachu, Pokemon.Species.Squirtle };
		
	public void Start()
	{
		pokemon = (Pokemon.Species[])Enum.GetValues(typeof(Pokemon.Species));
		elapsed = keyPressedDelay; // make sure key input is detected immediately
	}
		
	public void Update()
	{
		elapsed += Time.deltaTime;
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (elapsed >= keyPressedDelay || !m_arrowDownPressed)
			{
				int updatedRow = selectedIndex + 1;
				if (updatedRow >= rowsDisplayed)
				{
					if (updatedRow + scrollIndex < pokemon.Length)
					{
						++scrollIndex;
						scrollPosition.y = scrollIndex * m_rowHeight;
					}
				}
				else
				{
					selectedIndex = updatedRow;
				}
				
				m_arrowDownPressed = true;
				elapsed = 0.0f;
			}
		}
		else
		{
			m_arrowDownPressed = false;
		}
		
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			
			if (elapsed >= keyPressedDelay || !m_arrowUpPressed)
			{
				int updatedRow = selectedIndex - 1;
				if (updatedRow < 0)
				{
					scrollIndex = Mathf.Max(0, scrollIndex - 1);
					scrollPosition.y = scrollIndex * m_rowHeight;
				}
				else
				{
					selectedIndex = Math.Max(0, updatedRow);
				}
				
				m_arrowUpPressed = true;
				elapsed = 0.0f;
			}
		}
		else
		{
			m_arrowUpPressed = false;
		}
	}
		
	public void OnGUI()
	{
		if (m_currentState == State.Main)
		{
			float buttonWidth = Screen.width / 2.0f;
			float buttonHeight = Screen.height / 3.0f;
			for (int y = 0; y < 3; ++y)
			{
				for (int x = 0; x < 2; ++x)
				{
					int index = 2 * y + x;
					//GUI.Box(new Rect(x * buttonWidth, y * buttonHeight, buttonWidth, buttonHeight), "Pokemon " + (2 * y + x + 1));
					PokemonRosterButton.Display(new Rect(x * buttonWidth, y * buttonHeight, buttonWidth, buttonHeight), species[index]);
				}
			}
		}
		else if (m_currentState == State.List)
		{
			int numDisplayed = 6;
			GUIStyle scrollSkin = GUI.skin.GetStyle("VerticalScrollbar");
			float scrollWidth = GUI.skin.GetStyle("VerticalScrollbar").fixedWidth;
			int scrollMargin = scrollSkin.margin.left;
			m_rowHeight = Screen.height / numDisplayed;
			int rowWidth = (int)(Screen.width - scrollWidth - scrollMargin);
			
			scrollPosition = GUI.BeginScrollView(new Rect(0.0f, 0.0f, Screen.width, Screen.height), scrollPosition, new Rect(0.0f, 0.0f, rowWidth, pokemon.Length * m_rowHeight));
			
			for (int i = 0; i < pokemon.Length; ++i)
			{
				PokemonSelectionButton.Display(new Rect(0.0f, i * m_rowHeight, rowWidth, m_rowHeight), pokemon[i], (i == scrollIndex + selectedIndex));
			}
			
			GUI.EndScrollView();
		}
	}
}
