using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour {

  public Sprite selectedSprite;
  public ButtonMenu nextMenu;
  public List<MenuButton> MenuButtons = new List<MenuButton>();

  public bool isActive { get { return gameObject.activeInHierarchy; } }
  public int CursorPos, menuSize;
  // public CommandPanel[] Commands;

  void Start() {
    OnButtonSelect(MenuButtons[CursorPos]);
  }

  void Update() {
    if (Input.GetButtonDown("Horizontal")) {
      HorizontalMovement(Input.GetAxisRaw("Horizontal"));
    }
    else if (Input.GetButtonDown("Vertical")) {
      VerticalMovement(Input.GetAxisRaw("Vertical"));
    }
  }
  
  public void VerticalMovement(float input) {
    CursorPos -= (int)input;
    CursorPos %= menuSize;
    if (CursorPos < 0) {
      CursorPos += menuSize;
    }
    // Debug.Log($"Vertical: { input } CursorPos: {CursorPos} PageSize: { pageSize }");
    OnButtonSelect(MenuButtons[CursorPos]);
  }

  public void HorizontalMovement(float input) {
    // swap menu
  }

  public void Subscribe(MenuButton button) {
    MenuButtons.Add(button);
    Debug.Log("Adding");
    button.cursor.color = Color.clear;
  }

  public void OnButtonSelect(MenuButton button) {
    AudioManager.instance.PlaySfx("Cursor1");
    ResetButtons();
    button.cursor.color = Color.white;
    button.cursor.sprite = selectedSprite;
  }

  public void OnButtonSubmit(MenuButton button) {
    ResetButtons();
    button.cursor.sprite = selectedSprite;
    Debug.Log("Submitting");
  }

  public void ResetButtons() {
    Debug.Log("Reset");
    foreach (var button in MenuButtons) {
      button.cursor.color = Color.clear;
    }
  }

  public void Enable() {
    OnButtonSelect(MenuButtons[CursorPos]);
  }

  public void Disable() {
  }

  public void Hide() {
    gameObject.SetActive(false);
    Enable();
  }
  public void Show() => gameObject.SetActive(true);
}