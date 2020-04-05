
public class FightMenu : ButtonMenu {
  
  public bool isActive { get { return gameObject.activeInHierarchy; } }
  public int CursorPos;
  public CommandPanel[] Commands;

  public void VerticalMovement(float input) {
    // CursorPos -= (int)input;
    // CursorPos %= pageSize;
    // if (CursorPos < 0) {
    //   CursorPos += pageSize;
    // }
    // Debug.Log($"Vertical: { input } CursorPos: {CursorPos} PageSize: { pageSize }");
  }

  public void Setup() {
    gameObject.SetActive(true);
    Enable();
  }
  
}
