using UnityEngine;
using UnityEngine.UI;

public abstract class BattlePanel : MonoBehaviour {
  public Image Image;
  public int Initiative;
  public Unit Unit;
  public bool IsHero { get { return Unit?.GetType() == typeof(Hero); } }
  public bool IsAlive { get { return Unit?.CurHp > 0; } }

  [HideInInspector]
  public Command commandToExecute;
  [HideInInspector]
  public BattlePanel targetOfCommand;

  // status effects

  public virtual void Setup() {}

  public void RollInitiative() {
    Initiative = Random.Range(0, Unit.Agility);
  }
}
