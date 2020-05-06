using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
  public bool canInteract;
  public InteractionType interactionType;

  public abstract void Interact();

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      canInteract = true;
      PlayerController.GetInstance().SetInteraction(this);
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.tag == "Player") {
      canInteract = false;
      PlayerController.GetInstance().ClearInteraction();
    }
  }
}

public enum InteractionType {
  Talk,
  Question,
  Exclaim,
  Count
}
