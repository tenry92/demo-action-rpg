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
    private float gravity = 10f;

    [SerializeField]
    private Transform model;

    [SerializeField]
    private InputAction moveAction;
    #endregion

    private CharacterController characterController;

    private Animator animator;

    private Vector3 velocity;

    /// Direction in degrees.
    private float facingDirection = 0f;

    /// Movement vector on the XZ-plane.
    public Vector3 MovementDirection {
      get {
        return new Vector3(this.velocity.x, 0f, this.velocity.z);
      }
    }

    public float MovementSpeed {
      get {
        return this.MovementDirection.magnitude;
      }
    }

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

      this.velocity.x = input.x * this.moveSpeed;
      this.velocity.z = input.y * this.moveSpeed;

      if (this.characterController.isGrounded) {
        Debug.Log("grounded");
        this.velocity.y = 0f;
      } else {
        Debug.Log("not grounded");
        this.velocity.y -= this.gravity * Time.deltaTime;
      }

      this.characterController.Move(this.velocity * Time.deltaTime);

      if (this.MovementSpeed > 0f) {
        var targetRotation = Quaternion.LookRotation(this.MovementDirection, Vector3.up);
        this.model.transform.rotation = Quaternion.RotateTowards(this.model.transform.rotation, targetRotation, this.rotationSpeed * Time.deltaTime);
        this.facingDirection = Vector3.SignedAngle(Vector3.forward, this.model.transform.rotation * Vector3.forward, Vector3.up);
      }
    }

    private void UpdateAnimation() {
      this.animator?.SetFloat("MovementSpeed", this.MovementSpeed);
    }

    private IEnumerable<InputAction> GetAllActions() {
      yield return this.moveAction;
    }
  }
}
