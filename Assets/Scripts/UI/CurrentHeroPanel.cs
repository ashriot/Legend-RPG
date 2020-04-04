using UnityEngine;
using UnityEngine.UI;

public class CurrentHeroPanel : MonoBehaviour
{
  public Image HeroImage, StatusImage;
  public int Constitution;
  public Text CurHp, MaxHp;

  public GameObject Setup(BattlePanel panel) {
    HeroImage.sprite = panel.Unit.Sprite;
    StatusImage.gameObject.SetActive(false);
    CurHp.text = panel.Unit.CurHp.ToString();
    MaxHp.text = $"/{ panel.Unit.MaxHp }";
    return gameObject;
  }
}
