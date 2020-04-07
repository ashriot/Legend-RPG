
using UnityEngine;

public class FightMenu : ButtonMenu {

  public override void OnSubmit() {
    base.OnSubmit();
    Debug.Log("Fight Menu Submit");

    // MenuButtons[CursorPos].command.Execute();

  }
}
