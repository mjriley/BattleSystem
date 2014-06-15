using System;
using NUnit.Framework;
using PokeCore;
using System.Collections.Generic;

namespace Tests
{
	class CharacterStub : ICharacter
	{
		public int Spd { get; set; }
		public string Name { get { return ""; } }
		public Player Owner { get { return null; } }
		public CharacterStub(int speed)
		{
			Spd = speed;
		}
	}
	
	class ActionStub : ITurnAction
	{
		public ICharacter Subject { get; set; }
		public int Priority { get; set; }
		public ActionStatus Execute() { return null; }
		public bool Verify(out List<string> messages) { messages = new List<string>(); return true; }
		
		public ActionStub(int priority, int speed)
		{
			Subject = new CharacterStub(speed);
			Priority = priority;
		}
	}
	
	class FixedRandom : IRandom
	{
		int m_value;
		public FixedRandom(int value) { m_value = value; }
		public int Next() { return m_value; }
		public int Next(int x) { return m_value; }
		public int Next(int x, int y) { return m_value; }
	}
	
	[TestFixture]
	public class ActionSortTests
	{
		[Test]
		public void SpeedComparison()
		{
			ActionStub action1 = new ActionStub(0, speed: 70);
			ActionStub action2 = new ActionStub(0, speed: 50);
			
			Assert.AreEqual(1, ActionSort.ByPriority(action1, action2));
		}
		
		[Test]
		public void PriorityTrumpsSpeed()
		{
			ActionStub action1 = new ActionStub(priority: 1, speed: 50);
			ActionStub action2 = new ActionStub(priority: 0, speed: 70);
			
			Assert.AreEqual(1, ActionSort.ByPriority(action1, action2));
		}
		
		[Test]
		public void Equality()
		{
			ActionStub action1 = new ActionStub(priority: 0, speed: 55);
			ActionStub action2 = new ActionStub(priority: 0, speed: 55);
			FixedRandom random = new FixedRandom(0);
			
			Assert.AreEqual(1, ActionSort.ByPriority(action1, action2, random));
		}
		
	}
}

