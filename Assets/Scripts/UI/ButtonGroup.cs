using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
  public void Enable() {
    GetComponent<CanvasGroup>().blocksRaycasts = false;
  }

  public void Disable() {
    GetComponent<CanvasGroup>().blocksRaycasts = true;
  }

  public void Hide() => gameObject.SetActive(false);
  public void Show() => gameObject.SetActive(true);
}
