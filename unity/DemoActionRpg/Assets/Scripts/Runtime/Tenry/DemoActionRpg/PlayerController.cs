using UnityEngine;

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
    #endregion

    private CharacterController characterController;

    private Animator animator;

    private Vector3 velocity;

    private Vector3 respawnPosition;

    /// Direction in degrees.
    private float facingDirection = 0f;

    private Weapon weapon;

    public float MaxMoveSpeed => this.moveSpeed;

    public bool IsAttacking { get; set; }

    /// Movement vector on the XZ-plane.
    public Vector3 MovementDirection => new Vector3(this.velocity.x, 0f, this.velocity.z);

    public Vector2 Movement {
      get {
        return new Vector2(this.MovementDirection.x, this.MovementDirection.z);
      }
      set {
        this.velocity.x = value.x;
        this.velocity.z = value.y;
      }
    }

    public float MovementSpeed => this.MovementDirection.magnitude;

    public float FallingSpeed => -this.velocity.y;

    private void Awake() {
      this.respawnPosition = this.transform.position;

      Debug.Assert(this.model != null);
      Debug.Assert(this.characterController = this.GetComponent<CharacterController>());
      Debug.Assert(this.animator = this.GetComponentInChildren<Animator>());
      Debug.Assert(this.weapon = this.GetComponentInChildren<Weapon>());
    }

    private void Update() {
      this.UpdateMovement();
      this.UpdateAnimation();

      if (this.FallingSpeed > 50f) {
        // we assume the player is falling endlessly, so do a respawn
        this.Respawn();
      }
    }

    private void UpdateMovement() {
      if (this.characterController.isGrounded) {
        this.velocity.y = 0f;
      } else {
        this.velocity.y -= this.gravity * Time.deltaTime;
      }

      if (this.IsAttacking) {
        this.velocity.x = 0f;
        this.velocity.z = 0f;
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

    /// <summary>
    /// Respawn at last checkpoint.
    /// </summary>
    public void Respawn() {
      this.transform.position = this.respawnPosition;
      this.velocity = Vector3.zero;
    }

    public void Attack() {
      if (this.IsAttacking) {
        Debug.Log("Player is already attacking");
        return;
      }

      this.IsAttacking = true;
      this.weapon.WeaponActive = true;
      this.animator?.SetTrigger("MeleeAttack");

      this.Invoke("OnAttackEnd", this.weapon.Cooldown);
    }

    private void OnAttackEnd() {
      this.weapon.WeaponActive = false;
      this.IsAttacking = false;
    }
  }
}
