using UnityEngine;

public class CounterReplaceDisplay : MonoBehaviour
{
	Texture2D m_backgroundTexture;
	Texture2D m_solidTexture;
	
	IScreenController m_controller;
	
	void Awake()
	{
		m_backgroundTexture = Resources.Load<Texture2D>("Textures/ButtonLayout");
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
	}
	
	public int buttonWidth = 340;
	public int buttonHeight = 40;
	public int buttonVPadding = 20;
	
	public int m_boxY = 65;
	public int m_boxHeight = 150; 
	
	void OnGUI()
	{
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), m_backgroundTexture);
			
			DrawButtonSection(bounds);
		});
	}
	
	public void SetController(IScreenController controller)
	{
		m_controller = controller;
	}
	
	
	void DrawButtonSection(Rect bounds)
	{
		Rect innerBounds = new Rect(0.0f, m_boxY, bounds.width, m_boxHeight);
		GUIUtils.DrawGroup(innerBounds, delegate()
		{
			Color prevColor = GUI.color;
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, innerBounds.width, innerBounds.height), m_solidTexture);
			GUI.color = prevColor;
			
			if (SwitchPokemonButton.Display(new Rect((innerBounds.width - buttonWidth) / 2.0f, buttonVPadding, buttonWidth, buttonHeight), 
				"SWITCH POKEMON", Color.green))
			{
				m_controller.ProcessInput((int)CounterReplaceController.Options.Replace);
			}
			
			if (SwitchPokemonButton.Display(new Rect((innerBounds.width - buttonWidth) / 2.0f, innerBounds.height - buttonHeight - buttonVPadding, buttonWidth, buttonHeight), 
				"CONTINUE BATTLING", Color.cyan))
			{
				m_controller.ProcessInput((int)CounterReplaceController.Options.Continue);
			}
		});
	}
}

