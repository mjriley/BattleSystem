//// ------------------------------------------------------------------------------
////  <autogenerated>
////      This code was generated by a tool.
////      Mono Runtime Version: 4.0.30319.1
//// 
////      Changes to this file may cause incorrect behavior and will be lost if 
////      the code is regenerated.
////  </autogenerated>
//// ------------------------------------------------------------------------------
//using System;
//using System.Collections.Generic;
//using NUnit.Framework;
//
//namespace Tests
//{
//	[TestFixture]
//	public class CharacterTests
//	{
//		Character m_actor;
//		
//		[SetUp]
//		public void Init()
//		{
//			m_actor = new Character(70);
//		}
//			
//		[Test]
//		public void BasicConstructor()
//		{
//			Assert.AreEqual(70, m_actor.CurrentHP);
//		}
//		
//		[Test]
//		public void TakesDamageCorrectly()
//		{
//			m_actor.TakeDamage(50);
//			
//			Assert.AreEqual(20, m_actor.CurrentHP);
//		}
//		
//		[Test]
//		public void HealsCorrectly()
//		{
//			m_actor.TakeDamage(50);
//			m_actor.TakeDamage(-10);
//			
//			Assert.AreEqual(30, m_actor.CurrentHP);
//		}
//		
//		[Test]
//		public void CannotExceedMaxHP()
//		{
//			m_actor.TakeDamage(-10);
//			
//			Assert.AreEqual(70, m_actor.CurrentHP);
//		}
//		
//		[Test]
//		public void ZeroHP()
//		{
//			m_actor.TakeDamage(80);
//			
//			Assert.AreEqual (0, m_actor.CurrentHP);
//		}
//		
//		[Test]
//		public void IsDead()
//		{
//			m_actor.TakeDamage(70);
//			
//			Assert.IsTrue(m_actor.isDead());
//		}
//		
//		[Test]
//		public void TurnTargetTakesDamage()
//		{
//			RandomAttackStrategy strategy = new RandomAttackStrategy(new Random(0));
//			m_actor = new Character(70, strategy);
//			
//			Ability ability = new Ability("Basic Ability", "Normal", 20, 20);
//			m_actor.addAbility(ability);
//			
//			Character[] allies = {};
//			
//			Character enemy = new Character(90);
//			Character[] enemies = { enemy };
//			
//			m_actor.handleTurn(allies, enemies);
//			
//			Assert.AreEqual(70, enemy.CurrentHP);
//		}
//		
//		[Test]
//		public void TurnAbilityIsUsed()
//		{
//			RandomAttackStrategy strategy = new RandomAttackStrategy(new Random(0));
//			m_actor = new Character(70, strategy);
//			
//			Ability ability = new Ability("Basic Ability", "Normal", 20, 20);
//			m_actor.addAbility(ability);
//			
//			Character[] allies = {};
//			
//			Character enemy = new Character(90);
//			Character[] enemies = { enemy };
//			
//			m_actor.handleTurn(allies, enemies);
//			System.Console.WriteLine("Hello World 2");
//			
//			Assert.AreEqual(19, ability.CurrentUses);
//		}
//	}
//}
//
