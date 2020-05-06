using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController> {

  public InteractableObject interactableObject;
  public GameObject mainMenu;
  public Button interactButton, menuButton, upButton, downButton, leftButton, rightButton;

  public float movementSpeed;
  public bool canMove, canOpenMenu;
  
  public Transform movePoint;
  public Rigidbody2D rb;

  public LayerMask stopsMovement;
  public Animator animator;
  public Sprite menuSprite, backSprite, talkSprite, questionSprite, exclaimSprite;

  PlayerInputActions inputAction;
  Vector2 directionalInput;

  public void ClickOk() {
    if (interactableObject != null && interactableObject.canInteract) {
      interactableObject.Interact();
    } else if (DialogManager.GetInstance().dialogBox.activeInHierarchy) {
      DialogManager.GetInstance().EndDialog();
    }
  }

  public void ClickMenu() {
    AudioManager.GetInstance().PlaySfx("Blip1");
    mainMenu.SetActive(!mainMenu.activeInHierarchy);
    ToggleMovement(!mainMenu.activeInHierarchy);
    ToggleInteractButton(!mainMenu.activeInHierarchy);
    if (!mainMenu.activeInHierarchy) {
      menuButton.image.sprite = menuSprite;
    } else {
      menuButton.image.sprite = backSprite;
    }
  }

  public void ClickRelease() {
    directionalInput.x = 0;
    directionalInput.y = 0;
  }
  public void ClickUp() {
    directionalInput.y = 1;
  }

  public void ClickDown() {
    directionalInput.y = -1;
  }

  public void ClickRight() {
    directionalInput.x = 1;
  }
  
  public void ClickLeft() {
    directionalInput.x = -1;
  }

  protected override void Awake() {
    base.Awake();
    inputAction = new PlayerInputActions();
    inputAction.PlayerControls.Directional.performed += ctx => directionalInput = ctx.ReadValue<Vector2>();
    mainMenu.SetActive(false);
    interactButton.interactable = false;
  }

  void Start() {
    movePoint.parent = null;
  }

  void Update() {

    var ok = inputAction.PlayerControls.OK.triggered;
    var h = directionalInput.x;

    var v = directionalInput.y;
    var cancel = inputAction.PlayerControls.Cancel.triggered;
    var menu = inputAction.PlayerControls.Menu.triggered;
    // var directionTriggered = inputAction.PlayerControls.Directional.triggered;

    
    if (ok) {
      if (interactableObject != null && interactableObject.canInteract) {
        interactableObject.Interact();
      } else if (DialogManager.GetInstance().dialogBox.activeInHierarchy) {
        DialogManager.GetInstance().EndDialog();
      }
    }

    if (!canMove) {
      v = h = 0f;
    }

    transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);

    if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f) {
      if (Mathf.Abs(v) == 1) {
        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, v*2, 0f), .25f, stopsMovement)) {
          movePoint.position += new Vector3(0f, v*2, 0f);
        } else {
          animator.SetBool("moving", false);
        }
      } else if (Mathf.Abs(h) == 1) {
        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(h*2, 0f, 0f), .25f, stopsMovement)) {
          movePoint.position += new Vector3(h*2, 0f, 0f);
        } else {
          animator.SetBool("moving", false);
        }
      } else {
        animator.SetBool("moving", false);
      }
    } else {
      animator.SetBool("moving", true);
    }
  }

  public void SetInteraction(InteractableObject obj) {
    interactableObject = obj;
    interactButton.interactable = true;
    switch (obj.interactionType)
    {
        case InteractionType.Talk:
        interactButton.image.sprite = talkSprite;
        break;
        case InteractionType.Question:
        interactButton.image.sprite = questionSprite;
        break;
        case InteractionType.Exclaim:
        interactButton.image.sprite = exclaimSprite;
        break;
        default:
        break;
    }
  }

  public void ClearInteraction() {
    interactableObject = null;
    interactButton.interactable = false;
  }

  public void ToggleMovement(bool value) {
    canMove = value;
    upButton.interactable = value;
    downButton.interactable = value;
    leftButton.interactable = value;
    rightButton.interactable = value;
  }

  public void ToggleMenuButton(bool value) {
    ToggleMovement(value);
    canOpenMenu = value;
    menuButton.interactable = value;
  }

  public void ToggleInteractButton(bool value) {
    if (value) {
      if (interactableObject != null) {
        interactButton.interactable = true;
      }
    } else {
      interactButton.interactable = false;
    }
  }

  void OnEnable() {
    inputAction.Enable();
  }

  void OnDisable() {
    inputAction.PlayerControls.Directional.performed -= ctx => directionalInput = ctx.ReadValue<Vector2>();
    inputAction.Disable();
  }
}
