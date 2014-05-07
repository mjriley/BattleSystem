
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class CharacterTests
	{
		Character m_actor;
		
		[SetUp]
		public void Init()
		{
			m_actor = new Character("Test Pokemon", Pokemon.Species.Pikachu, Character.Sex.Male, 70, 1, BattleType.Normal, null);
		}
			
		[Test]
		public void BasicConstructor()
		{
			Assert.AreEqual(70, m_actor.CurrentHP);
		}
		
		[Test]
		public void TakesDamageCorrectly()
		{
			m_actor.TakeDamage(50);
			
			Assert.AreEqual(20, m_actor.CurrentHP);
		}
		
		[Test]
		public void HealsCorrectly()
		{
			m_actor.TakeDamage(50);
			m_actor.TakeDamage(-10);
			
			Assert.AreEqual(30, m_actor.CurrentHP);
		}
		
		[Test]
		public void CannotExceedMaxHP()
		{
			m_actor.TakeDamage(-10);
			
			Assert.AreEqual(70, m_actor.CurrentHP);
		}
		
		[Test]
		public void ZeroHP()
		{
			m_actor.TakeDamage(80);
			
			Assert.AreEqual (0, m_actor.CurrentHP);
		}
		
		[Test]
		public void IsDead()
		{
			m_actor.TakeDamage(70);
			
			Assert.IsTrue(m_actor.isDead());
		}
		
		[Test]
		public void BaseAttack()
		{
			Assert.AreEqual(50, m_actor.Atk);
		}
		
		[Test]
		public void PositiveStageIncreasesAttack()
		{
			m_actor.ModifyStage(Stat.Attack, 1);
			Assert.AreEqual(75, m_actor.Atk);
		}
		
		[Test]
		public void NegativeStageDecreasesAttack()
		{
			m_actor.ModifyStage(Stat.Attack, -1);
			Assert.AreEqual(33, m_actor.Atk);
		}
		
		[Test]
		public void MaximumBoostStage()
		{
			int max_stage = 6;
			m_actor.ModifyStage(Stat.Attack, max_stage);
			Assert.AreEqual(200, m_actor.Atk);
		}
		
		[Test]
		public void MaximumPenaltyStage()
		{
			int max_stage = -6;
			m_actor.ModifyStage(Stat.Attack, max_stage);
			Assert.AreEqual(12, m_actor.Atk);
		}
		
		[Test]
		public void CannotIncreaseBeyondMaximumStage()
		{
			int levels = m_actor.ModifyStage(Stat.Attack, 7);
			Assert.AreEqual(6, levels);
			Assert.AreEqual(200, m_actor.Atk);
		}
		
		[Test]
		public void CannotDecreaseBeyondMinimumStage()
		{
			int levels = m_actor.ModifyStage(Stat.Attack, -7);
			Assert.AreEqual(-6, levels);
			Assert.AreEqual(12, m_actor.Atk);
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
		
		
		
	}
}

