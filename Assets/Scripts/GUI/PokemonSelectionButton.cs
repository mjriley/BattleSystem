using UnityEngine;

public class PokemonSelectionButton
{
	static bool isInit = false;
	static GUIStyle style;
	
	static void Init()
	{
		if (!isInit)
		{
			// load textures
			isInit = true;
			
			style = new GUIStyle();
			style.border.top = style.border.bottom = style.border.left = style.border.right = 2;
			style.normal.background = Resources.Load<Texture2D>("Textures/glow");
		}
	}
	
	public static void Display(Rect parent, Pokemon.Species species, bool selected)
	{
		Init();
		
		Rect innerBounds = new Rect(2.0f, 2.0f, parent.width - 4.0f, parent.height - 4.0f);
		GUI.BeginGroup(parent);
			if (selected)
			{
				GUI.Box(new Rect(0.0f, 0.0f, parent.width, parent.height), "", style);
			}
			GUI.Box(innerBounds, species.ToString());
		GUI.EndGroup();
	}
}

