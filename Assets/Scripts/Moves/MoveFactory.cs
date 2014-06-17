using System.Collections.Generic;
using PokeCore.Pokemon;

namespace PokeCore {
namespace Moves {
	using Power;
	using Effects;

public class MoveFactory
{
	static Dictionary<string, Move> m_moves;
	
	static bool m_isInit = false;
	
	public static Move GetMove(string name)
	{
		if (!m_isInit)
		{
			Init();
			m_isInit = true;
		}
		
		return m_moves[name];
	}
	
	private static void AddMove(AbstractMove move)
	{
		string moveKey = move.Name.ToUpper().Replace(' ', '_');
		move.Description = L18N.Get(moveKey);
		m_moves[move.Name] = new Move(move, move.MaxPP);
	}
	
	public static void Init()
	{
		m_moves = new Dictionary<string, Move>();
		
		AddMove(new DamageMove("Thunder Shock", MoveType.Special, BattleType.Electric, 30, 40, 100, 
			onHitHandler: OnHitEffects.ParalyzeEffect(10)));
		
		AddMove(new DamageMove("Bubble", MoveType.Special, BattleType.Water, 30, 40, 100, 
			onHitHandler: OnHitEffects.BasicEffectWrapper(BasicEffects.StatEffect(Target.Enemy, Stat.Speed, -1, 10))));
		AddMove(new DamageMove("Water Gun", MoveType.Special, BattleType.Water, 25, 40, 100));
		AddMove(new EffectMove("Withdraw", MoveType.Status, BattleType.Water, 40, BasicEffects.StatEffect(Target.Self, Stat.Defense, 1)));
		
		AddMove(new DamageMove("Ember", MoveType.Special, BattleType.Fire, 25, 40, 100, 
			onHitHandler: OnHitEffects.BurnEffect(10)));
		AddMove(new DamageMove("Fire Fang", MoveType.Physical, BattleType.Fire, 15, 65, 95, 
			onHitHandler: OnHitEffects.CompositeEffect(OnHitEffects.BurnEffect(10), OnHitEffects.FlinchEffect(10))));
		
		AddMove(new DamageMove("Vine Whip", MoveType.Physical, BattleType.Grass, 25, 45, 100));
		AddMove(new DamageMove("Razor Leaf", MoveType.Physical, BattleType.Grass, 25, 55, 95, highCritRate: true));
		AddMove(new EffectMove("Leech Seed", MoveType.Status, BattleType.Grass, 10, BasicEffects.SeedTarget, 90));
		
		AddMove(new DamageMove("Quick Attack", MoveType.Physical, BattleType.Normal, 30, 40, 100, priority: 1));
		AddMove(new DamageMove("Tackle", MoveType.Physical, BattleType.Normal, 35, 50, 100));
		AddMove(new DamageMove("Pound", MoveType.Physical, BattleType.Normal, 35, 40, 100));
		AddMove(new MultiHitMove("Fury Swipes", MoveType.Physical, BattleType.Normal, 15, 18, 80, 2, 5, distribution_gen: new FiveHitDistribution()));
		AddMove(new DamageMove("Scratch", MoveType.Physical, BattleType.Normal, 35, 40, 100));
		AddMove(new NoOpMove("Splash", MoveType.Physical, BattleType.Normal, 40, "But nothing happened!"));
		AddMove(new DamageMove("Flail", MoveType.Physical, BattleType.Normal, 15, new GatedPercentHP(), 100));
		AddMove(new DamageMove("Take Down", MoveType.Physical, BattleType.Normal, 20, 90, 85, 
			onHitHandler: OnHitEffects.RecoilEffect(25)));
		AddMove(new EffectMove("Growl", MoveType.Status, BattleType.Normal, 40, BasicEffects.StatEffect(Target.Enemy, Stat.Attack, -1)));
		AddMove(new EffectMove("Tail Whip", MoveType.Status, BattleType.Normal, 30, BasicEffects.StatEffect(Target.Enemy, Stat.Defense, -1)));
		AddMove(new EffectMove("Howl", MoveType.Status, BattleType.Normal, 40, BasicEffects.StatEffect(Target.Self, Stat.Attack, 1)));
		AddMove(new EffectMove("Agility", MoveType.Status, BattleType.Psychic, 30, BasicEffects.StatEffect(Target.Self, Stat.Speed, 2)));
		AddMove(new EffectMove("Leer", MoveType.Status, BattleType.Normal, 30, BasicEffects.StatEffect(Target.Enemy, Stat.Defense, -1)));
		AddMove(new DamageMove("Headbutt", MoveType.Physical, BattleType.Normal, 15, 70, 100, 
			onHitHandler: OnHitEffects.FlinchEffect(30)));
		AddMove(new EffectMove("Smokescreen", MoveType.Status, BattleType.Normal, 20, BasicEffects.StatEffect(Target.Enemy, Stat.Accuracy, -1)));
		// TODO: Look into how it always hits
		AddMove(new EffectMove("Play Nice", MoveType.Status, BattleType.Normal, 20, BasicEffects.StatEffect(Target.Enemy, Stat.Attack, -1)));
		
		AddMove(new RepeatMove(new DamageMove("Rollout", MoveType.Physical, BattleType.Rock, 20, new PowerPerTurn(30), 90), 5));
		
		AddMove(new DamageMove("Dragon Rage", MoveType.Special, BattleType.Dragon, 10, 40, 100, damageFormula: DamageCalculations.FixedDamageFormula));
		
		AddMove(new DamageMove("Bite", MoveType.Physical, BattleType.Dark, 25, 60, 100, 
			onHitHandler: OnHitEffects.FlinchEffect(30)));
		
		
//		// Fire
//		Moves["Blast Burn"] = // Multi-turn Move -- [new Move("Blast Burn", MoveType.Special, BattleType.Fire, 150, 90, 5), RestMove]; does not recharge if first attack misses
//		Moves["Blaze Kick"] = // Composite Move [DamageMove("Blaze Kick", MoveType.Physical, BattleType.Fire, 85, 90, 10, CritStage + 1), StatusMove(Burn, 10%);
//		Moves["Blue Flare"] = // Composite Move [DamageMove(MoveType.Special, BattleType.Fire, 130, 85, 5), StatusMove(Burn, 20%)]
//		Moves["Ember"] = // Composite Move [DamageMove(MoveType.Special, BattleType.Fire, 40, 100, 25), StatusMove(Burn, 10%)]
//		Moves["Eruption"] = // DamageMove(MoveType.Special, BattleType.Fire, 150, 100, 5, AffectedByHP)
//		Moves["Fiery Dance"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 80, 100, 100), StatMove(SpAtk + 1, 50%)]
//		Moves["Fire Blast"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 110, 85%, 5), StatusMove(Burn, 10%)]
//		Moves["Fire Fang"] = // CompositeMove [DamageMove(MoveType.Physical, BattleType.Fire, 65, 95%, 15), StatusMove(Burn, 10%), StatusMove(Flinch, 10%)]
//		Moves["Fire Plege"] = // DamageMove (MoveType.Special, BattleType.Fire, 80, 100, 15) // Special Stuff for Double/Triple battles
//		Moves["Fire Punch"] = // CompositeMove [DamageAbiilty(MoveType.Physical, BattleType.Fire, 75, 100, 15), StatusMove(Burn, 10%)]
//		Moves["Fire Spin"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 35, 85, 15), StatusMove(Trap, only lands if parent move lands?)]
//		Moves["Flame Burst"] = // DamageMove(MoveType.Special, BattleType.Fire, 70, 100, 15) // hits adjacent targets
//		Moves["Flame Charge"] = // CompositeMove [DamageMove(MoveType.Physical, BattleType.Fire, 50, 100, 20), StatMove(Spd + 1, 100%)]
//		Moves["Flame Wheel"] = // CompositeMove [StatusMove(Thaw, 100%), DamageMove(MoveType.Physical, BattleType.Fire, 60, 100, 25), StatusMove(Burn, 10%)]
//		Moves["Flamethrower"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 90, 100, 15), StatusMove(Burn, 10%)]
//		Moves["Flare Blitz"] = // CompositeMove [StatusMove(Thaw, 100%), DamageMove(MoveType.Physical, BattleType.Fire, 120, 100, 15, Recoil(33%)), StatusMove(Burn, 10%)]
//		Moves["Fusion Flare"] = // CompositeMove [StatusMove(Thaw, 100%), DamageMove(MoveType.Special, BattleType.Fire, 100, 100, 5)] // Double damage if preceded by Fusion Bolt or Bolt strike
//		Moves["Heat Crash"] = // DamageMove(MoveType.Physical, BattleType.Fire, 40, 100, 10, WeightCalculation)
//		Moves["Heat Wave"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 95, 90, 10), StatusMove(Burn, 10%)] // also hits adjacent pokemon
//		Moves["Incinerate"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 60, 100, 15), ItemClassDestroy(Berries, Gems)] // also hits adjacent pokemon
//		Moves["Inferno"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 100, 50, 5), StatusMove(Burn, 100%)]
//		Moves["Lava Plume"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 80, 100, 15), StatusMove(Burn, 30%)]
//		Moves["Magma Storm"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 100, 75, 5), StatusMove(Trap2-5)]
//		Moves["Mystical Fire"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 65, 100, 10), StatusMove(SpAtk-1, 100%)]
//		Moves["Overheat"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 130, 90, 5), StatMove(SpAtk - 2, 100%)]
//		Moves["Sacred Fire"] = // CompositeMove [StatusMove(Thaw, 100%), DamageMove(MoveType.Physical, BattleType.Fire, 100, 95, 5), StatusMove(Burn, 50%)]
//		Moves["Searing Shot"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Fire, 100, 100, 5), StatusMove(Burn, 30%)]
//		Moves["Sunny Day"] = // MoveType.Status, BattleType.Fire, BattlefieldMove("Sunny"), 5 PP 
//		Moves["V-create"] = // CompositeMove [DamageMove(MoveType.Physical, BattleType.Fire, 180, 95, 5), StatMove(Def - 1), StatMove(SpDef - 1), StatMove(Spd - 1)]
//		Moves["Will-O-Wisp"] = // MoveType.Status, BattleType.Fire, 15 PP, StatusMove(Burn, 85)
//		
//		// Dragon
//		Moves["Draco Meteor"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Dragon, 130, 90, 5), StatMove(SpAtk - 2)]
//		Moves["Dragon Breath"] = // CompositeMove [DamageMove(MoveType.Special, BattleType.Dragon, 60, 100, 20), StatusMove(Paralyze, 30%)]
//		Moves["Dragon Claw"] = // DamageMove(MoveType.Physical, BattleType.Dragon, 80, 100, 15)
//		Moves["Dragon Dance"] = // CompositeMove(MoveType.Status, BattleType.Dragon, 20, [StatMove(Atk + 1), StatMove(Spd + 1)])
//		Moves["Dragon Pulse"] = // DamageMove(MoveType.Special, BattleType.Dragon, 85, 100, 10, "Pulse Move")
//		Moves["Dragon Rage"] = // DamageMove(MoveType.Special, BattleType.Dragon, 40, 100, 10, ExactPowerDamage)
//		Moves["Dragon Rush"] = // CompositeMove([DamageMove(MoveType.Physical, BattleType.Dragon, 100, 75, 10), StatusMove(Flinch, 20%)]
//		Moves["Dragon Tail"] = // CompositeMove([DamageMove(MoveType.Physical, BattleType.Dragon, 60, 90, 100, Priority: -6), ForceSwitchMove)
//		Moves["Dual Chop"] = // DamageMove(MoveType.Physical, BattleType.Dragon, 40, 90, 15, MultiFixedStrikes(2))
//		// TODO: FIGURE OUT
//		Moves["Outrage"] = // RepeatMove(MoveType.Physical, BattleType.Dragon, 10 PP, [DamageMove(120, 100)], 2-3 times), StatusMove(ConfusesSelf, 100%)
//		Moves["Roar of Time"] = // SequentialMove([DamageMove(MoveType.Special, BattleType.Dragon, 150, 90, 5), RestMove])
//		Moves["Spacial Rend"] = // DamageMove(MoveType.Special, BattleType.Dragon, 100, 95, 5, Crit+1)
//		Moves["Twister"] = // CompositeMove([DamageMove(MoveType.Special, BattleType.Dragon, 40, 100, 20, Bypasses Fly/Bounce/Skydrop invul (+ 2x damage)), StatusMove(Flinch, 20%)])
		
		// Fairy
		
		
		// The <X> was hurt by its burn!
		// The <X> already has a burn.
	}
}

}}