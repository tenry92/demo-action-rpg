using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.GamepadTest {
  public class StickTest : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction action;

    [SerializeField]
    private float amplitude = 10f;
    #endregion

    private RectTransform rectTransform;

    private Vector3 position;

    private void Awake() {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
      position = rectTransform.localPosition;
      Debug.Log(position);
    }

    private void OnEnable() {
      action.Enable();
    }

    private void OnDisable() {
      action.Disable();
    }

    private void Update() {
      var vector = action.ReadValue<Vector2>();

      var newPosition = position + new Vector3(vector.x, vector.y, 0f) * amplitude;

      rectTransform.localPosition = newPosition;
    }
  }
}
