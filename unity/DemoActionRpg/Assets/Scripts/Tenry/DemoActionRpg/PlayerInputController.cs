using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(PlayerController))]
  public class PlayerInputController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private SharedVariables.Switch enabledSwitch;
    #endregion

    private PlayerController playerController;

    private InputAction moveAction;

    private InputAction attackAction;

    private InputAction item1Action;

    private InputAction item2Action;

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
      item1Action = map.FindAction("Item 1");
      item2Action = map.FindAction("Item 2");

      this.attackAction.performed += this.OnAttack;
      this.item1Action.performed += this.OnUseItem1;
      this.item2Action.performed += this.OnUseItem2;
    }

    private void OnDisable() {
      GameController.Instance.InputManager.UnlistenToMap("Game");

      if (attackAction != null) {
        attackAction.performed -= this.OnAttack;
      }

      if (item1Action != null) {
        item1Action.performed -= this.OnUseItem1;
      }

      if (item2Action != null) {
        item2Action.performed -= this.OnUseItem2;
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

    private void OnUseItem1(InputAction.CallbackContext context) {
      this.playerController.UseItem1();
    }

    private void OnUseItem2(InputAction.CallbackContext context) {
      this.playerController.UseItem2();
    }
  }
}
