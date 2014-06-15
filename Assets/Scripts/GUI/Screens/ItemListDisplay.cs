using UnityEngine;
using System;
using System.Collections.Generic;
using PokeCore;
using PokemonGUI;
using Items;

public class ItemListDisplay : MonoBehaviour
{
	public int m_buttonWidth = 190;
	public int m_buttonHeight = 40;
	public int m_buttonSpacingY = 10;
	public int m_startY = 20;
	
	public GUIStyle m_testStyle;
	public GUIStyle m_statusStyle;
	
	ItemListController m_controller;
	List<KeyValuePair<IItem, int>> m_items = new List<KeyValuePair<IItem, int>>() {};
	string m_categoryName = "";
	int m_currentPage = 1;
	int m_totalPages = 1;
	
	const int ITEMS_PER_ROW = 2;
	
	Texture2D m_background;
	
	Texture2D m_solidTexture;
	
	void Awake()
	{
		m_background = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
	}
	
	void OnEnable()
	{
		Invalidate();
	}
	
	public void Invalidate()
	{
		m_items = m_controller.GetItems();
		m_categoryName = m_controller.GetCategoryName();
		m_currentPage = m_controller.GetCurrentPage();
		m_totalPages = m_controller.GetTotalPages();
	}
	
	public void SetController(ItemListController controller)
	{
		m_controller = controller;
	}

	public int m_staticX = 40;
	public int m_countX = 40;
	public int m_countY = 20;
	public int m_countHeight = 20;
	public GUIStyle m_countStyle;
	
	public int m_catNameX = 200;
	public int m_catNameY = 160;
	public int m_catNameWidth = 80;
	public int m_catNameHeight = 60;
	
	public int m_pageCountX = 300;
	
	void OnGUI()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			Color prevColor = GUI.color;
			GUI.color = Color.gray;
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_background);
			GUI.color = prevColor;
			
			float halfScreenWidth = bounds.width / 2;
			float offsetX = (halfScreenWidth - m_buttonWidth) / 2.0f;
			
			bool prevEnabled = GUI.enabled;
			
			for (int row = 0; row < 3; ++row)
			{
				int leftIndex = row * ITEMS_PER_ROW;
				string leftItemName = "";
				string leftCount = "";
				if (leftIndex < m_items.Count)
				{
					leftItemName = m_items[leftIndex].Key.Name;
					leftCount = m_items[leftIndex].Value.ToString();
				}
				else
				{
					GUI.enabled = false;
				}
				
				Rect buttonBounds = new Rect(offsetX, m_startY + (m_buttonSpacingY + m_buttonHeight) * row,
					m_buttonWidth, m_buttonHeight);
				GUIUtils.DrawGroup(buttonBounds, delegate()
				{
					if (GUI.Button(new Rect(0, 0, buttonBounds.width, buttonBounds.height), leftItemName, m_testStyle))
					{
						m_controller.ProcessInput(leftIndex);
					}
					
					GUIContent xContent = new GUIContent("x");
					Vector2 xSize = m_countStyle.CalcSize(xContent);
					
					GUI.Label(new Rect(m_staticX, m_countY, xSize.x, xSize.y), xContent, m_countStyle);
					if (leftItemName != "")
					{
						GUI.Label(new Rect(m_countX, m_countY, 40, xSize.y), leftCount, m_countStyle);
					}
				});
				
					
				int rightIndex = leftIndex + 1;
				string rightItemName = "";
				string rightCount = "";
				if (rightIndex < m_items.Count)
				{
					rightItemName = m_items[rightIndex].Key.Name;
					rightCount = m_items[rightIndex].Value.ToString();
				}
				else
				{
					GUI.enabled = false;
				}
				buttonBounds = new Rect(halfScreenWidth + offsetX, m_startY + (m_buttonSpacingY + m_buttonHeight) * row, 
					m_buttonWidth, m_buttonHeight);
				GUIUtils.DrawGroup(buttonBounds, delegate()
				{
					if (GUI.Button(new Rect(0, 0, buttonBounds.width, buttonBounds.height), rightItemName, m_testStyle))
					{
						m_controller.ProcessInput(rightIndex);
					}
					
					GUIContent xContent = new GUIContent("x");
					Vector2 xSize = m_countStyle.CalcSize(xContent);
					
					GUI.Label(new Rect(m_staticX, m_countY, xSize.x, xSize.y), xContent, m_countStyle);
					if (rightItemName != "")
					{
						GUI.Label(new Rect(m_countX, m_countY, 40, xSize.y), rightCount, m_countStyle);
					}
				});
			}
			
			GUI.enabled = prevEnabled;
			
			
			Vector2 arrowSize = NavigationArrow.CalcSize();
			prevColor = GUI.color;
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
			GUI.DrawTexture(new Rect(0, Common3DS.ArrowStartY, bounds.width, arrowSize.y), m_solidTexture);
			GUI.color = prevColor;
			
			GUI.Label(new Rect(m_catNameX, m_catNameY, m_catNameWidth, m_catNameHeight), m_categoryName, m_statusStyle);
			GUI.Label(new Rect(m_pageCountX, m_catNameY, 40, m_catNameHeight), m_currentPage + " / " + m_totalPages, m_statusStyle);
		});
		
		GUIUtils.DrawEnabled(m_controller.HasPrevPage, delegate
		{
			if (Common3DS.BackButton())
			{
				m_controller.PrevPage();
			}
		});
		
		GUIUtils.DrawEnabled(m_controller.HasNextPage, delegate
		{
			if (Common3DS.NextButton())
			{
				m_controller.NextPage();
			}
		});
		
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
}

