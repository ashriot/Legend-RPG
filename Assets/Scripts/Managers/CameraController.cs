using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

  public Transform target;
  public Tilemap tilemap;

  Vector3 bottomLeftLimit, topRightLimit;
  float halfHeight, halfWidth;

  void Start() {
    target = PlayerController.GetInstance().transform;

    halfHeight = Camera.main.orthographicSize;
    halfWidth = halfHeight * Camera.main.aspect;
    bottomLeftLimit = tilemap.localBounds.min + new Vector3(halfWidth - 5, halfHeight + 1f, 0f);
    topRightLimit = tilemap.localBounds.max + new Vector3(-halfWidth + 5, -halfHeight + 1f, 0f);

    Debug.Log(tilemap.size);
  }

  void LateUpdate() {
    transform.position = new Vector3(target.position.x, target.position.y - 6, transform.position.z);

    transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
      Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
      transform.position.z);
  }
}
