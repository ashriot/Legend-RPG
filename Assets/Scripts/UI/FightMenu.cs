﻿using UnityEngine;

public class FightMenu : ButtonMenu {

  public override void Submit() {
    base.Submit();
    Debug.Log($"Fight Menu Submit -> { ((CommandButton)MenuButtons[CursorPos]).Command.Name }");
    BattleManager.GetInstance().ClickFightButton(((CommandButton)MenuButtons[CursorPos]).Command);
  }
}
