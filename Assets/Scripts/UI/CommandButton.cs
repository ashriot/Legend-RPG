using UnityEngine.UI;

public class CommandButton : MenuButton {
  public Text Name, Amt;
  public Image Image;
  public Command Command;

  public void Setup(Command command) {
    Command = command;
    Name.text = Command.Name;
    Amt.text = Command.CurrentAmt.ToString();
    Image.sprite = Command.Sprite;
  }

  public void Clear() {
    Command = null;
    Name.text = Amt.text = string.Empty;
    Image.sprite = null;
  }
}
