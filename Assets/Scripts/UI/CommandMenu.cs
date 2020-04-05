using UnityEngine;

public class CommandMenu : ButtonGroup
{
  public bool isActive { get { return gameObject.activeInHierarchy; } }
  public int PageNumber, CursorPos;
  public GameObject Page0, Page1;
  public CommandPanel[] Commands;

  int pageSize = 4;

  public void SetPages() {
    Enable();
    Page0.SetActive(PageNumber == 0);
    Page1.SetActive(PageNumber == 1);
    int index = PageNumber * pageSize + CursorPos;
    Commands[index].Button.Select();
  }

  public void VerticalMovement(float input) {
    CursorPos -= (int)input;
    CursorPos %= pageSize;
    if (CursorPos < 0) {
      CursorPos += pageSize;
    }
    // Debug.Log($"Vertical: { input } CursorPos: {CursorPos} PageSize: { pageSize }");
    SetPages();
  }

  public void HorizontalMovement() {
    if (PageNumber == 0) {
      PageNumber = 1;
    } else {
      PageNumber = 0;
    }
    SetPages();
  }
}
