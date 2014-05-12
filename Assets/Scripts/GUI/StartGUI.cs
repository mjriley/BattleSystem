using System;
using UnityEngine;

public class StartGUI : MonoBehaviour
{
	RosterController m_controller;
	
	public void Start()
	{
		m_controller = new RosterController();
	}
	
	public void Update()
	{
		m_controller.GetActiveDisplay().Update();
	}
	
	public void OnGUI()
	{
		m_controller.GetActiveDisplay().Display();
	}
}
