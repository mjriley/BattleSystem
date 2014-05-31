using NUnit.Framework;
using Abilities;

namespace Tests
{
	[TestFixture]
	public class FixedDamageTests
	{
		DamageAbility m_ability;
		Character m_attacker;
		const uint POWER = 40;
		
		Character CreateTypedPokemon(BattleType battleType)
		{
			PokemonDefinition def = new PokemonDefinition(Pokemon.Species.Pikachu, battleType, 50, 50, 50, 50, 50, 50, null, 1.0f);
			return new Character("Defender", def, Pokemon.Gender.Male, 50, null);
		}
		
		[SetUp]
		public void Init()
		{
			m_ability = new DamageAbility("Test Ability", AbilityType.Special, BattleType.Dragon, 20, POWER, 100);
			m_attacker = PokemonFactory.CreatePokemon(Pokemon.Species.Pikachu, 50, "Attacker");
		}
		
		[Test]
		public void FixedDamageRegularEffectiveness()
		{
			// Dragon vs Normal is 1.0 effectiveness
			Character defender = CreateTypedPokemon(BattleType.Normal);
			
			DamageResult result = DamageCalculations.FixedDamageFormula(m_attacker, defender, m_ability, null);
			
			Assert.AreEqual(POWER, result.amount);
		}
		
		[Test]
		public void HigherEffectivenessIsNotUsed()
		{
			// Dragon vs Dragon is 2x effective
			Character defender = CreateTypedPokemon(BattleType.Dragon);
			
			DamageResult result = DamageCalculations.FixedDamageFormula(m_attacker, defender, m_ability, null);
			
			Assert.AreEqual(POWER, result.amount);
		}
		
		[Test]
		public void ImmunityRespected()
		{
			// Dragon vs Fairy, Fairy is immune
			Character defender = CreateTypedPokemon(BattleType.Fairy);
			
			DamageResult result = DamageCalculations.FixedDamageFormula(m_attacker, defender, m_ability, null);
			
			System.Console.WriteLine("Types are: " + defender.Types.Count);
			System.Console.WriteLine("Multiplier is: " + DamageCalculations.getDamageMultiplier(BattleType.Dragon, defender.Types));
			System.Console.WriteLine("Effectiveness is: " + result.effectiveness);
			Assert.AreEqual(0, result.amount);
		}
	}
}