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
    private Pool.ObjectPoolLink bombPool;

    [SerializeField]
    private ObjectSpawner boomerangSpawner;
    #endregion

    private CharacterController characterController;

    private Animator animator;

    /// Current direction in degrees.
    private float direction = 0f;

    private float speed = 0f;

    private float verticalSpeed = 0f;

    private float FallingSpeed {
      get {
        return -verticalSpeed;
      }
      set {
        verticalSpeed = -value;
      }
    }

    private bool IsFalling => FallingSpeed > 0f;

    private Vector3 respawnPosition;

    private DamageDealer weapon;

    private bool didMove = false;

    public float MaxMoveSpeed => moveSpeed;

    public bool IsAttacking { get; set; }

    private void Awake() {
      respawnPosition = transform.position;

      characterController = GetComponent<CharacterController>();
      Debug.Assert(characterController != null);
      animator = GetComponentInChildren<Animator>();
      Debug.Assert(animator != null);
      weapon = GetComponentInChildren<DamageDealer>();
      weapon.gameObject.SetActive(false);
    }

    private void LateUpdate() {
      if (!didMove) {
        Decelarate();
      } else {
        didMove = false;
      }

      UpdateMovement();
      UpdateAnimation();

      if (FallingSpeed > 50f) {
        // we assume the player is falling endlessly, so do a respawn
        Respawn();
      }
    }

    private void UpdateMovement() {
      if (characterController.isGrounded) {
        StopFalling();
      } else {
        FallingSpeed += gravity * Time.deltaTime;
      }

      if (IsAttacking) {
        speed = 0f;
      }

      var moveVector = transform.forward * speed + Vector3.down * FallingSpeed;
      characterController.Move(moveVector * Time.deltaTime);
    }

    private void UpdateAnimation() {
      animator?.SetFloat("Speed", speed);
    }

    private void Accelerate(float desiredSpeed) {
      speed = Mathf.MoveTowards(speed, desiredSpeed, acceleration * Time.deltaTime);
    }

    private void Decelarate() {
      Accelerate(0f);
    }

    private void Turn(float desiredDirection) {
      direction = Mathf.MoveTowardsAngle(direction, desiredDirection, angularSpeed * Time.deltaTime);

      var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(direction, Vector3.up) * Vector3.forward, Vector3.up);
      transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * Time.deltaTime);
    }

    public void Move(float direction, float speed = 1f) {
      var desiredSpeed = MaxMoveSpeed * speed;
      Accelerate(desiredSpeed);
      Turn(direction);
      didMove = true;
    }

    public void StopFalling() {
      if (IsFalling) {
        verticalSpeed = 0f;
      }
    }

    /// <summary>
    /// Respawn at last checkpoint.
    /// </summary>
    public void Respawn() {
      transform.position = respawnPosition;
      speed = 0f;
      verticalSpeed = 0f;
    }

    public async Task Attack() {
      if (IsAttacking) {
        return;
      }

      IsAttacking = true;
      weapon.gameObject.SetActive(true);
      animator?.SetTrigger("Attack");

      await Task.Delay(Mathf.RoundToInt(0.5f * 1000));

      weapon.gameObject.SetActive(false);
      IsAttacking = false;
    }

    public void UseItem1() {
      var bomb = bombPool.Pool.Get();
      bomb.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    public void UseItem2() {
      boomerangSpawner.Spawn(out var go);
      var boomerang = go.GetComponent<Boomerang>();
      boomerang.SetHome(boomerangSpawner.transform);
    }
  }
}
