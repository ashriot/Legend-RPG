using UnityEngine;

public class EssentialsLoader : Singleton<EssentialsLoader> {

  public GameObject uiScreen, player, gameManager;

  void Start() {
    PlayerController.GetInstance();
    GameManager.GetInstance();
  }

}
