using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
	public GameObject m_roster;
	
	public void Awake()
	{
		GameObject existingObject = GameObject.Find("PlayerRoster");
		
		if (existingObject == null)
		{
			GameObject roster = (GameObject)Instantiate(m_roster);
			roster.name = "PlayerRoster";
		}
	}
}
