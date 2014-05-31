using NUnit.Framework;
using Abilities;
using Abilities.Power;

namespace Tests
{

class DummyRepeatAbility : Abilities.IRepeatAbility
{
	public void Reset() { }
	public ActionStatus Execute(Character actor, Player targetPlayer, IAbilityImpl parentAbility) { return new ActionStatus(); }
	public int CurrentTurn { get; set; }
	public int MaxTurns { get { return 0; } }
	public DummyRepeatAbility(int currentTurn) { CurrentTurn = currentTurn; }
}

[TestFixture]
public class PowerPerTurnTests
{
	RepeatAbility m_repeatAbility;
	PowerPerTurn m_powerObject;
	
	[SetUp]
	public void Init()
	{
		m_powerObject = new PowerPerTurn(30);
	}
	
	[Test]
	public void BasePowerOnFirstTurn()
	{
		DummyRepeatAbility parent = new DummyRepeatAbility(0);
		uint power = m_powerObject.GetPower(null, null, null, parent);
		Assert.AreEqual(30, power);
	}
	
	[Test]
	public void PowerOnTurnFour()
	{
		DummyRepeatAbility parent = new DummyRepeatAbility(3);
		uint power = m_powerObject.GetPower(null, null, null, parent);
		Assert.AreEqual(240, power);
	}
}

}

