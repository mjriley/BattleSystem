using UnityEngine;
using System.Collections;

public class WhiteOut : MonoBehaviour
{

	Texture2D m_flatTexture;
	
	public float Opacity;

	void Start()
	{
		m_flatTexture = Resources.Load<Texture2D>("Textures/white_tile");
		Opacity = 0.0f;
	}
	
	void OnGUI()
	{
		Color prevColor = GUI.color;
		GUI.color = new Color(1.0f, 1.0f, 1.0f, Opacity);
		GUI.DrawTexture(new Rect(0.0f, 0.0f, 400, 240), m_flatTexture);
		GUI.color = prevColor;
	}
}
