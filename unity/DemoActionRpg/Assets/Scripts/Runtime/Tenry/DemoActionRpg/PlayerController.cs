using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private InputAction moveAction;
    #endregion

    private CharacterController characterController;

    private void Awake() {
      Debug.Assert(this.moveAction != null);
      Debug.Assert(this.characterController = this.GetComponent<CharacterController>());
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

      this.characterController.Move(new Vector3(input.x, 0f, input.y) * this.moveSpeed * Time.deltaTime);
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.moveAction;
    }
  }
}
