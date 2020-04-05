using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour{

  public ButtonMenu ButtonMenu;

  public Image cursor;

  public void OnSelect() {
    ButtonMenu.OnButtonSelect(this);
  }

  void Start() {
    ButtonMenu = GetComponentInParent<ButtonMenu>();
    ButtonMenu.Subscribe(this);
  }

}
