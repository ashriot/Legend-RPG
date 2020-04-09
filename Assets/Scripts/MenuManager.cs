
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager> {

  public List<ButtonMenu> buttonMenus;
  public ButtonMenu previousMenu;
  public ButtonMenu activeMenu;

  PlayerInputActions inputAction;
  Vector2 directionalInput;

  protected override void Awake() {
    base.Awake();

    inputAction = new PlayerInputActions();
    inputAction.PlayerControls.Directional.performed += ctx => directionalInput = ctx.ReadValue<Vector2>();
  }

  void Start() {
    buttonMenus = GetComponentsInChildren<ButtonMenu>().ToList();
    buttonMenus.ForEach(m => m.gameObject.SetActive(false));
  }

  public void SwitchMenus(ButtonMenu menu) {
    previousMenu = activeMenu;
    activeMenu = menu;
  }

  void Update() {
    var h = directionalInput.x;
    var v = directionalInput.y;
    var ok = inputAction.PlayerControls.OK.triggered;
    var cancel = inputAction.PlayerControls.Cancel.triggered;
    var menu = inputAction.PlayerControls.Menu.triggered;
    var directionTriggered = inputAction.PlayerControls.Directional.triggered;

    if (v != 0 && directionTriggered) {
      activeMenu.VerticalMovement(v);
    } else if (h != 0 && directionTriggered) {
      activeMenu.HorizontalMovement(h);
    } else if (ok) {
      activeMenu.Submit();
    }
  }

  void OnEnable() {
    inputAction.Enable();
  }

  void OnDisable() {
    inputAction.PlayerControls.Directional.performed -= ctx => directionalInput = ctx.ReadValue<Vector2>();
    inputAction.Disable();
  }
}
