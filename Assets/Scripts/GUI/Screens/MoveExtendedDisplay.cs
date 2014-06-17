using PokemonGUI;
using UnityEngine;

public class MoveExtendedDisplay : MoveDisplay, IGameScreen
{
	PokemonSummaryController m_controller;
	
	public GUIStyle m_summaryStyle;
	
	
	protected new void Awake()
	{
		base.Awake();
		
	}
	
	public void SetController(PokemonSummaryController controller)
	{
		base.SetController(controller);
		
		m_controller = controller;
	}
	
	public new void Invalidate()
	{
		base.Invalidate();
	}
	
	protected new void OnGUI()
	{
		base.OnGUI();
		
		DetailElements.Display(ScreenCoords.BottomScreen, m_pokemon);
		
		GUIUtils.DrawGroup(ScreenCoords.BottomScreen, delegate(Rect bounds)
		{
			if (GUI.Button(new Rect(155, 199, 140, 26), "SUMMARY", m_summaryStyle))
			{
				m_controller.SwitchScreens(PokemonSummaryController.ActiveScreen.Summary);
			}
		});
				
		GUIUtils.DrawEnabled((m_controller == null || m_controller.HasPreviousPage), delegate
		{
			if (Common3DS.BackButton())
			{
				m_controller.PreviousPage();
			}
		});
		
		GUIUtils.DrawEnabled(m_controller == null || m_controller.HasNextPage, delegate
		{
			if (Common3DS.NextButton())
			{
				m_controller.NextPage();
			}
		});
		
	}
	
}

