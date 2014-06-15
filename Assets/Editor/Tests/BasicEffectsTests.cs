using NUnit.Framework;
using Abilities;
using Abilities.Effects;
using Pokemon;
using PokeCore;

namespace Tests
{
	[TestFixture]
	public class BasicEffectsTests
	{
		Character m_actor;
		Character m_enemy;
		Player m_enemyPlayer;
		ActionStatus m_status;
		
		[SetUp]
		public void Init()
		{
			m_actor = PokemonFactory.CreatePokemon(Pokemon.Species.Pikachu, 50);
			m_enemy = PokemonFactory.CreatePokemon(Pokemon.Species.Pikachu, 50);
			m_enemyPlayer = new Player("", null, null, null);
			m_enemyPlayer.AddPokemon(m_enemy);
			
			m_status = new ActionStatus();
		}
		
		[Test]
		public void TargetEffectTest()
		{
			EffectAbility.BasicEffect effect = BasicEffects.StatEffect(Target.Enemy, Stat.Attack, 1);
			effect(null, m_actor, m_enemyPlayer, ref m_status);
			
			// Pikachu's base attack is 75 @ 50, 112 with a stage boost
			Assert.AreEqual(112, m_enemy.Atk);
		}
		
		[Test]
		public void SelfEffectTest()
		{
			EffectAbility.BasicEffect effect = BasicEffects.StatEffect(Target.Self, Stat.Attack, 1);
			effect(null, m_actor, m_enemyPlayer, ref m_status);
			
			Assert.AreEqual(112, m_actor.Atk);
		}
	}
}