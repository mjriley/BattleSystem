using UnityEngine;
using PokeCore;

public class ItemCategoryDisplay : MonoBehaviour
{
	public int m_buttonWidth = 150;
	public int m_buttonHeight = 60;
	public int m_buttonPaddingX = 20;
	public int m_buttonSpacingY = 10;
	public GUIStyle m_buttonStyle;
	
	IScreenController m_controller = null;
	
	Texture2D m_backgroundTexture;
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
	}
	
	public void SetController(IScreenController controller)
	{
		m_controller = controller;
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_backgroundTexture);
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			if (GUI.Button(new Rect(m_buttonPaddingX, m_buttonSpacingY, m_buttonWidth, m_buttonHeight), L18N.GetItemCategory(ItemCategory.Resource).ToUpper(), m_buttonStyle))
			{
				m_controller.ProcessInput((int)ItemCategory.Resource);
			}
			
			if (GUI.Button(new Rect(bounds.width - m_buttonPaddingX - m_buttonWidth, m_buttonSpacingY, m_buttonWidth, m_buttonHeight), 
				L18N.GetItemCategory(ItemCategory.Balls).ToUpper(), m_buttonStyle))
			{
				m_controller.ProcessInput((int)ItemCategory.Balls);
			}
			
			if (GUI.Button(new Rect(m_buttonPaddingX, m_buttonHeight + m_buttonSpacingY * 2, 
				m_buttonWidth, m_buttonHeight), L18N.GetItemCategory(ItemCategory.Status).ToUpper(), m_buttonStyle))
			{
				m_controller.ProcessInput((int)ItemCategory.Status);
			}
			
			if (GUI.Button(new Rect(bounds.width - m_buttonPaddingX - m_buttonWidth, m_buttonHeight + m_buttonSpacingY * 2, 
				m_buttonWidth, m_buttonHeight), L18N.GetItemCategory(ItemCategory.Battle).ToUpper(), m_buttonStyle))
			{
				m_controller.ProcessInput((int)ItemCategory.Battle);
			}
		});
		
		if (BackButton.Display(ScreenCoords.BottomScreen))
		{
			m_controller.Unload();
		}
	}
	
	
}
