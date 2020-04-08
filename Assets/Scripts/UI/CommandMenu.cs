using System.Linq;

public class CommandMenu : ButtonMenu {
  
  public CommandMenu Setup(HeroPanel heroPanel) {
    if (heroPanel.LastUsedAction is null) {
      CursorPos = 0;
    }
    var commands = ((Hero)heroPanel.Unit).Commands.Where(c => c != null).ToList();
    for (var i = 0; i < MenuButtons.Length; i++) {
      if (i < commands.Count) {
        ((CommandButton)MenuButtons[i]).Setup(commands[i]);
      } else {
        ((CommandButton)MenuButtons[i]).Clear();
      }
    }

    return this;
  }
}

