using System.Threading.Tasks;

using UnityEngine;

using Tenry.BehaviorTree.Runtime;

namespace Tenry.DemoActionRpg {
  public class SeekNode : ActionNode {
    private EnemyBehavior enemyBehaviour;

    private PlayerController controller;

    protected override void OnStart() {
      this.enemyBehaviour = this.GameObject.GetComponent<EnemyBehavior>();
      this.controller = this.GameObject.GetComponent<PlayerController>();
    }

    protected override void OnEnd() {}

    protected override NodeStatus OnUpdate() {
      var seekFor = this.enemyBehaviour.SeekFor;
      var directionVector = seekFor - this.GameObject.transform.position;
      var direction = Vector3.SignedAngle(Vector3.forward, directionVector.normalized, Vector3.up);

      this.controller.Move(direction);

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

    private PlayerController controller;

    public Vector3 SeekFor { get; set; }

    private void Awake() {
      this.damage = this.GetComponentInChildren<Damageable>();
      this.animator = this.GetComponent<Animator>();
      this.controller = this.GetComponent<PlayerController>();
    }

    private void Start() {
      
    }

    private void Update() {
      
    }

    private void OnEnable() {
      if (this.damage != null) {
        this.damage.Damaged.AddListener(this.OnDamage);
        this.damage.Destroyed.AddListener(this.Perish);
      }
    }

    private void OnDisable() {
      if (this.damage != null) {
        this.damage.Damaged.RemoveListener(this.OnDamage);
        this.damage.Destroyed.RemoveListener(this.Perish);
      }
    }

    public async void Perish() {
      if (this.animator == null) {
        return;
      }

      this.animator.SetInteger("DamageType", 1);
      this.animator.SetTrigger("Damage");

      await Task.Delay(500);
      Destroy(this.gameObject);

      if (this.perishEffectPrefab != null) {
        Instantiate(this.perishEffectPrefab, this.transform.position, Quaternion.identity);
      }
    }

    private void OnDamage(int amount) {
      if (this.damage.IsDead) {
        return;
      }

      if (this.animator == null) {
        return;
      }

      this.animator.SetInteger("DamageType", 0);
      this.animator.SetTrigger("Damage");
    }

    private void AlertObservers() {
      Debug.Log("AlertObservers");
    }
  }
}
