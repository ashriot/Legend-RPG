using UnityEngine;

public abstract class UnitScript : ScriptableObject {

  public string Id;
  public string Name;
  public Sprite Sprite;
  public bool IsDead { get { return CurHp <= 0; } }
  public Command[] Commands = new Command[6];

	[LabeledArray(typeof(Stats))]
	public int[] BaseStats = new int[(int)Stats.Count];

	public int CurHp { 
		get { return GetStat(Stats.Hp); }
		set { SetStat(Stats.Hp, value); }
	}
	
	public int MaxHp { 
		get { return GetStat(Stats.MaxHp); }
		set { SetStat(Stats.MaxHp, value); }
	}

	public int Strength { 
		get { return GetStat(Stats.Str); }
		set { SetStat(Stats.Str, value); }
	}

	public int Agility { 
		get { return GetStat(Stats.Agi); }
		set { SetStat(Stats.Agi, value); }
	}

	public int Psyche {
		get { return GetStat(Stats.Mag); }
		set { SetStat(Stats.Mag, value); }
	}

	public int Defense {
		get { return GetStat(Stats.Def); }
		set { SetStat(Stats.Def, value); }
	}

  public int Xp {
    get { return _xp; }
    set { _xp = value; }
  }

  public virtual int GetStat (Stats type) {
    var index = (int)type;
		return BaseStats[index];
	}

  int _xp;
  
  Stats[] statOrder = new Stats[] {
    Stats.Hp,
    Stats.MaxHp,
    Stats.Str,
    Stats.Agi,
    Stats.Mag,
    Stats.Def
  };

  public void SetStat (Stats type, int value) {
		int index = (int)type;
		if (BaseStats[index] == value) { return; }

		int fromValue = BaseStats[index];
		BaseStats[index] = value;
	}

	public int GetBaseStat (Stats type) {
		int index = IndexForStat(type);
		return index != -1 ? BaseStats[index] : 0;
	}

	public void SetBaseStat (Stats type, int value) {
		int index = IndexForStat(type);
		if (index != -1) { BaseStats[index] = value; }
	}

	// public int GetBaseGrowth (Stats type) {
	// 	int index = IndexForStat(type);
	// 	return index != -1 ? statGrowth[index] : 0;
	// }

	// public void SetBaseGrowth (Stats type, int value) {
	// 	int index = IndexForStat(type);
	// 	if (index != -1) { statGrowth[index] = value; }
	// }

	int IndexForStat (Stats type) {
    return (int)type;
	}
}

public enum Stats {
  Hp,
  MaxHp,
  Str,
  Agi,
  Mag,
  Def,
  Count
}