using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class RandomAttackStrategyTests
	{
	
		Character m_actor;
		Ability m_ability;
		RandomAttackStrategy m_strategy;
		
		Character[] m_allies;
		Character[] m_enemies;
	
		[SetUp]
		public void Init()
		{
			m_actor = new Character(70);
			m_ability = new Ability("Ability 1", "Normal", 20, 20);
			
			Random presetGenerator = new Random(0);
			m_strategy = new RandomAttackStrategy(presetGenerator);
			
			m_actor.addAbility(m_ability);
			
			m_allies = new Character[] {};
			
			Character enemy = new Character(70);
			m_enemies = new Character[] { enemy };
		}
		
		[Test]
		public void CorrectAbilityCalled()
		{
			Ability a2 = new Ability("Ability 2", "Normal", 20, 20);
			Ability a3 = new Ability("Ability 3", "Normal", 20, 20);
			Ability a4 = new Ability("Ability 4", "Normal", 20, 20);
			
			m_actor.addAbility(a2);
			m_actor.addAbility(a3);
			m_actor.addAbility(a4);
			
			AbilityUse turnInfo = m_strategy.Execute(m_actor, m_allies, m_enemies);
			
			Assert.AreEqual(a4.Name, turnInfo.ability.Name);
		}
		
		[Test]
		public void FindCorrectTarget()
		{
			Character e1 = new Character(10);
			Character e2 = new Character(20);
			Character e3 = new Character(30);
			
			Character[] enemies = { e1, e2, e3 };
			
			AbilityUse turnInfo = m_strategy.Execute(m_actor, m_allies, enemies);
			
			Assert.AreEqual(e3, turnInfo.targets.First());
		}
		
		[Test]
		public void SkipInsufficientPPAbilities()
		{
			Ability a2 = new Ability("0 PP Ability", "Normal", 20, 0);
			
			m_actor.addAbility(a2);
			
			AbilityUse turnInfo = m_strategy.Execute(m_actor, m_allies, m_enemies);
			
			Assert.AreEqual(m_ability.Name, turnInfo.ability.Name);
		}
		
		[Test]
		public void SkipTurnOnNoPPLeft()
		{
			Ability a1 = new Ability("Ability 1", "Normal", 20, 0);
			Ability a2 = new Ability("Ability 2", "Normal", 20, 0);
			
			m_actor.replaceAbility(0, a1);
			m_actor.addAbility(a2);
			
			AbilityUse turnInfo = m_strategy.Execute(m_actor, m_allies, m_enemies);
			
			Assert.IsNull(turnInfo.ability);
		}
	}
}

