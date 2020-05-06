using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu> {

  public GameObject mainMenu;

  public Text[] curHpTexts, maxHpTexts;
  public Image[] charImage;
  public GameObject[] unitHolder;
  
  Unit[] heroes;

  private void OnEnable() {
    UpdateStats();
  }

  public void UpdateStats() {
    heroes = GameManager.GetInstance().heroes;

    for (var i = 0; i < heroes.Length; i++) {
      if (!heroes[i].gameObject.activeInHierarchy) {
        unitHolder[i].SetActive(false);
        continue;
      }
      unitHolder[i].SetActive(true);
      const string colorTag = "<color=\"#32323255\">";
      const string endTag = "</color>";
      var curHp = string.Empty;
      if (heroes[i].CurHp < 10) {
        curHp = colorTag + "000" + endTag;
      } else if (heroes[i].CurHp < 100) {
        curHp = colorTag + "00" + endTag;
      } else if (heroes[i].CurHp < 1000) {
        curHp = colorTag + "0" + endTag;
      }
      curHp += heroes[i].CurHp.ToString();
      curHpTexts[i].text = curHp;
      
      var maxHp = string.Empty;
      if (heroes[i].MaxHp < 10) {
        maxHp = colorTag + "000" + endTag;
      } else if (heroes[i].MaxHp < 100) {
        maxHp = colorTag + "00" + endTag;
      } else if (heroes[i].MaxHp < 1000) {
        maxHp = colorTag + "0" + endTag;
      }
      maxHp += heroes[i].MaxHp.ToString();
      maxHpTexts[i].text = $"/{maxHp}";
      charImage[i].sprite = heroes[i].Sprite;
    }
  }
}
