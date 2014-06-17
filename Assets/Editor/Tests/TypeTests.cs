using System.Collections.Generic;
using NUnit.Framework;
using PokeCore.Moves;

namespace Tests
{
	[TestFixture]
	public class TypeTests
	{
		[Test]
		public void WaterVsFire()
		{
			List<BattleType> pokemonTypes = new List<BattleType>();
			pokemonTypes.Add(BattleType.Fire);
			
			double multiplier = DamageCalculations.getDamageMultiplier(BattleType.Water, pokemonTypes);
			
			Assert.AreEqual(2.0f, multiplier);
		}
		
		[Test]
		public void GhostVsGhostPsychic()
		{
			List<BattleType> pokemonTypes = new List<BattleType>();
			pokemonTypes.Add(BattleType.Ghost);
			pokemonTypes.Add(BattleType.Psychic);
			
			double multiplier = DamageCalculations.getDamageMultiplier(BattleType.Ghost, pokemonTypes);
			
			Assert.AreEqual(4.0f, multiplier);
		}
	}

}