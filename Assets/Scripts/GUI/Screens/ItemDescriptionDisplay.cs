using UnityEngine;
using PokeCore;
using Items;
using System.Collections.Generic;

public class ItemDescriptionDisplay : MonoBehaviour
{
	IItemDescriptionController m_controller;
	KeyValuePair<IItem, int> m_currentItem;
	
	Texture2D m_texture;
	Texture2D m_backgroundTexture;
	Texture2D m_buttonTexture;
	Texture2D m_separator;
	
	public int m_buttonWidth = 200;
	public int m_buttonHeight = 20;
	public int m_buttonPaddingY = 10;
	
	public int m_boxStartY = 40;
	public int m_boxHeight = 120;
	
	public int m_titleOffsetY = 5;
	public int m_titleX = 40;
	public int m_countX = 300;
	
	public int m_descOffsetX = 10;
	public int m_descStartY = 80;
	public int m_descWidth = 140;
	
	public GUIStyle m_textStyle;
	GUIStyle m_buttonStyle;
	
	void Start()
	{
		m_texture = Resources.Load<Texture2D>("Textures/white_tile");
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_buttonTexture = Resources.Load<Texture2D>("Textures/Buttons/ItemUseButton");
		m_separator = Resources.Load<Texture2D>("Textures/Separator");
		m_buttonStyle = new GUIStyle();
		m_buttonStyle.normal.background = m_buttonTexture;
		m_buttonStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnEnable()
	{
		m_currentItem = m_controller.GetItem();
	}
	
	public void SetController(IItemDescriptionController controller)
	{
		m_controller = controller;
	}
	
	public int separatorWidth = 200;
	public int separatorPadding = 5; 
	
	void OnGUI()
	{
		Vector2 backButtonSize = BackButton.CalcSize();
		
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds) {
			GUI.DrawTexture(new Rect(0, 0, bounds.width, bounds.height), m_backgroundTexture);
			
			Color prevColor = GUI.color;
			
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
			GUI.DrawTexture(new Rect(0.0f, m_boxStartY, bounds.width, m_boxHeight), m_texture);
			GUI.color = prevColor;
			
			float buttonOffsetX = (bounds.width - backButtonSize.x - m_buttonWidth) / 2.0f;
			
			GUIContent content = new GUIContent("x\t" + m_currentItem.Value);
			Vector2 countSize = m_textStyle.CalcSize(content);
			
			GUI.DrawTexture(new Rect((bounds.width - separatorWidth) / 2.0f, m_boxStartY + m_titleOffsetY - separatorPadding, 
				separatorWidth, 1), m_separator);
			GUI.DrawTexture(new Rect((bounds.width - separatorWidth) / 2.0f, m_boxStartY + m_titleOffsetY + separatorPadding + countSize.y,
				separatorWidth, 1), m_separator);
			GUI.Label(new Rect(m_titleX, m_boxStartY + m_titleOffsetY, 100, countSize.y), m_currentItem.Key.Name, m_textStyle);
			
			
			GUI.Label(new Rect(m_countX, m_boxStartY + m_titleOffsetY, countSize.x, countSize.y), content, m_textStyle);
			GUI.Label(new Rect(m_descOffsetX, m_descStartY, m_descWidth, 100), m_currentItem.Key.Desc, m_textStyle);
			
			if (GUI.Button(new Rect(buttonOffsetX, bounds.height - m_buttonHeight - m_buttonPaddingY, m_buttonWidth, m_buttonHeight), "USE", m_buttonStyle))
			{
				m_controller.ProcessInput(0);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}

