using UnityEngine;

public class StarTransition : MonoBehaviour
{
	Texture2D m_starTexture;
	Texture2D m_solidTexture;
	Texture2D m_gradientTexture;
	
	public int m_timePerFrame = 200;
	int m_currentTime = 0;
	const int totalUniqueFrames = 7;
	int m_totalFrames = 7;
	const int frameWidth = 48;
	int m_currentFrame = 0;
	
	public bool IsAnimating { get; protected set; }
	
	void Awake()
	{
		m_starTexture = Resources.Load<Texture2D>("Textures/stars");
		m_solidTexture = Resources.Load<Texture2D>("Textures/white_tile");
		m_gradientTexture = Resources.Load<Texture2D>("Textures/GradientBlend");
	}
	
	void Start()
	{
		Reset();
		IsAnimating = true;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Reset();
			IsAnimating = true;
		}
		
		if (IsAnimating)
		{
			if (m_currentFrame < m_totalFrames)
			{
				m_currentTime += (int)(Time.deltaTime * 1000);
				
				while (m_currentTime >= m_timePerFrame)
				{
					m_currentFrame += 1;
					m_currentTime -= m_timePerFrame;
					
					m_currentFrame = Mathf.Min(m_currentFrame + 1, m_totalFrames);
				}
			}
			else
			{
				IsAnimating = false;
			}
		}
	}
	
	void Reset()
	{
		m_currentFrame = 0;
		m_currentTime = 0;
		m_totalFrames = Mathf.CeilToInt(Screen.width / (float)frameWidth) + totalUniqueFrames;
	}
	
	void OnGUI()
	{
		Color prevColor = GUI.color;
		GUI.color = Color.black;
		GUI.DrawTextureWithTexCoords(new Rect(Screen.width - frameWidth * m_currentFrame, 0.0f, m_starTexture.width, m_starTexture.height * 5), m_starTexture, new Rect(0.0f, 0.0f, 1.0f, 5.0f));
		if (m_currentFrame > totalUniqueFrames)
		{
			int offsetFrame = m_currentFrame - totalUniqueFrames;
			int solidWidth = offsetFrame * frameWidth + frameWidth / 2;
			int gradientX = offsetFrame * frameWidth + frameWidth;
			
			GUI.DrawTexture(new Rect(Screen.width - solidWidth, 0.0f, solidWidth, 240), m_solidTexture);
			
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
			GUI.DrawTexture(new Rect(Screen.width - gradientX, 0.0f, frameWidth / 2, 240), m_gradientTexture);
		}
		GUI.color = prevColor;
	}
}
