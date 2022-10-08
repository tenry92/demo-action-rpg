using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tenry.GamepadTest {
  public class TriggerTest : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction action;
    #endregion

    private Image image;

    private Color color;

    private void Awake() {
      image = GetComponent<Image>();
    }

    private void Start() {
      color = image.color;
    }

    private void OnEnable() {
      action.Enable();
    }

    private void OnDisable() {
      action.Disable();
    }

    private void Update() {
      var value = action.ReadValue<float>();

      var newColor = color;
      newColor.a = value;

      image.color = newColor;
    }
  }
}
