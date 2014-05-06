using System.Collections.Generic;

public class AbilityFactory
{
	static Dictionary<string, AbstractAbility> m_abilities;
	static IAbilityDescriptions m_descriptions;
	
	static bool m_isInit = false;
	
	public static AbstractAbility GetAbility(string name)
	{
		if (!m_isInit)
		{
			Init();
			m_isInit = true;
		}
		
		return m_abilities[name];
	}
	
	private static void AddAbility(AbstractAbility ability)
	{
		ability.Description = m_descriptions.GetDescription(ability.Name);
		m_abilities[ability.Name] = ability;
	}
	
	public static void Init()
	{
		m_descriptions = new AbilityDescriptionsEnglish();
		m_abilities = new Dictionary<string, AbstractAbility>();
		
		AddAbility(new DamageAbility("Bubble", AbilityType.Special, BattleType.Water, 30, 40, 100));
		AddAbility(new DamageAbility("Water Gun", AbilityType.Special, BattleType.Water, 25, 40, 100));
		
		AddAbility(new DamageAbility("Ember", AbilityType.Special, BattleType.Fire, 25, 40, 100, OnHitEffects.BurnEffect(10)));
		AddAbility(new DamageAbility("Fire Fang", AbilityType.Physical, BattleType.Fire, 15, 65, 95));
		
		AddAbility(new DamageAbility("Vine Whip", AbilityType.Physical, BattleType.Grass, 25, 45, 100));
		AddAbility(new DamageAbility("Razor Leaf", AbilityType.Physical, BattleType.Grass, 25, 55, 95));
		
		AddAbility(new DamageAbility("Tackle", AbilityType.Physical, BattleType.Normal, 35, 50, 100));
		AddAbility(new DamageAbility("Fury Swipes", AbilityType.Physical, BattleType.Normal, 15, 18, 80));
		AddAbility(new DamageAbility("Scratch", AbilityType.Physical, BattleType.Normal, 35, 40, 100));
		AddAbility(new DamageAbility("Splash", AbilityType.Physical, BattleType.Normal, 40, 10, 100));
		AddAbility(new DamageAbility("Flail", AbilityType.Physical, BattleType.Normal, 15, 10, 100));
		AddAbility(new DamageAbility("Take Down", AbilityType.Physical, BattleType.Normal, 20, 90, 85, OnHitEffects.RecoilEffect(25)));
		
		AddAbility(new DamageAbility("Rollout", AbilityType.Physical, BattleType.Rock, 20, 30, 90));
		
		AddAbility(new DamageAbility("Dragon Rage", AbilityType.Special, BattleType.Dragon, 10, 40, 100));
		
		
		AddAbility(new DamageAbility("Bite", AbilityType.Physical, BattleType.Dark, 25, 60, 100));
		
		
//		// Fire
//		Abilities["Blast Burn"] = // Multi-turn Ability -- [new Ability("Blast Burn", AbilityType.Special, BattleType.Fire, 150, 90, 5), RestAbility]; does not recharge if first attack misses
//		Abilities["Blaze Kick"] = // Composite Ability [DamageAbility("Blaze Kick", AbilityType.Physical, BattleType.Fire, 85, 90, 10, CritStage + 1), StatusAbility(Burn, 10%);
//		Abilities["Blue Flare"] = // Composite Ability [DamageAbility(AbilityType.Special, BattleType.Fire, 130, 85, 5), StatusAbility(Burn, 20%)]
//		Abilities["Ember"] = // Composite Ability [DamageAbility(AbilityType.Special, BattleType.Fire, 40, 100, 25), StatusAbility(Burn, 10%)]
//		Abilities["Eruption"] = // DamageAbility(AbilityType.Special, BattleType.Fire, 150, 100, 5, AffectedByHP)
//		Abilities["Fiery Dance"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 80, 100, 100), StatAbility(SpAtk + 1, 50%)]
//		Abilities["Fire Blast"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 110, 85%, 5), StatusAbility(Burn, 10%)]
//		Abilities["Fire Fang"] = // CompositeAbility [DamageAbility(AbilityType.Physical, BattleType.Fire, 65, 95%, 15), StatusAbility(Burn, 10%), StatusAbility(Flinch, 10%)]
//		Abilities["Fire Plege"] = // DamageAbility (AbilityType.Special, BattleType.Fire, 80, 100, 15) // Special Stuff for Double/Triple battles
//		Abilities["Fire Punch"] = // CompositeAbility [DamageAbiilty(AbilityType.Physical, BattleType.Fire, 75, 100, 15), StatusAbility(Burn, 10%)]
//		Abilities["Fire Spin"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 35, 85, 15), StatusAbility(Trap, only lands if parent move lands?)]
//		Abilities["Flame Burst"] = // DamageAbility(AbilityType.Special, BattleType.Fire, 70, 100, 15) // hits adjacent targets
//		Abilities["Flame Charge"] = // CompositeAbility [DamageAbility(AbilityType.Physical, BattleType.Fire, 50, 100, 20), StatAbility(Spd + 1, 100%)]
//		Abilities["Flame Wheel"] = // CompositeAbility [StatusAbility(Thaw, 100%), DamageAbility(AbilityType.Physical, BattleType.Fire, 60, 100, 25), StatusAbility(Burn, 10%)]
//		Abilities["Flamethrower"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 90, 100, 15), StatusAbility(Burn, 10%)]
//		Abilities["Flare Blitz"] = // CompositeAbility [StatusAbility(Thaw, 100%), DamageAbility(AbilityType.Physical, BattleType.Fire, 120, 100, 15, Recoil(33%)), StatusAbility(Burn, 10%)]
//		Abilities["Fusion Flare"] = // CompositeAbility [StatusAbility(Thaw, 100%), DamageAbility(AbilityType.Special, BattleType.Fire, 100, 100, 5)] // Double damage if preceded by Fusion Bolt or Bolt strike
//		Abilities["Heat Crash"] = // DamageAbility(AbilityType.Physical, BattleType.Fire, 40, 100, 10, WeightCalculation)
//		Abilities["Heat Wave"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 95, 90, 10), StatusAbility(Burn, 10%)] // also hits adjacent pokemon
//		Abilities["Incinerate"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 60, 100, 15), ItemClassDestroy(Berries, Gems)] // also hits adjacent pokemon
//		Abilities["Inferno"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 100, 50, 5), StatusAbility(Burn, 100%)]
//		Abilities["Lava Plume"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 80, 100, 15), StatusAbility(Burn, 30%)]
//		Abilities["Magma Storm"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 100, 75, 5), StatusAbility(Trap2-5)]
//		Abilities["Mystical Fire"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 65, 100, 10), StatusAbility(SpAtk-1, 100%)]
//		Abilities["Overheat"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 130, 90, 5), StatAbility(SpAtk - 2, 100%)]
//		Abilities["Sacred Fire"] = // CompositeAbility [StatusAbility(Thaw, 100%), DamageAbility(AbilityType.Physical, BattleType.Fire, 100, 95, 5), StatusAbility(Burn, 50%)]
//		Abilities["Searing Shot"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Fire, 100, 100, 5), StatusAbility(Burn, 30%)]
//		Abilities["Sunny Day"] = // AbilityType.Status, BattleType.Fire, BattlefieldAbility("Sunny"), 5 PP 
//		Abilities["V-create"] = // CompositeAbility [DamageAbility(AbilityType.Physical, BattleType.Fire, 180, 95, 5), StatAbility(Def - 1), StatAbility(SpDef - 1), StatAbility(Spd - 1)]
//		Abilities["Will-O-Wisp"] = // AbilityType.Status, BattleType.Fire, 15 PP, StatusAbility(Burn, 85)
//		
//		// Dragon
//		Abilities["Draco Meteor"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Dragon, 130, 90, 5), StatAbility(SpAtk - 2)]
//		Abilities["Dragon Breath"] = // CompositeAbility [DamageAbility(AbilityType.Special, BattleType.Dragon, 60, 100, 20), StatusAbility(Paralyze, 30%)]
//		Abilities["Dragon Claw"] = // DamageAbility(AbilityType.Physical, BattleType.Dragon, 80, 100, 15)
//		Abilities["Dragon Dance"] = // CompositeAbility(AbilityType.Status, BattleType.Dragon, 20, [StatAbility(Atk + 1), StatAbility(Spd + 1)])
//		Abilities["Dragon Pulse"] = // DamageAbility(AbilityType.Special, BattleType.Dragon, 85, 100, 10, "Pulse Move")
//		Abilities["Dragon Rage"] = // DamageAbility(AbilityType.Special, BattleType.Dragon, 40, 100, 10, ExactPowerDamage)
//		Abilities["Dragon Rush"] = // CompositeAbility([DamageAbility(AbilityType.Physical, BattleType.Dragon, 100, 75, 10), StatusAbility(Flinch, 20%)]
//		Abilities["Dragon Tail"] = // CompositeAbility([DamageAbility(AbilityType.Physical, BattleType.Dragon, 60, 90, 100, Priority: -6), ForceSwitchAbility)
//		Abilities["Dual Chop"] = // DamageAbility(AbilityType.Physical, BattleType.Dragon, 40, 90, 15, MultiFixedStrikes(2))
//		// TODO: FIGURE OUT
//		Abilities["Outrage"] = // RepeatAbility(AbilityType.Physical, BattleType.Dragon, 10 PP, [DamageAbility(120, 100)], 2-3 times), StatusAbility(ConfusesSelf, 100%)
//		Abilities["Roar of Time"] = // SequentialAbility([DamageAbility(AbilityType.Special, BattleType.Dragon, 150, 90, 5), RestAbility])
//		Abilities["Spacial Rend"] = // DamageAbility(AbilityType.Special, BattleType.Dragon, 100, 95, 5, Crit+1)
//		Abilities["Twister"] = // CompositeAbility([DamageAbility(AbilityType.Special, BattleType.Dragon, 40, 100, 20, Bypasses Fly/Bounce/Skydrop invul (+ 2x damage)), StatusAbility(Flinch, 20%)])
		
		// Fairy
		
		
		// The <X> was hurt by its burn!
		// The <X> already has a burn.
	}
}
