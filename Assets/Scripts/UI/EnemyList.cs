using UnityEngine;
using UnityEngine.UI;

public class EnemyList : MonoBehaviour {

  public Text[] EnemyNames;
  public int[] EnemyQty;
  public Text[] EnemyQtyText;

  public void Clear() {
    for(var i =0; i < EnemyNames.Length; i++) {
      EnemyNames[i].text = "";
      EnemyQty[i] = 0;
      EnemyQtyText[i].text = "";
    }
  }

  public void SetQtys() {
    for (var i = 0; i < EnemyNames.Length; i++) {
      if (EnemyQty[i] == 0) { return; }
      EnemyQtyText[i].text = EnemyQty[i].ToString();
    }
  }
}
