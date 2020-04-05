
using System.Collections.Generic;
using System.Linq;

public class MenuManager : Singleton<MenuManager> {

  List<ButtonMenu> buttonMenus;
  ButtonMenu lastActiveMenu;
  int cursorPos;

  void Start() {
    buttonMenus = GetComponentsInChildren<ButtonMenu>().ToList();
    buttonMenus.ForEach(m => m.gameObject.SetActive(false));
  }

  void SwitchMenus(ButtonMenu menu) {
    if (lastActiveMenu != null) {
      lastActiveMenu.gameObject.SetActive(false);
    }

    menu.gameObject.SetActive(true);
    lastActiveMenu = menu;
  }
}
