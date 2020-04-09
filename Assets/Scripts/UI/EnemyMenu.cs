using System.Linq;
using UnityEngine;

public class EnemyMenu : ButtonMenu {

  public override void VerticalMovement(float input) {

    var availableTargets = MenuButtons.Where(mb => mb.GetComponent<EnemyPanel>().IsAlive).ToList();
    var len = availableTargets.Count;
    var pos = input > 0 ? 1 : -1;

    do {
      CursorPos -= pos;
      CursorPos %= len;
      if (CursorPos < 0) {
        CursorPos += len;
      }
    } while (!MenuButtons[CursorPos].GetComponent<EnemyPanel>().IsAlive);
    
    OnButtonSelect(MenuButtons[CursorPos]);
  }

  public override void HorizontalMovement(float input) { }

  public override void Submit() {
    base.Submit();
    Debug.Log($"Command Menu Submit -> { (MenuButtons[CursorPos]).GetComponent<EnemyPanel>().Unit.Name }");
    BattleManager.GetInstance().ClickEnemyButton(MenuButtons[CursorPos].GetComponent<EnemyPanel>());
  }
}
