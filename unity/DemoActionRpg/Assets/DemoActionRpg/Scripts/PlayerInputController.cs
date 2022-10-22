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
      playerController = GetComponent<PlayerController>();
      Debug.Assert(playerController != null);

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

      attackAction.performed += OnAttack;
      item1Action.performed += OnUseItem1;
      item2Action.performed += OnUseItem2;
    }

    private void OnDisable() {
      GameController.Instance.InputManager.UnlistenToMap("Game");

      if (attackAction != null) {
        attackAction.performed -= OnAttack;
      }

      if (item1Action != null) {
        item1Action.performed -= OnUseItem1;
      }

      if (item2Action != null) {
        item2Action.performed -= OnUseItem2;
      }
    }

    private void Update() {
      UpdateMovement();
    }

    private void UpdateMovement() {
      var input = moveAction.ReadValue<Vector2>();

      if (input.magnitude > 0f) {
        var direction = -Vector2.SignedAngle(Vector2.right, input.normalized) + 90f;
        playerController.Move(direction, input.magnitude);
      }
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return moveAction;
      yield return attackAction;
    }

    private async void OnAttack(InputAction.CallbackContext context) {
      await playerController.Attack();
    }

    private void OnUseItem1(InputAction.CallbackContext context) {
      playerController.UseItem1();
    }

    private void OnUseItem2(InputAction.CallbackContext context) {
      playerController.UseItem2();
    }
  }
}
