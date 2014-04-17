using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class SequentialPokemonStrategyTests
	{
	
		private Character createTestPokemon()
		{
			return new Character("Test Pokemon", Character.Sex.Male, 70, BattleType.Normal, null);
		}
		
		private Player m_player;
		private int m_totalPokemon;
		private SequentialPokemonStrategy m_strategy;
		
		[SetUp]
		public void Setup()
		{
			m_player = new Player("Test Player", null);
			m_totalPokemon = 5;
			m_strategy = new SequentialPokemonStrategy();
			
			for (int i = 0; i < m_totalPokemon; ++i)
			{
				m_player.AddPokemon(createTestPokemon());
			}
		}
	
		[Test]
		public void GetNextInOrder()
		{
			int nextIndex = m_strategy.getNextPokemon(m_player, null);
			
			Assert.AreEqual(1, nextIndex);
		}
		
		[Test]
		public void GetNextWrapAround()
		{
			m_player.setActivePokemon(m_totalPokemon - 1);
			
			int nextIndex = m_strategy.getNextPokemon(m_player, null);
			
			Assert.AreEqual(0, nextIndex);
		}
		
		[Test]
		public void SkipsDead()
		{
			// kill next pokemon
			Character pokemon = m_player.Pokemon[1];
			pokemon.TakeDamage(pokemon.CurrentHP);
			
			int nextIndex = m_strategy.getNextPokemon(m_player, null);
			
			Assert.AreEqual(2, nextIndex);
		}
		
		[Test]
		public void NoValidPokemon()
		{
			// kill all pokemon
			for (int i=0; i < m_totalPokemon; ++i)
			{
				Character pokemon = m_player.Pokemon[i];
				pokemon.TakeDamage(pokemon.CurrentHP);
			}
			
			int nextIndex = m_strategy.getNextPokemon(m_player, null);
			
			Assert.AreEqual(-1, nextIndex);
		}
		
	}
}

