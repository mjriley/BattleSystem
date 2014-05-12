using UnityEngine;

public class GUIUtils
{
	public delegate void DrawFunc(Rect bounds);
	
	// wrapper around begin group to make the code easier to read/maintain
	//  and less error-prone. Essentially RIAA for groups
	static public void DrawGroup(Rect bounds, DrawFunc func)
	{
		GUI.BeginGroup(bounds);
		func(bounds);
		GUI.EndGroup();
	}
}
