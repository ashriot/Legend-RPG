using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : BattlePanel {
  public Text Hp, MaxHp;
  public Action LastUsedAction;

  public override void Setup() {
    base.Setup();
    Image.sprite = Unit.Sprite;
    Hp.text = Unit.CurHp.ToString();
    MaxHp.text = $"/{ Unit.MaxHp }";
  }
}
