using System.Threading.Tasks;

using UnityEngine;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.DemoActionRpg {
  public class SeekNode : ActionNode {
    private EnemyBehavior enemyBehaviour;

    private PlayerController controller;

    protected override void OnStart() {
      enemyBehaviour = GameObject.GetComponent<EnemyBehavior>();
      controller = GameObject.GetComponent<PlayerController>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      var seekFor = enemyBehaviour.SeekFor;
      var directionVector = seekFor - GameObject.transform.position;
      var direction = Vector3.SignedAngle(Vector3.forward, directionVector.normalized, Vector3.up);

      controller.Move(direction);

      return NodeStatus.Running;
    }
  }

  public class EnemyBehavior : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private GameObject perishEffectPrefab;
    #endregion

    private Damageable damage;

    private Animator animator;

    public Vector3 SeekFor { get; set; }

    private void Awake() {
      damage = GetComponentInChildren<Damageable>();
      animator = GetComponent<Animator>();
    }

    private void Start() {
      
    }

    private void Update() {
      
    }

    private void OnEnable() {
      if (damage != null) {
        damage.Damaged.AddListener(OnDamage);
        damage.Destroyed.AddListener(Perish);
      }
    }

    private void OnDisable() {
      if (damage != null) {
        damage.Damaged.RemoveListener(OnDamage);
        damage.Destroyed.RemoveListener(Perish);
      }
    }

    public async void Perish() {
      if (animator == null) {
        return;
      }

      animator.SetInteger("DamageType", 1);
      animator.SetTrigger("Damage");

      await Task.Delay(500);
      Destroy(gameObject);

      if (perishEffectPrefab != null) {
        Instantiate(perishEffectPrefab, transform.position, Quaternion.identity);
      }
    }

    private void OnDamage(int amount) {
      if (damage.IsDead) {
        return;
      }

      if (animator == null) {
        return;
      }

      animator.SetInteger("DamageType", 0);
      animator.SetTrigger("Damage");
    }

    private void AlertObservers() {
      Debug.Log("AlertObservers");
    }
  }
}
