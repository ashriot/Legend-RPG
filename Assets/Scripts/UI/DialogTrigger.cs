using UnityEngine;

public class DialogTrigger : InteractableObject {

  public Animator animator;
  public Dialog dialog;

  bool canActivate;

  public override void Interact() {
    if (!DialogManager.GetInstance().dialogBox.activeInHierarchy) {
      DialogManager.GetInstance().StartDialog(dialog, animator);
    } else if (DialogManager.GetInstance().dialogBox.activeInHierarchy) {
      DialogManager.GetInstance().DisplayNextSentence();
    }
  }
}
