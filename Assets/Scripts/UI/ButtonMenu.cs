using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour {

  public ButtonMenu nextMenu;
  public List<MenuButton> MenuButtons;

  public bool isActive { get { return gameObject.activeInHierarchy; } }
  public int CursorPos;

  bool isEnabled;

  void Update() {
    if (!isEnabled) return;

    if (Input.GetButtonDown("Horizontal")) {
      HorizontalMovement(Input.GetAxisRaw("Horizontal"));
    }
    else if (Input.GetButtonDown("Vertical")) {
      VerticalMovement(Input.GetAxisRaw("Vertical"));
    } else if (Input.GetButtonDown("Fire1")) {
      OnSubmit();
    }
  }

  public virtual void VerticalMovement(float input) {
    CursorPos -= (int)input;
    CursorPos %= MenuButtons.Count;
    if (CursorPos < 0) {
      CursorPos += MenuButtons.Count;
    }
    OnButtonSelect(MenuButtons[CursorPos]);
  }

  public virtual void HorizontalMovement(float input) { }

  public void OnButtonSelect(MenuButton button) {
    AudioManager.instance.PlaySfx("Cursor1");
    ResetButtons();
    button.cursor.color = Color.white;
  }

  public virtual void OnSubmit() {
    AudioManager.instance.PlaySfx("Select1");
    ResetButtons();
    MenuButtons[CursorPos].cursor.color = Color.white;
  }

  public void ResetButtons() {
    foreach (var button in MenuButtons) {
      button.cursor.color = Color.clear;
    }
  }

  public void Enable() {
    ResetButtons();
    isEnabled = true;
    Show();
    MenuButtons[CursorPos].cursor.color = Color.white;
  }

  public void Disable() {
    isEnabled = false;
  }

  public void Hide() => gameObject.SetActive(false);
  public void Show() => gameObject.SetActive(true);
}