// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class AbilityTests
	{
		[Test]
		public void AbilityDeductsUsage()
		{
			Ability ability = new Ability("Test Ability", "Normal", 20, 20);
			
			ability.Use();
			
			Assert.AreEqual(19, ability.CurrentUses);
		}
		
		[Test]
		[ExpectedException(typeof(Exception))]
		public void ExceptionOnZeroUses()
		{
			Ability ability = new Ability("Test Ability", "Normal", 20, 0);
			
			ability.Use();
		}
	}
}

