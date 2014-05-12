using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{ 

	public Texture2D backTile;
	public Texture2D solidBackground;
	public Texture2D gradient;
	public Texture2D windTexture;
	public float offsetX = 1.0f;
	public float offsetY = 0.0f;
	public float width = 300;
	public float height = 300;
	public float angle = 45.0f;
	
	private float windOffset = 0.0f;
	public float windSpeed = 0.1f;
	
	private Texture2D trainer;
	
	Rect m_topScreen = new Rect(0, 0, 400, 240);
	Rect m_bottomScreen = new Rect(0, 240, 400, 240);

	void Start()
	{
		trainer = Resources.Load<Texture2D>("Textures/Trainers/VSFurisode_Girl_2");
	}
	
	void Update()
	{
		offsetX -= 0.5f * Time.deltaTime;
		if (offsetX < 0.0f)
		{
			offsetX = 1.0f;
		}
		
		offsetY += 0.5f * Time.deltaTime;
		if (offsetY > 1.0f)
		{
			offsetY = 0.0f;
		}
		
		windOffset += windSpeed * Time.deltaTime;
		if (windOffset > 1.0f)
		{
			windOffset %= 1.0f;
		}
	}
	
	void DrawTopScreen()
	{
		GUIUtils.DrawGroup(m_topScreen, delegate(Rect bounds)
		{
			// Plain backdrop
			Color prevColor = GUI.color;
			GUI.color = new Color(9.0f / 255.0f, 28.0f / 255.0f, 58.0f / 255.0f);
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), solidBackground);
			GUI.color = prevColor;
		});
		
		// Calculate boundary information for a Rect large enough to contain a rotated texture
		//  that will still fill up the viewable screen
		// NOTE: This can't be done within a draw group because unity 2d clips prior to rotations
		// meaning that the rotated texture will not fill the desired area
		{
			Rect bounds = m_topScreen;
				
			float angle_rads = angle * Mathf.PI / 180.0f;
			float bound2width = bounds.width * Mathf.Cos(angle_rads) + bounds.height * Mathf.Sin(angle_rads);
			float bound2height = bounds.width * Mathf.Sin(angle_rads) + bounds.height * Mathf.Cos(angle_rads);
			
			Rect bounds2 = new Rect(bounds.width / 2.0f - bound2width / 2.0f, bounds.height / 2.0f - bound2height / 2.0f, bound2width, bound2height);
			Rect scale = new Rect(offsetX, offsetY,  bound2width / backTile.width, bound2height / backTile.height);
			
			// Rotated Texture
			Matrix4x4 prevMatrix = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, new Vector2(bounds.x + bounds.width / 2.0f, bounds.y + bounds.height / 2.0f));
			Color prevColor = GUI.color;
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.05f);
			GUI.DrawTextureWithTexCoords(bounds2, backTile, scale);
			GUI.color = prevColor;
			GUI.matrix = prevMatrix;
		}
			
		GUIUtils.DrawGroup(m_topScreen, delegate(Rect bounds) {
			GUI.DrawTexture(new Rect(0, bounds.height / 2.0f - gradientHeight / 2.0f, bounds.width, gradientHeight), gradient);
			
			// Wind
			Color prevColor = GUI.color;
			GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
			GUI.DrawTextureWithTexCoords(new Rect(0, 0, bounds.width, bounds.height), windTexture, new Rect(windOffset, 0.0f, 0.5f, 1.0f));
			GUI.color = prevColor;
			
			// Trainer
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), trainer);
		});
	}
	
	void DrawBottomScreen()
	{
		GUIUtils.DrawGroup(m_bottomScreen, delegate(Rect bounds)
		{
			Color prevColor = GUI.color;
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0.0f, 0.0f, bounds.width, bounds.height), solidBackground);
			GUI.color = prevColor;
		});
	}
	
	float gradientHeight = 150;
	void OnGUI()
	{
		GUI.depth = 5;
		
		// NOTE: Make sure the top is painted prior to the bottom;
		// due to unity's mishandling of rotations, bottom needs
		// to paint over some rotated artifacts
		DrawTopScreen();
		DrawBottomScreen();
	}
}
