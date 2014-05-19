using UnityEngine;
using System.Collections.Generic;

public class PokemonDetailsState : IDisplayState
{
	Rect m_topScreen = new Rect(0, 0, 400, 240);
	Rect m_bottomScreen = new Rect(0, 260, 400, 240);
	
	int colorWidth = 4;
	int tabWidth = 185;
	int tabHeight = 20;
	int cellSpacing = 1;
	
	static Color DarkGray = new Color32(51, 51, 51, 255);
	static Color LightGray = new Color32(97, 97, 97, 255);
	
	Texture2D m_backgroundTexture;
	Texture2D m_tabTexture;
	Texture2D m_solidColor;
	
	GUIStyle m_textStyle;
	
	Dictionary<Stat, Color> m_statColors = new Dictionary<Stat, Color>();
	Dictionary<Stat, string> m_statNames = new Dictionary<Stat, string>();
	Dictionary<Stat, int> m_statValues = new Dictionary<Stat, int>();
	List<Stat> m_statOrder = new List<Stat> {Stat.Attack, Stat.Defense, Stat.SpecialAttack, Stat.SpecialDefense, Stat.Speed};
	
	RosterController m_controller;
	PokemonPrototype m_activePokemon;
	
	public PokemonDetailsState(RosterController controller)
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/DetailsBackground");
		m_tabTexture = Resources.Load<Texture2D>("Textures/DetailsTabBackground");
		m_solidColor = Resources.Load<Texture2D>("Textures/white_tile");
		
		m_textStyle = new GUIStyle();
		m_textStyle.normal.textColor = Color.white;
		m_textStyle.fontSize = 16;
		m_textStyle.alignment = TextAnchor.MiddleLeft;
		
		m_statColors[Stat.HP] = new Color32(70, 189, 129, 255);
		m_statColors[Stat.Attack] = new Color32(235, 236, 119, 255);
		m_statColors[Stat.Defense] = new Color32(236, 161, 92, 255);
		m_statColors[Stat.SpecialAttack] = new Color32(92, 234, 236, 255);
		m_statColors[Stat.SpecialDefense] = new Color32(92, 136, 236, 255);
		m_statColors[Stat.Speed] = new Color32(177, 92, 236, 255);
		
		m_statNames[Stat.HP] = "HP";
		m_statNames[Stat.Attack] = "Attack";
		m_statNames[Stat.Defense] = "Defense";
		m_statNames[Stat.SpecialAttack] = "Sp. Atk";
		m_statNames[Stat.SpecialDefense] = "Sp. Def";
		m_statNames[Stat.Speed] = "Speed";
		
		m_controller = controller;
	}
	
	int hpLabelOffset = 20;
	int hpOffset = 90;
	int textOffset = 6;
	int statOffset = 110;
	
	void DrawTopScreen()
	{
		GUIUtils.DrawGroup(m_topScreen, delegate(Rect bounds) {
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_backgroundTexture);
			
			Color prevColor = GUI.color;
			
			GUI.color = m_statColors[Stat.HP];
			GUI.DrawTexture(new Rect(bounds.width - tabWidth - colorWidth, 25.0f, colorWidth, tabHeight), m_solidColor);
			GUI.color = DarkGray;
			GUI.DrawTexture(new Rect(bounds.width - tabWidth, 25.0f, tabWidth, tabHeight), m_tabTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect(bounds.width - tabWidth + hpLabelOffset, 25.0f, 40, tabHeight), m_statNames[Stat.HP], m_textStyle);
			int hp = m_statValues[Stat.HP];
			string hpString = string.Format("{0} / {0}", hp);
			GUI.Label(new Rect(bounds.width - tabWidth + hpOffset, 25.0f , 80, tabHeight), hpString, m_textStyle);
			
			int statGroupOffset = 60;
			
			for (int i = 0; i < m_statOrder.Count; ++i)
			{
				GUI.color = m_statColors[m_statOrder[i]];
				GUI.DrawTexture(new Rect(bounds.width - tabWidth - colorWidth, statGroupOffset + (tabHeight + cellSpacing) * i, colorWidth, tabHeight), m_solidColor);
				GUI.color = (i % 2 == 0) ? LightGray : DarkGray;
				GUI.DrawTexture(new Rect(bounds.width - tabWidth, statGroupOffset + (tabHeight + cellSpacing) * i, tabWidth, tabHeight), m_tabTexture);
				GUI.color = prevColor;
				GUI.Label(new Rect(bounds.width - tabWidth + textOffset, statGroupOffset + (tabHeight + cellSpacing) * i, 80, tabHeight), m_statNames[m_statOrder[i]], m_textStyle);
				GUI.Label(new Rect(bounds.width - tabWidth + statOffset, statGroupOffset + (tabHeight + cellSpacing) * i, 80, tabHeight), m_statValues[m_statOrder[i]].ToString(), m_textStyle);
			}
			
			GUI.color = prevColor;
		});
	}
	
	int buttonWidth = 120;
	int buttonHeight = 40;
	void DrawBottomScreen()
	{
		GUIUtils.DrawBottomScreenBackground(m_bottomScreen);
		GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds)
		{
			if (GUI.Button(new Rect((bounds.width - buttonWidth) / 2, (bounds.height - buttonHeight) / 2, buttonWidth, buttonHeight), "Back"))
			{
				m_controller.DisplayRoster();
			}
		
		});
	}
	
	public void SetActiveSlot(int slot)
	{
		m_activePokemon = m_controller.GetRosterSlot(slot);
		
		m_statValues[Stat.HP] = Pokemon.GetStat(m_activePokemon.Species, Stat.HP, m_activePokemon.Level);
		foreach (Stat stat in m_statOrder)
		{
			m_statValues[stat] = Pokemon.GetStat(m_activePokemon.Species, stat, m_activePokemon.Level);
		}
	}
	
	public void Update()
	{
	
	}
	
	public void Display()
	{
		DrawTopScreen();
		DrawBottomScreen();
	}
	
	public void OnEnter()
	{
	}
	
	public void OnLeave()
	{
	}
}
