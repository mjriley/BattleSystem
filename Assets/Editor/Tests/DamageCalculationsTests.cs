using NUnit.Framework;
using PokeCore.Moves;

namespace Tests
{
	[TestFixture]
	public class DamageCalculationsTests
	{
		double same_type_multiplier;
		double effectiveness_multiplier;
		double crit_multiplier;
		double variance_multiplier;
		uint level;
		int attack;
		int defense;
		uint power;
		
		private int Execute()
		{
			return DamageCalculations.CalculateBaseDamage(attack, defense, level, power, same_type_multiplier, effectiveness_multiplier, crit_multiplier, variance_multiplier);
		}
		
		[SetUp]
		public void Init()
		{
			same_type_multiplier = 1.0;
			effectiveness_multiplier = 1.0;
			crit_multiplier = 1.0;
			variance_multiplier = 1.0;
			level = 50;
			attack = 50;
			defense = 50;
			power = 100;
		}
		
		[Test]
		public void CalculateWithNoMultipliers()
		{
			int amount = Execute();
			Assert.AreEqual(46, amount);
		}
		
		[Test]
		public void UsesSameTypeMultiplier()
		{
			same_type_multiplier = 1.5;
			int amount = Execute();
			Assert.AreEqual(69, amount);
		}
		
		[Test]
		public void UsesEffectivenessMultiplier()
		{
			effectiveness_multiplier = 1.5;
			int amount = Execute();
			Assert.AreEqual(69, amount);
		}
		
		[Test]
		public void UsesCritMultiplier()
		{
			crit_multiplier = 1.5;
			int amount = Execute();
			Assert.AreEqual(69, amount);
		}
		
		[Test]
		public void UsesVarianceMultiplier()
		{
			variance_multiplier = 0.85;
			int amount = Execute();
			Assert.AreEqual(39, amount);
		}
		
		[Test]
		public void UsesLevel()
		{
			level = 100;
			int amount = Execute();
			Assert.AreEqual(86, amount);
		}
		
		[Test]
		public void UsesPower()
		{
			power = 50;
			int amount = Execute();
			Assert.AreEqual(24, amount);
		}
		
		[Test]
		public void UsesAttack()
		{
			attack = 10;
			int amount = Execute();
			Assert.AreEqual(10, amount);
		}
		
		[Test]
		public void UsesDefense()
		{
			defense = 10;
			int amount = Execute();
			Assert.AreEqual(222, amount);
		}
	}
}
