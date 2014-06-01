
using System;
using System.Collections.Generic;
using NUnit.Framework;

using Pokemon;
using PokeCore;

namespace Tests
{
	[TestFixture]
	public class CharacterTests
	{
		Character m_actor;
		
		[SetUp]
		public void Init()
		{
			//m_actor = new Character("Test Pokemon", Pokemon.Species.Pikachu, Pokemon.Gender.Male, 70, 1, BattleType.Normal, null);
			m_actor = PokemonFactory.CreatePokemon(Species.Pikachu, 50, "");
		}
			
		[Test]
		public void BaseHealth()
		{
			// Pikachu's Base HP at Level 50, Perfect IV is 110
			Assert.AreEqual(110, m_actor.CurrentHP);
		}
		
		[Test]
		public void TakesDamageCorrectly()
		{
			m_actor.TakeDamage(50);
			
			Assert.AreEqual(60, m_actor.CurrentHP);
		}
		
		[Test]
		public void HealsCorrectly()
		{
			m_actor.TakeDamage(50);
			m_actor.TakeDamage(-10);
			
			Assert.AreEqual(70, m_actor.CurrentHP);
		}
		
		[Test]
		public void CannotExceedMaxHP()
		{
			m_actor.TakeDamage(-10);
			
			Assert.AreEqual(110, m_actor.CurrentHP);
		}
		
		[Test]
		public void ZeroHP()
		{
			m_actor.TakeDamage(110);
			
			Assert.AreEqual(0, m_actor.CurrentHP);
		}
		
		[Test]
		public void IsDead()
		{
			m_actor.TakeDamage(110);
			
			Assert.IsTrue(m_actor.isDead());
		}
		
		[Test]
		public void BaseAttack()
		{
			// Base Attack for Pikachu is 55
			// At Level 50, this should be 75 with max IV and no EV
			Assert.AreEqual(75, m_actor.Atk);
		}
		
		[Test]
		public void PositiveStageIncreasesAttack()
		{
			m_actor.ModifyStage(Stat.Attack, 1);
			Assert.AreEqual(112, m_actor.Atk);
		}
		
		[Test]
		public void NegativeStageDecreasesAttack()
		{
			m_actor.ModifyStage(Stat.Attack, -1);
			Assert.AreEqual(50, m_actor.Atk);
		}
		
		[Test]
		public void MaximumBoostStage()
		{
			int max_stage = 6;
			m_actor.ModifyStage(Stat.Attack, max_stage);
			Assert.AreEqual(300, m_actor.Atk);
		}
		
		[Test]
		public void MaximumPenaltyStage()
		{
			int max_stage = -6;
			m_actor.ModifyStage(Stat.Attack, max_stage);
			Assert.AreEqual(18, m_actor.Atk);
		}
		
		[Test]
		public void CannotIncreaseBeyondMaximumStage()
		{
			int levels = m_actor.ModifyStage(Stat.Attack, 7);
			Assert.AreEqual(6, levels);
			Assert.AreEqual(300, m_actor.Atk);
		}
		
		[Test]
		public void CannotDecreaseBeyondMinimumStage()
		{
			int levels = m_actor.ModifyStage(Stat.Attack, -7);
			Assert.AreEqual(-6, levels);
			Assert.AreEqual(18, m_actor.Atk);
		}
		
		[Test]
		public void BaseAccuracy()
		{
			Assert.AreEqual(100, m_actor.Accuracy);
		}
		
		[Test]
		public void BaseEvasion()
		{
			Assert.AreEqual(100, m_actor.Evasion);
		}
		
		[Test]
		public void PositiveStageIncreasesAccuracy()
		{
			m_actor.ModifyStage(Stat.Accuracy, 1);
			Assert.AreEqual(133, m_actor.Accuracy);
		}
		
		[Test]
		public void PositiveStageIncreaseEvasion()
		{
			m_actor.ModifyStage(Stat.Evasion, 1);
			Assert.AreEqual(133, m_actor.Evasion);
		}
		
		[Test]
		public void BeneficialNature()
		{
			PokemonDefinition def = PokemonDefinition.GetEntry(Pokemon.Species.Pikachu);
			// Lonely increases attack and decreases Defense
			Nature nature = NatureFactory.GetNature(Nature.Type.Lonely);
			Character pokemon = new Character("", def, Pokemon.Gender.Male, 50, null, nature: nature);
			
			Assert.AreEqual(83, pokemon.Atk);
		}
		
		[Test]
		public void DetrimentalNature()
		{
			PokemonDefinition def = PokemonDefinition.GetEntry(Pokemon.Species.Pikachu);
			// Bold increases defense and decreases Attack
			Nature nature = NatureFactory.GetNature(Nature.Type.Bold);
			Character pokemon = new Character("", def, Pokemon.Gender.Male, 50, null, nature: nature);
			
			Assert.AreEqual(67, pokemon.Atk);
		}
		
		[Test]
		public void HandleIVs()
		{
			PokemonDefinition def = PokemonDefinition.GetEntry(Pokemon.Species.Pikachu);
			Dictionary<Stat, int> iv = new Dictionary<Stat, int> { {Stat.Attack, 10} };
			
			Character pokemon = new Character("", def, Pokemon.Gender.Male, 50, null, ivs: iv);
			
			Assert.AreEqual(65, pokemon.Atk);
		}
		
		[Test]
		public void HandleEVs()
		{
			PokemonDefinition def = PokemonDefinition.GetEntry(Pokemon.Species.Pikachu);
			Dictionary<Stat, int> ev = new Dictionary<Stat, int> { {Stat.Attack, 200} };
			
			Character pokemon = new Character("", def, Pokemon.Gender.Male, 50, null, evs: ev);
			
			Assert.AreEqual(100, pokemon.Atk);
		}
	}
}

