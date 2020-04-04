using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : BattlePanel
{
  public Image HpBar;
  public Text HitChance;

  public override void Setup() {
    Image.sprite = Unit.Sprite;
    HpBar.fillAmount = (float)Unit.CurHp / Unit.MaxHp;
  }
}
