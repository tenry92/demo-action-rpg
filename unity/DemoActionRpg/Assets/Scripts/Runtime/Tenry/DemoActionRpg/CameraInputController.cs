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
      this.cameraController = this.GetComponent<CameraController>();
      Debug.Assert(this.cameraController != null);
    }

    private void OnEnable() {
      foreach (var action in this.GetAllActions()) {
        action?.Enable();
      }
    }

    private void OnDisable() {
      foreach (var action in this.GetAllActions()) {
        action?.Disable();
      }
    }

    private void Update() {
      var zoom = this.zoomAction.ReadValue<float>();

      if (zoom != 0f) {
        this.cameraController.Zoom(zoom * Time.deltaTime);
      }
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.zoomAction;
    }
  }
}
