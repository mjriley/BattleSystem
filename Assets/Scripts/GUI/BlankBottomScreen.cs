using UnityEngine;

namespace PokemonGUI {

public class BlankBottomScreen : MonoBehaviour
{
	Texture2D m_solidTexture;
	
	void Awake()
	{
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
	}
	
	void OnGUI()
	{
		GUI.depth = 0;
		Color prevColor = GUI.color;
		GUI.color = Color.black;
		GUI.DrawTexture(ScreenCoords.BottomScreen, m_solidTexture);
		GUI.color = prevColor;
	}
}

}

