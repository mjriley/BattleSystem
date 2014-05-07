using System.Collections.Generic;

public class AbilityDescriptionsEnglish : IAbilityDescriptions
{
	public Dictionary<string, string> descriptions;
	
	private bool m_isInit = false;
	
	private void Init()
	{
		descriptions = new Dictionary<string, string>();
		descriptions["Ember"] = "The target is attacked with small flames. It may also leave the target with a burn.";
		descriptions["Fire Fang"] = "The user bites with flame-cloaked fangs. It may also make the target flinch or leave it burned.";
		
		descriptions["Vine Whip"] = "The target is struck with slender, whiplike vines to inflict damage.";
		descriptions["Razor Leaf"] = "Sharp-edged leaves are launched to slash at the opposing team. Critical hits land more easily.";
		descriptions["Fury Swipes"] = "The target is raked with sharp claws or scythes for two to fives times in quick succession.";
		descriptions["Tackle"] = "A physical attack in which the user charges and slams into the target with its whole body.";
		descriptions["Scratch"] = "Hard, pointed, and sharp claws rake the target to inflict damage.";
		descriptions["Splash"] = "The user just flops and splashes around to no effect at all...";
		descriptions["Flail"] = "The user flails about aimlessly to attack. It becomes more powerful the less HP the user has.";
		descriptions["Take Down"] = "A reckless, full-body charge attack for slamming into the target. It also damages the user a little.";
		descriptions["Growl"] = "The user growls in an endearing way, making opposing Pokemon less wary. This lowers their Attack stats.";
		
		descriptions["Rollout"] = "The user continually rolls into the target over five turns. It becomes stronger each time it hits.";
		descriptions["Bite"] = "The target is bitten with viciously sharp fangs. It may make the target flinch.";
		descriptions["Water Gun"] = "The target is blasted with a forceful shot of water.";
		descriptions["Bubble"] = "A spray of countless bubbles is jetted at the opposing team. It may also lower the targets' Speed stats.";
		descriptions["Dragon Rage"] = "This attack hits the target with a shock wave of pure rage. This attack always inflicts 40 HP damage.";
	}
	
	
	public string GetDescription(string abilityName)
	{
		if (!m_isInit)
		{
			Init();
			m_isInit = true;
		}
		
		return descriptions[abilityName];
	}
}

