using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(PlayerController))]
  public class PlayerInputController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private Variables.Switch enabledSwitch;
    #endregion

    private PlayerController playerController;

    private InputAction moveAction;

    private InputAction attackAction;

    private void Awake() {
      this.playerController = this.GetComponent<PlayerController>();
      Debug.Assert(this.playerController != null);

      if (enabledSwitch != null) {
        enabled = enabledSwitch.Value;
      }
    }

    private void OnEnable() {
      var map = GameController.Instance.InputManager.ListenToMap("Game");

      moveAction = map.FindAction("Move");
      attackAction = map.FindAction("Attack");

      this.attackAction.performed += this.OnAttack;
    }

    private void OnDisable() {
      GameController.Instance.InputManager.UnlistenToMap("Game");

      if (attackAction != null) {
        attackAction.performed -= this.OnAttack;
      }
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
