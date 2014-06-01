using System.Collections.Generic;
using System;

namespace PokeCore {

public class ActionStatus
{
	public bool turnComplete;
	public bool isComplete;
	public List<EventArgs> events = new List<EventArgs>();
}

}