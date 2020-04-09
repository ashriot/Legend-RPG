using UnityEngine;
using UnityEngine.UI;

public class CurrentHeroPanel : MonoBehaviour
{
  public Image HeroImage, StatusImage;
  public int Constitution;
  public Text CurHp, MaxHp;

  public void Setup(BattlePanel panel) {
    HeroImage.sprite = panel.Unit.Sprite;
    StatusImage.gameObject.SetActive(false);
    CurHp.text = panel.Unit.CurHp.ToString();
    MaxHp.text = $"/{ panel.Unit.MaxHp }";
    gameObject.SetActive(true);
  }

  public void Hide() {
    gameObject.SetActive(false);
  }
}
