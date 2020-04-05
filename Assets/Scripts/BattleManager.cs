using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

  public static BattleManager Instance;
  public Hero[] HeroesToImport;

  [Header("Panel Refs")]
  public HeroPanel[] HeroPanels;
  public EnemyPanel[] EnemyPanels;
  public CommandMenu CommandMenu;
  public EnemyList EnemyList;
  public CurrentHeroPanel CurrentHeroPanel;
  public FightMenu FightMenu;

  [Header("Button Groups")]
  public ButtonMenu EnemyMenu;
  public ButtonMenu HeroMenu;

  bool battleActive, turnWaiting, choosingEnemyTarget, choosingHeroTarget;
  List<BattlePanel> activeBattlers = new List<BattlePanel>();
  List<BattlePanel> turnOrder = new List<BattlePanel>();
  BattlePanel currentPanel;

  void Awake() {
    if (Instance is null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
  }

  void Update() {
    // FOR TESTING
    if (!battleActive) { InitializeBattle(new string[] {"devil", "devil"}); }

    if (!battleActive || turnWaiting) return;

  }

  void CleanupBattleScene() {
    CommandMenu.Hide();
    CurrentHeroPanel.gameObject.SetActive(false);
    FightMenu.Hide();
    EnemyMenu.Disable();
    HeroMenu.Disable();
    CommandMenu.Disable();
    FightMenu.Disable();
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

  void BattleFlow() {
    if (turnOrder.Count == 0) {
      DetermineTurnOrder();
    }
    currentPanel = turnOrder[0];
    turnOrder.RemoveAt(0);
    Debug.Log($"Current Turn: { currentPanel.Unit.Name }");
    turnWaiting = true;

    if (currentPanel.IsHero) {
    } else {
      Debug.Log("Enemy Turn!");
    }
  }

  void FightCheck() {
    SetupEnemyList();
    FightMenu.Setup();
    FightMenu.Commands[0].OnSelect();
    turnOrder.Clear();
  }

  void ChooseCommands() {
    FightMenu.Hide();

    currentPanel = turnOrder[0];
    turnOrder.RemoveAt(0);

    if (((HeroPanel)currentPanel).LastUsedAction is null) {
      CommandMenu.CursorPos = 0;
    }
    var commands = ((Hero)currentPanel.Unit).Commands.Where(c => c != null).ToList();
    // for (var i = 0; i < CommandMenu.Commands.Length; i++) {
    //   if (i < commands.Count) {
    //     CommandMenu.Commands[i].Setup(commands[i]);
    //   } else {
    //     CommandMenu.Commands[i].Clear();
    //   }
    // }
    CurrentHeroPanel.Setup(currentPanel);
    CommandMenu.gameObject.SetActive(true);

  }

  void DetermineTurnOrder() {
    Debug.Log("Determining Turn Order");
    for (var i = 0; i < activeBattlers.Count; i++) {
      activeBattlers[i].RollInitiative();
      Debug.Log($"{ activeBattlers[i].Unit.Name}: { activeBattlers[i].Initiative }");
    }
    turnOrder = activeBattlers.OrderByDescending(t => t.Initiative).ToList();
  }

  public void ClickCommand(int buttonId) {
    AudioManager.instance.PlaySfx("Select1");

    if (FightMenu.isActive) {
      EnemyList.gameObject.SetActive(false);
      foreach(var panel in HeroPanels.Where(hp => hp.gameObject.activeInHierarchy)) {
        turnOrder.Add(panel);
      }
      ChooseCommands();
      return;
    }

    var command = ((Hero)currentPanel.Unit).Commands[buttonId];
    Debug.Log($"Button with ID: { buttonId } was clicked -> { command.Name }!");

    // CommandPanels.gameObject.SetActive(false);
    DetermineTargetType(command.Action.TargetType);
  }

  void DetermineTargetType(TargetTypes type) {
    if (type == TargetTypes.OneEnemy) {
      var enemy = EnemyPanels.First(p => !p.IsDead);
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(enemy.GetComponent<Button>().gameObject);
      EnemyMenu.Enable();
      // enemy.GetComponent<Button>().Select();
      choosingEnemyTarget = true;
    }
  }

}
