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

    private Controls controls;

    private void Awake() {
      playerController = GetComponent<PlayerController>();
      Debug.Assert(playerController != null);

      controls = new Controls();

      if (enabledSwitch != null) {
        enabled = enabledSwitch.Value;
      }
    }

    private void OnEnable() {
      controls.Game.Enable();

      controls.Game.Attack.performed += OnAttack;
      controls.Game.Item1.performed += OnUseItem1;
      controls.Game.Item2.performed += OnUseItem2;
    }

    private void OnDisable() {
      controls.Game.Disable();

      controls.Game.Attack.performed -= OnAttack;
      controls.Game.Item1.performed -= OnUseItem1;
      controls.Game.Item2.performed -= OnUseItem2;
    }

    private void Update() {
      UpdateMovement();
    }

    private void UpdateMovement() {
      var input = controls.Game.Move.ReadValue<Vector2>();

      if (input.magnitude > 0f) {
        var direction = -Vector2.SignedAngle(Vector2.right, input.normalized) + 90f;
        playerController.Move(direction, input.magnitude);
      }
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
