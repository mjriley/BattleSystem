using UnityEngine;
using Moves;
using System.Collections.Generic;

public class TypeColor
{
	static Dictionary<BattleType, Color32> m_colors = new Dictionary<BattleType, Color32>
	{
		{BattleType.Bug, new Color32(176, 188, 0, 255)},
		{BattleType.Dark, new Color32(115, 90, 73, 255)},
		{BattleType.Dragon, new Color32(117, 101, 235, 255)},
		{BattleType.Electric, new Color32(255, 198, 5, 255)},
		{BattleType.Fairy, new Color32(245, 183, 249, 255)},
		{BattleType.Fight, new Color32(165, 84, 54, 255)},
		{BattleType.Fire, new Color32(246, 87, 40, 255)},
		{BattleType.Flying, new Color32(153, 174, 250, 255)},
		{BattleType.Ghost, new Color32(95, 100, 184, 255)},
		{BattleType.Grass, new Color32(128, 205, 71, 255)},
		{BattleType.Ground, new Color32(215, 181, 81, 255)},
		{BattleType.Ice, new Color32(90, 205, 233, 255)},
		{BattleType.Normal, new Color32(173, 165, 147, 255)},
		{BattleType.Poison, new Color32(179, 93, 167, 255)},
		{BattleType.Psychic, new Color32(254, 119, 166, 255)},
		{BattleType.Rock, new Color32(190, 165, 84, 255)},
		{BattleType.Steel, new Color32(172, 173, 199, 255)},
		{BattleType.Water, new Color32(44, 156, 255, 255)}
	};
	
	public static Color32 GetColor(BattleType battleType)
	{
		return m_colors[battleType];
	}
}

