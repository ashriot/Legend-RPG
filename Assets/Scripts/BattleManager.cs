using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
  public static BattleManager Instance;

  public EventSystem EventSystem;

  public Hero[] HeroesToImport;

  [Header("Panel Refs")]
  public HeroPanel[] HeroPanels;
  public EnemyPanel[] EnemyPanels;
  public CommandPanels CommandPanels;
  public CurrentHeroPanel CurrentHeroPanel;
  public GameObject FightPanel;

  bool battleActive, turnWaiting, choosingEnemyTarget, choosingHeroTarget;
  List<BattlePanel> activeBattlers = new List<BattlePanel>();
  List<BattlePanel> turnOrder = new List<BattlePanel>();
  BattlePanel currentPanel;

    void Awake() {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    
    void Start() { }

    void Update() {
      if (Input.GetKeyDown(KeyCode.T) && !battleActive) {
        InitializeBattle(new string[] {"devil", "devil"});
      }

      if (CommandPanels.isActive) {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") != 0) {
          CommandPanels.HorizontalMovement();
          Debug.Log($"Swapping pages to: { CommandPanels.PageNumber }");
        }
        else if (Input.GetButtonDown("Vertical")) {
          CommandPanels.VerticalMovement(Input.GetAxisRaw("Vertical"));
        }
        if (!battleActive || turnWaiting) return;
      }
    }

    void FixedUpdate() {

    }

    void InitializeBattle(string[] enemiesToLoad) {
      if (battleActive) return;

      battleActive = true;
      CommandPanels.gameObject.SetActive(false);
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
      
      activeBattlers.AddRange(HeroPanels.Where(p => p.gameObject.activeInHierarchy).ToList());
      activeBattlers.AddRange(EnemyPanels.Where(p => p.gameObject.activeInHierarchy).ToList());

      BattleFlow();
    }

    void BattleFlow() {
      if (turnOrder.Count == 0) {
        FightCheck();
        DetermineTurnOrder();
      }
      currentPanel = turnOrder[0];
      turnOrder.RemoveAt(0);
      Debug.Log($"Current Turn: { currentPanel.Unit.Name }");
      turnWaiting = true;

      if (currentPanel.IsHero) {
        Debug.Log("Hero Turn!");
        CurrentHeroPanel.Setup(currentPanel).SetActive(true);
        if (((HeroPanel)currentPanel).LastUsedAction is null) {
          CommandPanels.PageNumber = 0;
          CommandPanels.CursorPos = 0;
        }
        var commands = ((Hero)currentPanel.Unit).Commands.Where(c => c != null).ToList();
        for (var i = 0; i < CommandPanels.Commands.Length; i++) {
          if (i < commands.Count) {
            CommandPanels.Commands[i].Setup(commands[i]);
          } else {
            CommandPanels.Commands[i].Clear();
          }
        }
        CommandPanels.gameObject.SetActive(true);
        CommandPanels.SetPages();

      } else {
        Debug.Log("Enemy Turn!");
      }
    }

    void FightCheck() {
      FightPanel.SetActive(true);
      
    }

    void DetermineTurnOrder() {
      Debug.Log("Determining Turn Order");
      for (var i = 0; i < activeBattlers.Count; i++) {
        activeBattlers[i].RollInitiative();
        Debug.Log($"{ activeBattlers[i].Unit.Name}: { activeBattlers[i].Initiative } ");
      }
      turnOrder = activeBattlers.OrderByDescending(t => t.Initiative).ToList();
    }

    public void ClickCommand(int buttonId) {
      CommandPanels.gameObject.SetActive(false);
      var command = ((Hero)currentPanel.Unit).Commands[buttonId];
      DetermineTargetType(command.Action.TargetType);

      Debug.Log($"Button with ID: { buttonId } was clicked -> { command.Name }.");
    }

    void DetermineTargetType(TargetTypes type) {
      if (type == TargetTypes.OneEnemy) {
        var enemy = EnemyPanels.First(p => !p.IsDead);
        enemy.Cursor.SetActive(true);
      }
    }

}
