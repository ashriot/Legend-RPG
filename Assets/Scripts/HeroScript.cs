using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Legend-RPG/Hero")]
public class HeroScript : UnitScript
{
	[LabeledArray(typeof(Stats))]
	public int[] BonusStats = new int[(int)Stats.Count];

  Equipment Head, Body, Legs, Feet, FingerL, FingerR;

    public override int GetStat (Stats type) {
    var index = (int)type;
		return BaseStats[index] + BonusStats[index];
	}
}
