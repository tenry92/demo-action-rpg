using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(PlayerController))]
  public class PlayerInputController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InputAction moveAction;

    [SerializeField]
    private InputAction attackAction;
    #endregion

    private PlayerController playerController;

    private void Awake() {
      Debug.Assert(this.moveAction != null);
      Debug.Assert(this.attackAction != null);
      Debug.Assert(this.playerController = this.GetComponent<PlayerController>());
    }

    private void OnEnable() {
      foreach (var action in this.GetAllActions()) {
        action?.Enable();
      }

      this.attackAction.performed += this.OnAttack;
    }

    private void OnDisable() {
      foreach (var action in this.GetAllActions()) {
        action?.Disable();
      }

      this.attackAction.performed -= this.OnAttack;
    }

    private void Update() {
      this.UpdateMovement();
    }

    private void UpdateMovement() {
      var input = this.moveAction.ReadValue<Vector2>();

      if (input.magnitude > 0f) {
        var direction = -Vector2.SignedAngle(Vector2.right, input.normalized) + 90f;
        this.playerController.Move(direction, input.magnitude);
      }
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.moveAction;
      yield return this.attackAction;
    }

    private async void OnAttack(InputAction.CallbackContext context) {
      await this.playerController.Attack();
    }
  }
}
