using System.Threading.Tasks;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float moveSpeed = 10f;

    /// Model rotation in degrees per second.
    [SerializeField]
    private float angularSpeed = 360f * 2f;

    [SerializeField]
    private float acceleration = 15f;

    [SerializeField]
    private float gravity = 10f;

    [SerializeField]
    private Transform model;

    [SerializeField]
    private GameObject bombPrefab;
    #endregion

    private CharacterController characterController;

    private Animator animator;

    /// Current direction in degrees.
    private float direction = 0f;

    private float speed = 0f;

    private float verticalSpeed = 0f;

    private float FallingSpeed {
      get {
        return -this.verticalSpeed;
      }
      set {
        this.verticalSpeed = -value;
      }
    }

    private bool IsFalling => this.FallingSpeed > 0f;

    private Vector3 respawnPosition;

    private DamageDealer weapon;

    private bool didMove = false;

    public float MaxMoveSpeed => this.moveSpeed;

    public bool IsAttacking { get; set; }

    public Vector3 MovementDirection => Quaternion.AngleAxis(this.direction, Vector3.up) * Vector3.forward;

    private void Awake() {
      this.respawnPosition = this.transform.position;

      Debug.Assert(this.model != null);
      this.characterController = this.GetComponent<CharacterController>();
      Debug.Assert(this.characterController != null);
      this.animator = this.GetComponentInChildren<Animator>();
      Debug.Assert(this.animator != null);
      this.weapon = this.GetComponentInChildren<DamageDealer>();
    }

    private void LateUpdate() {
      if (!this.didMove) {
        this.Decelarate();
      } else {
        this.didMove = false;
      }

      this.UpdateMovement();
      this.UpdateAnimation();

      if (this.FallingSpeed > 50f) {
        // we assume the player is falling endlessly, so do a respawn
        this.Respawn();
      }
    }

    private void UpdateMovement() {
      if (this.characterController.isGrounded) {
        this.StopFalling();
      } else {
        this.FallingSpeed += this.gravity * Time.deltaTime;
      }

      if (this.IsAttacking) {
        this.speed = 0f;
      }

      var moveVector = this.MovementDirection * this.speed + Vector3.down * this.FallingSpeed;
      this.characterController.Move(moveVector * Time.deltaTime);
    }

    private void UpdateAnimation() {
      this.animator?.SetFloat("Speed", this.speed);
    }

    private void Accelerate(float desiredSpeed) {
      this.speed = Mathf.MoveTowards(this.speed, desiredSpeed, this.acceleration * Time.deltaTime);
    }

    private void Decelarate() {
      this.Accelerate(0f);
    }

    private void Turn(float desiredDirection) {
      this.direction = Mathf.MoveTowardsAngle(this.direction, desiredDirection, this.angularSpeed * Time.deltaTime);

      var targetRotation = Quaternion.LookRotation(this.MovementDirection, Vector3.up);
      this.model.transform.rotation = Quaternion.RotateTowards(this.model.transform.rotation, targetRotation, this.angularSpeed * Time.deltaTime);
    }

    public void Move(float direction, float speed = 1f) {
      var desiredSpeed = this.MaxMoveSpeed * speed;
      this.Accelerate(desiredSpeed);
      this.Turn(direction);
      this.didMove = true;
    }

    public void StopFalling() {
      if (this.IsFalling) {
        this.verticalSpeed = 0f;
      }
    }

    /// <summary>
    /// Respawn at last checkpoint.
    /// </summary>
    public void Respawn() {
      this.transform.position = this.respawnPosition;
      this.speed = 0f;
      this.verticalSpeed = 0f;
    }

    public async Task Attack() {
      if (this.IsAttacking) {
        return;
      }

      Debug.Log($"{this.gameObject.name} attacks!");

      this.IsAttacking = true;
      this.weapon.WeaponActive = true;
      this.animator?.SetTrigger("Attack");

      await Task.Delay(Mathf.RoundToInt(0.5f * 1000));

      this.weapon.WeaponActive = false;
      this.IsAttacking = false;
    }

    public void UseItem1() {
      Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
  }
}
