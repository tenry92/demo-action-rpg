using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float moveSpeed = 10f;

    /// Model rotation in degrees per second.
    [SerializeField]
    private float rotationSpeed = 360f * 2f;

    [SerializeField]
    private Transform model;

    [SerializeField]
    private InputAction moveAction;
    #endregion

    private CharacterController characterController;

    private Animator animator;

    private float movementSpeed = 0f;

    /// Direction in degrees.
    private float facingDirection = 0f;

    private void Awake() {
      Debug.Assert(this.moveAction != null);
      Debug.Assert(this.model != null);
      Debug.Assert(this.characterController = this.GetComponent<CharacterController>());
      Debug.Assert(this.animator = this.GetComponentInChildren<Animator>());
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
      this.UpdateAnimation();
    }

    private void UpdateMovement() {
      var input = this.moveAction.ReadValue<Vector2>();

      var moveVector = new Vector3(input.x, 0f, input.y) * this.moveSpeed;
      this.characterController.Move(moveVector * Time.deltaTime);
      this.movementSpeed = input.magnitude;

      if (moveVector.magnitude > 0f) {
        var targetRotation = Quaternion.LookRotation(moveVector, Vector3.up);
        this.model.transform.rotation = Quaternion.RotateTowards(this.model.transform.rotation, targetRotation, this.rotationSpeed * Time.deltaTime);
        this.facingDirection = Vector3.SignedAngle(Vector3.forward, this.model.transform.rotation * Vector3.forward, Vector3.up);
      }
    }

    private void UpdateAnimation() {
      this.animator?.SetFloat("MovementSpeed", this.movementSpeed);
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.moveAction;
    }
  }
}
