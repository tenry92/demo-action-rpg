using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tenry.GamepadTest {
  public class ButtonTest : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction action;
    #endregion

    private Image image;

    private void Awake() {
      image = GetComponent<Image>();
    }

    private void OnEnable() {
      action.Enable();
    }

    private void OnDisable() {
      action.Disable();
    }
    
    private void Update() {
      image.enabled = action.IsPressed();
    }
  }
}
