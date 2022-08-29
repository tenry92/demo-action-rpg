using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(PlayerController))]
  public class PlayerInputController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction moveAction;
    #endregion

    private PlayerController playerController;

    private void Awake() {
      Debug.Assert(this.moveAction != null);
      Debug.Assert(this.playerController = this.GetComponent<PlayerController>());
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
      this.UpdateMovement();
    }

    private void UpdateMovement() {
      var input = this.moveAction.ReadValue<Vector2>();

      this.playerController.Movement = input * this.playerController.MaxMoveSpeed;
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.moveAction;
    }
  }
}
