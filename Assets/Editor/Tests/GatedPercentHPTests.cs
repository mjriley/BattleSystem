using Abilities.Power;
using NUnit.Framework;

using Pokemon;
using PokeCore;

namespace Tests
{

	[TestFixture]
	public class GatedPercentHPTests
	{
		Character m_attacker;
		Character m_defender;
		GatedPercentHP m_powerFormula;
		
		[SetUp]
		public void Init()
		{
			// A Level 50 Pikachu's Max HP is 110
			m_attacker = PokemonFactory.CreatePokemon(Species.Pikachu, 50, "Attacker");
			m_defender = PokemonFactory.CreatePokemon(Species.Pikachu, 50, "Defender");
			
			m_powerFormula = new GatedPercentHP();
		}
		
		[Test]
		public void Tier1Test()
		{
			// 110 HP = 100%
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(20, power);
		}
		
		[Test]
		public void Tier2Test()
		{
			// 77 HP = 70%
			m_attacker.TakeDamage(33);
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(40, power);
		}
		
		[Test]
		public void Tier3Test()
		{
			// 38 HP = <35%
			m_attacker.TakeDamage(72);
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(80, power);
		}
		
		[Test]
		public void Tier4Test()
		{
			// 22 HP = 20%
			m_attacker.TakeDamage(88);
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(100, power);
		}
		
		[Test]
		public void Tier5Test()
		{
			// 11 HP = 10%
			m_attacker.TakeDamage(99);
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(150, power);
		}
		
		[Test]
		public void Tier6Test()
		{
			// 5 HP = < 5%
			m_attacker.TakeDamage(105);
			uint power = m_powerFormula.GetPower(m_attacker, m_defender, null, null);
			Assert.AreEqual(200, power);
		}
	}
	
}

