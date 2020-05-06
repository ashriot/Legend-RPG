using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager> {

  public static BattleManager Instance;
  public HeroScript[] HeroesToImport;

  [Header("Panel Refs")]
  public HeroPanel[] HeroPanels;
  public EnemyPanel[] EnemyPanels;
  public CommandMenu CommandMenuL, CommandMenuR;
  public EnemyList EnemyList;
  public CurrentHeroPanel CurrentHeroPanel;
  public ButtonMenu FightMenu;

  [Header("Button Groups")]
  public ButtonMenu EnemyMenu;
  public ButtonMenu HeroMenu;

  bool battleActive, turnWaiting, choosingEnemyTarget, choosingHeroTarget;
  List<BattlePanel> activeBattlers = new List<BattlePanel>();
  List<BattlePanel> turnOrder = new List<BattlePanel>();
  BattlePanel currentPanel;
  int currentHeroIndex;

  void Update() {
    // FOR TESTING
    if (!battleActive) { InitializeBattle(new string[] {"devil", "devil"}); }

    if (!battleActive || turnWaiting) return;

  }

  void CleanupBattleScene() {
    CommandMenuL.Hide();
    CommandMenuR.Hide();
    CurrentHeroPanel.gameObject.SetActive(false);
    FightMenu.Hide();
    EnemyMenu.Disable();
    HeroMenu.Disable();
  }

  void InitializeBattle(string[] enemiesToLoad) {
    if (battleActive) return;

    // AudioManager.instance.PlayBgm("Battle1");

    battleActive = true;
    CleanupBattleScene();
    // set gamemanager battle active

    // Heroes
    for(var i = 0; i < HeroPanels.Length; i++) {
      if (i >= HeroesToImport.Length) {
        HeroPanels[i].gameObject.SetActive(false);
      } else {
        var panel = HeroPanels[i];
        panel.Unit = HeroesToImport[i];
        panel.gameObject.SetActive(true);
        panel.Setup();
      }
    }

    // Enemies

    for(var i = 0; i < EnemyPanels.Length; i++) {
      if (i >= enemiesToLoad.Length) { 
        EnemyPanels[i].gameObject.SetActive(false);
        continue;
      } else {
        var panel = EnemyPanels[i];
        panel.Unit = Instantiate(Resources.Load<Enemy>("Enemies/" + enemiesToLoad[i]));
        panel.gameObject.SetActive(true);
        panel.Setup();
      }
    }
    
    SetupEnemyList();

    activeBattlers.AddRange(HeroPanels.Where(p => p.gameObject.activeInHierarchy).ToList());
    activeBattlers.AddRange(EnemyPanels.Where(p => p.gameObject.activeInHierarchy).ToList());

    FightCheck();
  }

  void SetupEnemyList() {
    EnemyList.Clear();

    for (var i = 0; i < EnemyPanels.Length; i++) {
      if (!EnemyPanels[i].gameObject.activeInHierarchy) { break; }

      for (var j = 0; j < EnemyList.EnemyNames.Length; j++) {
        if (EnemyList.EnemyNames[j].text == EnemyPanels[i].Unit.Name) {
          EnemyList.EnemyQty[j]++;
          break;
        } else if (EnemyList.EnemyNames[j].text == "") {
          EnemyList.EnemyNames[j].text = EnemyPanels[i].Unit.Name;
          EnemyList.EnemyQty[j]++;
          break;
        }
      }
    }
    EnemyList.SetQtys();
    EnemyList.gameObject.SetActive(true);
  }

  void DetermineTurnOrder() {
    for (var i = 0; i < activeBattlers.Count; i++) {
      activeBattlers[i].RollInitiative();
      // Debug.Log($"{ activeBattlers[i].Unit.Name}: { activeBattlers[i].Initiative }");
    }
    turnOrder = activeBattlers.OrderByDescending(t => t.Initiative).ToList();
    BattleFlow();
  }

  void BattleFlow() {
    CurrentHeroPanel.Hide();
    CommandMenuL.Hide();
    EnemyMenu.Disable();
    while (turnOrder.Count > 0) {
      currentPanel = turnOrder[0];
      turnOrder.RemoveAt(0);
      turnWaiting = true;

      if (currentPanel.IsHero) {
        ExecuteCommand();
      } else {
      }
    }
    currentHeroIndex = 0;
    currentPanel = HeroPanels[currentHeroIndex];
    FightCheck();
  }

  void FightCheck() {
    SetupEnemyList();
    turnOrder.Clear();
    FightMenu.Show();
    Debug.Log("Enabling Fight Menu");
  }

  public void ClickFightButton(Command command) {
    if (command.Name == "Fight") {
      NextHeroInput();
    } else if (command.Name == "Check") {
      Debug.Log("Checking");
      FightMenu.Hide();
      EnemyList.Hide();
    }

    else {
      Debug.LogError("Missing case for fight button!");
    }
  }

  void NextHeroInput() {
    currentPanel = HeroPanels[currentHeroIndex];
    MenuManager.GetInstance().SwitchMenus(CommandMenuL.Setup((HeroPanel)currentPanel));
    FightMenu.Hide();
    EnemyList.Hide();
    EnemyMenu.Disable();
    CurrentHeroPanel.Setup(currentPanel);
    CommandMenuL.Show();
  }

  public void ClickCommandButton(Command command) {
    currentPanel.commandToExecute = command;
    DetermineTargetType(command.Action.TargetType);
  }

  public void ClickEnemyButton(EnemyPanel enemy) {
    currentPanel.targetOfCommand = enemy;
    currentHeroIndex++;
    if (currentHeroIndex < HeroPanels.Where(hp => hp.IsAlive).Count()) {
      NextHeroInput();
    } else {
      DetermineTurnOrder();
    }
  }

  // public void ClickCommand(int buttonId) {
  //   AudioManager.instance.PlaySfx("Select1");

  //   if (FightMenu.isActive) {
  //     EnemyList.Hide();
  //     foreach(var panel in HeroPanels.Where(hp => hp.gameObject.activeInHierarchy)) {
  //       turnOrder.Add(panel);
  //     }
  //     ChooseCommands();
  //     return;
  //   }

  //   var command = ((Hero)currentPanel.Unit).Commands[buttonId];
  //   Debug.Log($"Button with ID: { buttonId } was clicked -> { command.Name }!");

  //   // CommandPanels.gameObject.SetActive(false);
  //   DetermineTargetType(command.Action.TargetType);
  // }

  void DetermineTargetType(TargetType type) {
    if (type == TargetType.OneEnemy) {
      Debug.Log("Choosing target");
      var enemy = EnemyPanels.First(p => !p.IsAlive);
      EnemyMenu.Enable();
      choosingEnemyTarget = true;
    }
  }

  void ExecuteCommand() {
    var target = currentPanel.targetOfCommand;

    var action = currentPanel.commandToExecute.Action;
    currentPanel.commandToExecute.CurrentAmt--;
    var power = action.Power;
    
    switch (action.StatUsed) {
      case Stats.Str:
        power *= currentPanel.Unit.Strength;
        power += Random.Range(0, currentPanel.Unit.Strength);
        break;
      case Stats.Mag:
        power *= currentPanel.Unit.Psyche;
        power += Random.Range(0, currentPanel.Unit.Psyche);
        break;
      case Stats.Agi:
        power *= currentPanel.Unit.Agility;
        power += Random.Range(0, currentPanel.Unit.Agility);
        break;
      default:
        break;
    }

    switch (action.StatTargeted) {
      case Stats.Def:
        power -= target.Unit.Defense * 3;
        break;
      case Stats.Mag:
        power *= (200 - (target.Unit.Psyche * 2 - currentPanel.Unit.Psyche)) / 200f;
        break;
      default:
        break;
    }

    power = power > 0 ? Mathf.Floor(power) : 0;

    Debug.Log($"{ currentPanel.Unit.Name } used { currentPanel.commandToExecute.Name }! { currentPanel.targetOfCommand.Unit.Name } took { power } damage!");
  }
}
