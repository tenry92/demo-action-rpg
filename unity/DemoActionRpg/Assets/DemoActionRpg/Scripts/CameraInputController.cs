using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(CameraController))]
  public class CameraInputController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction zoomAction;
    #endregion

    private CameraController cameraController;

    private void Awake() {
      cameraController = GetComponent<CameraController>();
      Debug.Assert(cameraController != null);
    }

    private void OnEnable() {
      foreach (var action in GetAllActions()) {
        action?.Enable();
      }
    }

    private void OnDisable() {
      foreach (var action in GetAllActions()) {
        action?.Disable();
      }
    }

    private void Update() {
      var zoom = zoomAction.ReadValue<float>();

      if (zoom != 0f) {
        cameraController.Zoom(zoom * Time.deltaTime);
      }
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return zoomAction;
    }
  }
}
