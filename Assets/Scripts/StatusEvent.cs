using System.Collections.Generic;
using System;

public class StatusEvent : IBattleEvent
{
	public string Status { get; set; }
	
	public StatusEvent(string status)
	{
		Status = status;
	}
	
	public List<EventArgs> Execute()
	{
		List<EventArgs> events = new List<EventArgs>();
		events.Add(new StatusUpdateEventArgs(Status));
		
		return events;
	}
	
}

