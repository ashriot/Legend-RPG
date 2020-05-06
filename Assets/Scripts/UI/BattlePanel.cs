using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattlePanel : MonoBehaviour {
  public Image Image;
  public int Initiative;
  public UnitScript Unit;
  public bool IsHero { get { return Unit?.GetType() == typeof(HeroScript); } }
  public bool IsAlive { get { return Unit?.CurHp > 0; } }
  public List<Command> Commands;

  [HideInInspector]
  public Command commandToExecute;
  [HideInInspector]
  public BattlePanel targetOfCommand;

  // status effects

  public virtual void Setup() {
    foreach(var command in Unit.Commands) {
      if (command is null) return;
      
      Debug.Log("Adding command");
      Commands.Add(Instantiate(command));
    }
  }

  public void RollInitiative() {
    Initiative = Random.Range(0, Unit.Agility);
  }
}
