using UnityEngine;
using Pokemon;
using PokeCore;

public class MoveDetailsDisplay : MonoBehaviour
{
	Texture2D m_backgroundTexture;
	Texture2D m_solidTexture;
	Texture2D m_physicalTypeTexture;
	Texture2D m_separator;
	
	Character m_pokemon;
	MoveDetailsController m_controller;
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
		m_physicalTypeTexture = Resources.Load<Texture2D>("Textures/Move/physical");
		m_separator = Resources.Load<Texture2D>("Textures/Separator");
	}
	
	void OnEnable()
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
	}
	
	public void SetController(MoveDetailsController controller)
	{
		m_controller = controller;
	}
	
	public int m_boxY = 42;
	public int m_boxHeight = 142;
	
	public int m_moveNameX = 50;
	public int m_moveNameY = 5;
	public int m_tagX = 120;
	public int m_ppX = 250;
	public int m_numXPadding = 10;
	
	public int m_categoryX = 10;
	public int m_categoryY = 50;
	public int m_powerX = 300;
	public int m_powerValueX = 350;
	public int m_categoryValueX = 50;
	public int m_categoryValueY = 100;
	
	public int m_descY = 100;
	public int m_descHeight = 100;
	public int m_numFieldWidth = 50;
	
	public int m_typeTexX = 10;
	
	public GUIStyle m_textStyle;
	public GUIStyle m_numStyle; 
	public GUIStyle m_descStyle;
	
	public int m_separatorY = 40;
	public int m_separatorWidth = 350;
	
	public int m_separator2X = 5;
	public int m_separator2Y = 50;
	public int m_separator2Width = 100;
	
	public int m_separator3X = 200;
	
	void OnGUI()
	{
		Color prevColor;
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		DetailElements.Display(ScreenCoords.BottomScreen, m_pokemon);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Rect boxBounds = new Rect(0, m_boxY, bounds.width, m_boxHeight);
			GUIUtils.DrawGroup(boxBounds, delegate()
			{
				prevColor = GUI.color;
				GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
				GUI.DrawTexture(new Rect(0, 0, boxBounds.width, boxBounds.height), m_solidTexture);
				GUI.color = prevColor;
				
				Vector2 typeBounds = TypeTag.CalcSize();
				GUI.Label(new Rect(m_moveNameX, m_moveNameY, 100, typeBounds.y), "Leech Seed", m_textStyle);
				TypeTag.Display(new Rect(m_tagX, m_moveNameY, typeBounds.x, typeBounds.y), Moves.BattleType.Grass);
				GUI.Label(new Rect(m_ppX, m_moveNameY, 100, typeBounds.y), "PP", m_textStyle);
				GUI.Label(new Rect(boxBounds.width - m_numXPadding - m_numFieldWidth, m_moveNameY, m_numFieldWidth, typeBounds.y), "0/10", m_numStyle);
				
				GUI.DrawTexture(new Rect((boxBounds.width - m_separatorWidth) / 2.0f, m_separatorY, m_separatorWidth, 1), m_separator);
				
				GUI.Label(new Rect(m_categoryX, m_categoryY, 100, typeBounds.y), "CATEGORY", m_textStyle);
				GUI.Label(new Rect(m_powerX, m_categoryY, 100, typeBounds.y), "POWER", m_textStyle);
				GUI.Label(new Rect(boxBounds.width - m_numXPadding - m_numFieldWidth, m_categoryY, m_numFieldWidth, typeBounds.y), "50", m_numStyle);
				
				GUI.DrawTexture(new Rect(m_typeTexX, m_categoryValueY + (typeBounds.y - m_physicalTypeTexture.height) / 2.0f, m_physicalTypeTexture.width, m_physicalTypeTexture.height), m_physicalTypeTexture);
				GUI.Label(new Rect(m_categoryValueX, m_categoryValueY, 100, typeBounds.y), "PHYSICAL", m_textStyle);
				GUI.Label(new Rect(m_powerX, m_categoryValueY, 100, typeBounds.y), "ACCURACY", m_textStyle);
				GUI.Label(new Rect(boxBounds.width - m_numXPadding - m_numFieldWidth, m_categoryValueY, m_numFieldWidth, typeBounds.y), "100", m_numStyle);
				
				GUI.DrawTexture(new Rect(m_separator2X, m_separator2Y, m_separator2Width, 1), m_separator);
				GUI.DrawTexture(new Rect(m_separator3X, m_separator2Y, m_separator2Width, 1), m_separator);
				
				prevColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.green;
				GUI.Label(new Rect(0, m_descY, boxBounds.width, m_descHeight), "A physical attack in which the user charges and slams into the target with its whole body.", m_descStyle);
				GUI.backgroundColor = prevColor;
			});
		
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}
