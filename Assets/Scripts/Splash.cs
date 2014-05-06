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
	
	void OnGUI()
	{
		GUI.depth = 5;
		
		Color prevColor = GUI.color;
		GUI.color = new Color(9.0f / 255.0f, 28.0f / 255.0f, 58.0f / 255.0f);
		GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), solidBackground);
		GUI.color = prevColor;
		
		
	
		//float bound1width = 250.0f;
		float bound1width = Screen.width;
		float bound1height = Screen.height;
		//Rect bounds1 = new Rect(Screen.width / 2.0f - bound1width / 2.0f, Screen.height / 2.0f - bound1height / 2.0f, bound1width, bound1height);
		float angle_rads = angle * Mathf.PI / 180.0f;
		
		float bound2width = bound1width * Mathf.Cos(angle_rads) + bound1height * Mathf.Sin(angle_rads);
		float bound2height = bound1width * Mathf.Sin(angle_rads) + bound1height * Mathf.Cos(angle_rads);
		
		Rect bounds2 = new Rect(Screen.width / 2.0f - bound2width / 2.0f, Screen.height / 2.0f - bound2height / 2.0f, bound2width, bound2height);
		Rect scale = new Rect(offsetX, offsetY,  bound2width / backTile.width, bound2height / backTile.height);
		
		Matrix4x4 prevMatrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));
		prevColor = GUI.color;
		GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.05f);
		GUI.DrawTextureWithTexCoords(bounds2, backTile, scale);
		GUI.color = prevColor;
		GUI.matrix = prevMatrix;
		
		
		GUI.DrawTexture(new Rect(0, Screen.height / 2.0f - gradient.height / 2.0f, Screen.width, gradient.height), gradient);
		
		
		// Wind
		prevColor = GUI.color;
		GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, Screen.width, Screen.height), windTexture, new Rect(windOffset, 0.0f, 0.5f, 1.0f));
		GUI.color = prevColor;
		
		// Trainer
		GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), trainer);
	}
}
