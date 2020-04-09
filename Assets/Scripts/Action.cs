using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Legend-RPG/Action")]
public class Action : ScriptableObject {
  public string Name, Description;
  public Stats StatUsed, StatTargeted;
  public ActionTypes ActionType;
  public TargetType TargetType;
  public int Power, HitChance;
}

public enum ActionTypes {
  Weapon,
  Spell,
  Heal,
  Buff,
  Debuff,
  Count
}

public enum TargetType {
  OneEnemy,
  OneRow,
  AllEnemies,
  RandomEnemy,
  Self,
  OneHero,
  AllHeroes,
  RandomHero,
  Count
}
