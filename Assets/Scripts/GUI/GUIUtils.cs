using UnityEngine;

public class GUIUtils
{
	public delegate void DrawFunc(Rect bounds);
	public delegate void AnonDrawFunc();
	public delegate void BlockFunc();
	
	private static Texture2D m_solidTexture;
	private static bool m_isInit = false;
	
	// wrapper around begin group to make the code easier to read/maintain
	//  and less error-prone. Essentially RIAA for groups
	static public void DrawGroup(Rect bounds, DrawFunc func)
	{
		DrawGroup(bounds, delegate() { func(bounds); });
	}
	
	static public void DrawGroup(Rect bounds, AnonDrawFunc func)
	{
		GUI.BeginGroup(bounds);
		func();
		GUI.EndGroup();
	}
	
	static public void DrawEnabled(bool enabled, BlockFunc func)
	{
		bool prevEnabled = GUI.enabled;
		
		if (!enabled)
		{
			GUI.enabled = false;
		}
		
		func();
		
		GUI.enabled = prevEnabled;
	}
	
	static void Init()
	{
		if (!m_isInit)
		{
			m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
			m_isInit = true;
		}
	}
	
	static public void DrawSeparatorBar()
	{
		Init();
		Rect separatorBar = new Rect(0.0f, 240, 400, 20);
		Color prevColor = GUI.color;
		GUI.color = Color.black;
		GUI.DrawTexture(separatorBar, m_solidTexture);
		GUI.color = prevColor;
	}
	
	static public void DrawBottomScreenBackground(Rect bounds, int borderWidth=5)
	{
		Init();
		
		DrawGroup(bounds, delegate(Rect inner_bounds)
		{
			Color prevColor = GUI.color;
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0.0f, 0.0f, inner_bounds.width, inner_bounds.height), m_solidTexture);
			
			GUI.color = Color.gray;
			GUI.DrawTexture(new Rect(borderWidth, borderWidth, inner_bounds.width - borderWidth * 2, inner_bounds.height - borderWidth * 2), m_solidTexture);
			
			GUI.color = prevColor;
		});
	}
}
