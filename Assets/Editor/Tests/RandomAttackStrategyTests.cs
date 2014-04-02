using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class RandomAttackStrategyTests
	{
		[Test]
		public void CorrectAbilityCalled()
		{
			Character c = new Character(70);
			Ability a1 = new Ability("Test Ability", "Normal", 20, 20, c);
			Ability a2 = new Ability("Unselected", "Normal", 20, 20, c);
			Ability a3 = new Ability("Unselected", "Normal", 20, 20, c);
			Ability a4 = new Ability("Unselected", "Normal", 20, 20, c);
			
			c.setAbility(0, a1);
			c.setAbility(1, a2);
			c.setAbility(2, a3);
			c.setAbility(3, a4);
			
			Random presetGenerator = new Random(0);
			
			IAttackStrategy strategy = new RandomAttackStrategy(presetGenerator);
			
			strategy.Execute(c, new Character[] {});
			
			uint[] uses = {a1.CurrentUses, a2.CurrentUses, a3.CurrentUses, a4.CurrentUses};
				
			uint[] expected = { 20, 20, 20, 19 };
			
			string message = "";
			foreach (uint i in uses)
			{
				message += i + " ";
			}
			
			//Assert.Fail("results are: " + message);
			Assert.AreEqual(expected, uses);
		}
	}
}

